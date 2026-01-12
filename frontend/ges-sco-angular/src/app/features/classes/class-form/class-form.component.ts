import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { ClassesService } from '../../../core/services/classes.service';

@Component({
    selector: 'app-class-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './class-form.component.html',
    styleUrls: ['./class-form.component.css']
})
export class ClassFormComponent implements OnInit {
    classForm: FormGroup;
    isEditMode = false;
    classId: number | null = null;
    submitted = false;

    constructor(
        private fb: FormBuilder,
        private classesService: ClassesService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.classForm = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(50)]],
            level: ['', Validators.required],
            academicYear: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{4}$/)]],
            capacity: [30, [Validators.required, Validators.min(1)]]
        });
    }

    ngOnInit(): void {
        this.classId = Number(this.route.snapshot.paramMap.get('id'));
        if (this.classId) {
            this.isEditMode = true;
            this.loadClass(this.classId);
        }
    }

    loadClass(id: number): void {
        this.classesService.getById(id).subscribe(cls => {
            this.classForm.patchValue({
                name: cls.name,
                level: cls.level,
                academicYear: cls.academicYear,
                capacity: cls.capacity
            });
        });
    }

    onSubmit(): void {
        this.submitted = true;
        if (this.classForm.invalid) return;

        if (this.isEditMode && this.classId) {
            this.classesService.update(this.classId, this.classForm.value).subscribe({
                next: () => this.router.navigate(['/classes']),
                error: (err) => console.error(err)
            });
        } else {
            this.classesService.create(this.classForm.value).subscribe({
                next: () => this.router.navigate(['/classes']),
                error: (err) => console.error(err)
            });
        }
    }
}
