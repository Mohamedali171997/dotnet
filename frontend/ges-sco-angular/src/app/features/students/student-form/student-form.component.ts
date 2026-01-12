import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { StudentsService } from '../../../core/services/students.service';
import { ClassesService } from '../../../core/services/classes.service';
import { Class } from '../../../core/models/models';

@Component({
    selector: 'app-student-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './student-form.component.html',
    styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent implements OnInit {
    studentForm: FormGroup;
    isEditMode = false;
    studentId: number | null = null;
    classes: Class[] = [];
    submitted = false;

    constructor(
        private fb: FormBuilder,
        private studentsService: StudentsService,
        private classesService: ClassesService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.studentForm = this.fb.group({
            firstName: ['', [Validators.required, Validators.maxLength(50)]],
            lastName: ['', [Validators.required, Validators.maxLength(50)]],
            email: ['', [Validators.required, Validators.email]],
            password: [''], // Required only for creation
            dateOfBirth: ['', Validators.required],
            address: [''],
            phone: [''],
            classId: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        this.loadClasses();

        this.studentId = Number(this.route.snapshot.paramMap.get('id'));
        if (this.studentId) {
            this.isEditMode = true;
            this.loadStudent(this.studentId);
        } else {
            this.studentForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
        }
    }

    loadClasses(): void {
        this.classesService.getAll().subscribe(data => this.classes = data);
    }

    loadStudent(id: number): void {
        this.studentsService.getById(id).subscribe(student => {
            this.studentForm.patchValue({
                firstName: student.firstName,
                lastName: student.lastName,
                email: student.email,
                dateOfBirth: student.dateOfBirth.split('T')[0], // Format for input date
                address: student.address,
                phone: student.phone,
                classId: student.classId
            });
        });
    }

    onSubmit(): void {
        this.submitted = true;
        if (this.studentForm.invalid) return;

        const formValue = this.studentForm.value;
        const studentData = {
            ...formValue,
            classId: Number(formValue.classId)
        };

        if (this.isEditMode && this.studentId) {
            // Remove password if empty in edit mode (backend handles optional password update logic if implemented, but usually separate)
            // For now, assuming update DTO doesn't require password.
            delete studentData.password;

            this.studentsService.update(this.studentId, studentData).subscribe({
                next: () => this.router.navigate(['/students']),
                error: (err) => console.error(err)
            });
        } else {
            this.studentsService.create(studentData).subscribe({
                next: () => this.router.navigate(['/students']),
                error: (err) => console.error(err)
            });
        }
    }
}
