import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { GradesService } from '../../../core/services/grades.service';
import { StudentsService } from '../../../core/services/students.service';
import { CoursesService } from '../../../core/services/courses.service';
import { Student, Course } from '../../../core/models/models';

@Component({
    selector: 'app-grade-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './grade-form.component.html',
    styleUrls: ['./grade-form.component.css']
})
export class GradeFormComponent implements OnInit {
    gradeForm: FormGroup;
    isEditMode = false;
    gradeId: number | null = null;
    submitted = false;

    students: Student[] = [];
    courses: Course[] = [];

    constructor(
        private fb: FormBuilder,
        private gradesService: GradesService,
        private studentsService: StudentsService,
        private coursesService: CoursesService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.gradeForm = this.fb.group({
            studentId: ['', Validators.required],
            courseId: ['', Validators.required],
            value: ['', [Validators.required, Validators.min(0), Validators.max(20)]],
            coefficient: [1, [Validators.required, Validators.min(0.1)]],
            type: ['Exam', Validators.required],
            date: [new Date().toISOString().split('T')[0], Validators.required],
            comment: ['']
        });
    }

    ngOnInit(): void {
        this.loadData();
        this.gradeId = Number(this.route.snapshot.paramMap.get('id'));
        if (this.gradeId) {
            this.isEditMode = true;
            this.loadGrade(this.gradeId);
        }
    }

    loadData(): void {
        this.studentsService.getAll().subscribe(data => this.students = data);
        this.coursesService.getAll().subscribe(data => this.courses = data);
    }

    loadGrade(id: number): void {
        this.gradesService.getById(id).subscribe(grade => {
            this.gradeForm.patchValue({
                studentId: grade.studentId,
                courseId: grade.courseId,
                value: grade.value,
                coefficient: grade.coefficient,
                type: grade.type,
                date: grade.date.split('T')[0],
                comment: grade.comment
            });
        });
    }

    onSubmit(): void {
        this.submitted = true;
        if (this.gradeForm.invalid) return;

        if (this.isEditMode && this.gradeId) {
            this.gradesService.update(this.gradeId, this.gradeForm.value).subscribe({
                next: () => this.router.navigate(['/grades']),
                error: (err) => console.error(err)
            });
        } else {
            this.gradesService.create(this.gradeForm.value).subscribe({
                next: () => this.router.navigate(['/grades']),
                error: (err) => console.error(err)
            });
        }
    }
}
