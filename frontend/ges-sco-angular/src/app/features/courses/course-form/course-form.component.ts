import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { CoursesService } from '../../../core/services/courses.service';
import { SubjectsService } from '../../../core/services/subjects.service';
import { ClassesService } from '../../../core/services/classes.service';
import { TeachersService } from '../../../core/services/teachers.service';
import { Subject, Class, Teacher } from '../../../core/models/models';

@Component({
    selector: 'app-course-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './course-form.component.html',
    styleUrls: ['./course-form.component.css']
})
export class CourseFormComponent implements OnInit {
    courseForm: FormGroup;
    isEditMode = false;
    courseId: number | null = null;
    submitted = false;

    subjects: Subject[] = [];
    classes: Class[] = [];
    teachers: Teacher[] = [];

    constructor(
        private fb: FormBuilder,
        private coursesService: CoursesService,
        private subjectsService: SubjectsService,
        private classesService: ClassesService,
        private teachersService: TeachersService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.courseForm = this.fb.group({
            subjectId: ['', Validators.required],
            classId: ['', Validators.required],
            teacherId: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        this.loadData();
        this.courseId = Number(this.route.snapshot.paramMap.get('id'));
        if (this.courseId) {
            this.isEditMode = true;
            this.loadCourse(this.courseId);
        }
    }

    loadData(): void {
        this.subjectsService.getAll().subscribe(data => this.subjects = data);
        this.classesService.getAll().subscribe(data => this.classes = data);
        this.teachersService.getAll().subscribe(data => this.teachers = data);
    }

    loadCourse(id: number): void {
        this.coursesService.getById(id).subscribe(course => {
            this.courseForm.patchValue({
                subjectId: course.subjectId,
                classId: course.classId,
                teacherId: course.teacherId
            });
        });
    }

    onSubmit(): void {
        this.submitted = true;
        if (this.courseForm.invalid) return;

        if (this.isEditMode && this.courseId) {
            this.coursesService.update(this.courseId, this.courseForm.value).subscribe({
                next: () => this.router.navigate(['/courses']),
                error: (err) => console.error(err)
            });
        } else {
            this.coursesService.create(this.courseForm.value).subscribe({
                next: () => this.router.navigate(['/courses']),
                error: (err) => console.error(err)
            });
        }
    }
}
