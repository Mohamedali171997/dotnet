import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TeachersService } from '../../../core/services/teachers.service';
import { Teacher } from '../../../core/models/models';

@Component({
    selector: 'app-teacher-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './teacher-list.component.html',
    styleUrls: ['./teacher-list.component.css']
})
export class TeacherListComponent implements OnInit {
    teachers: Teacher[] = [];
    loading = true;

    constructor(private teachersService: TeachersService) { }

    ngOnInit(): void {
        this.loadTeachers();
    }

    loadTeachers(): void {
        this.teachersService.getAll().subscribe({
            next: (data) => {
                this.teachers = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error loading teachers', err);
                this.loading = false;
            }
        });
    }

    deleteTeacher(id: number): void {
        if (confirm('Êtes-vous sûr de vouloir supprimer cet enseignant ?')) {
            this.teachersService.delete(id).subscribe(() => {
                this.teachers = this.teachers.filter(t => t.id !== id);
            });
        }
    }
}
