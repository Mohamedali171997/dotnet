import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CoursesService } from '../../../core/services/courses.service';
import { Course } from '../../../core/models/models';

@Component({
    selector: 'app-course-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './course-list.component.html',
    styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
    courses: Course[] = [];
    loading = true;

    constructor(private coursesService: CoursesService) { }

    ngOnInit(): void {
        this.loadCourses();
    }

    loadCourses(): void {
        this.coursesService.getAll().subscribe({
            next: (data) => {
                this.courses = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error loading courses', err);
                this.loading = false;
            }
        });
    }

    deleteCourse(id: number): void {
        if (confirm('Êtes-vous sûr de vouloir supprimer ce cours ?')) {
            this.coursesService.delete(id).subscribe(() => {
                this.courses = this.courses.filter(c => c.id !== id);
            });
        }
    }
}
