import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { AttendanceService } from '../../../core/services/attendance.service';
import { StudentsService } from '../../../core/services/students.service';
import { CoursesService } from '../../../core/services/courses.service';
import { Student, Course } from '../../../core/models/models';

@Component({
    selector: 'app-attendance-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './attendance-form.component.html',
    styleUrls: ['./attendance-form.component.css']
})
export class AttendanceFormComponent implements OnInit {
    attendanceForm: FormGroup;
    isEditMode = false;
    attendanceId: number | null = null;
    submitted = false;

    students: Student[] = [];
    courses: Course[] = [];

    constructor(
        private fb: FormBuilder,
        private attendanceService: AttendanceService,
        private studentsService: StudentsService,
        private coursesService: CoursesService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.attendanceForm = this.fb.group({
            studentId: ['', Validators.required],
            courseId: ['', Validators.required],
            date: [new Date().toISOString().split('T')[0], Validators.required],
            status: ['Present', Validators.required],
            isJustified: [false],
            justification: ['']
        });
    }

    ngOnInit(): void {
        this.loadData();
        this.attendanceId = Number(this.route.snapshot.paramMap.get('id'));
        if (this.attendanceId) {
            this.isEditMode = true;
            this.loadAttendance(this.attendanceId);
        }
    }

    loadData(): void {
        this.studentsService.getAll().subscribe(data => this.students = data);
        this.coursesService.getAll().subscribe(data => this.courses = data);
    }

    loadAttendance(id: number): void {
        this.attendanceService.getById(id).subscribe(att => {
            this.attendanceForm.patchValue({
                studentId: att.studentId,
                courseId: att.courseId,
                date: att.date.split('T')[0],
                status: att.status,
                isJustified: att.isJustified,
                justification: att.justification
            });
        });
    }

    onSubmit(): void {
        this.submitted = true;
        if (this.attendanceForm.invalid) return;

        if (this.isEditMode && this.attendanceId) {
            this.attendanceService.update(this.attendanceId, this.attendanceForm.value).subscribe({
                next: () => this.router.navigate(['/attendance']),
                error: (err) => console.error(err)
            });
        } else {
            this.attendanceService.create(this.attendanceForm.value).subscribe({
                next: () => this.router.navigate(['/attendance']),
                error: (err) => console.error(err)
            });
        }
    }
}
