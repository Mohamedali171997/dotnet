import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { TeachersService } from '../../../core/services/teachers.service';

@Component({
    selector: 'app-teacher-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './teacher-form.component.html',
    styleUrls: ['./teacher-form.component.css']
})
export class TeacherFormComponent implements OnInit {
    teacherForm: FormGroup;
    isEditMode = false;
    teacherId: number | null = null;
    submitted = false;

    constructor(
        private fb: FormBuilder,
        private teachersService: TeachersService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.teacherForm = this.fb.group({
            firstName: ['', [Validators.required, Validators.maxLength(50)]],
            lastName: ['', [Validators.required, Validators.maxLength(50)]],
            email: ['', [Validators.required, Validators.email]],
            password: [''], // Required only for creation
            specialization: [''],
            qualification: [''],
            hireDate: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        this.teacherId = Number(this.route.snapshot.paramMap.get('id'));
        if (this.teacherId) {
            this.isEditMode = true;
            this.loadTeacher(this.teacherId);
        } else {
            this.teacherForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
        }
    }

    loadTeacher(id: number): void {
        this.teachersService.getById(id).subscribe(teacher => {
            this.teacherForm.patchValue({
                firstName: teacher.firstName,
                lastName: teacher.lastName,
                email: teacher.email,
                specialization: teacher.specialization,
                qualification: teacher.qualification,
                hireDate: teacher.hireDate.split('T')[0]
            });
        });
    }

    onSubmit(): void {
        this.submitted = true;
        if (this.teacherForm.invalid) return;

        const teacherData = this.teacherForm.value;

        if (this.isEditMode && this.teacherId) {
            delete teacherData.password;
            this.teachersService.update(this.teacherId, teacherData).subscribe({
                next: () => this.router.navigate(['/teachers']),
                error: (err) => console.error(err)
            });
        } else {
            this.teachersService.create(teacherData).subscribe({
                next: () => this.router.navigate(['/teachers']),
                error: (err) => console.error(err)
            });
        }
    }
}
