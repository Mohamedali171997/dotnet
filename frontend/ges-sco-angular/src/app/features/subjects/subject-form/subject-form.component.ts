import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { SubjectsService } from '../../../core/services/subjects.service';

@Component({
    selector: 'app-subject-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './subject-form.component.html',
    styleUrls: ['./subject-form.component.css']
})
export class SubjectFormComponent implements OnInit {
    subjectForm: FormGroup;
    isEditMode = false;
    subjectId: number | null = null;
    submitted = false;

    constructor(
        private fb: FormBuilder,
        private subjectsService: SubjectsService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.subjectForm = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
            code: ['', [Validators.required, Validators.maxLength(20)]],
            coefficient: [1, [Validators.required, Validators.min(0.1)]],
            credits: [1, [Validators.required, Validators.min(0)]],
            description: ['']
        });
    }

    ngOnInit(): void {
        this.subjectId = Number(this.route.snapshot.paramMap.get('id'));
        if (this.subjectId) {
            this.isEditMode = true;
            this.loadSubject(this.subjectId);
        }
    }

    loadSubject(id: number): void {
        this.subjectsService.getById(id).subscribe(subject => {
            this.subjectForm.patchValue({
                name: subject.name,
                code: subject.code,
                coefficient: subject.coefficient,
                credits: subject.credits,
                description: subject.description
            });
        });
    }

    onSubmit(): void {
        this.submitted = true;
        if (this.subjectForm.invalid) return;

        if (this.isEditMode && this.subjectId) {
            this.subjectsService.update(this.subjectId, this.subjectForm.value).subscribe({
                next: () => this.router.navigate(['/subjects']),
                error: (err) => console.error(err)
            });
        } else {
            this.subjectsService.create(this.subjectForm.value).subscribe({
                next: () => this.router.navigate(['/subjects']),
                error: (err) => console.error(err)
            });
        }
    }
}
