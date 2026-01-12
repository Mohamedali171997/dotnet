import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { StudentsService } from '../../../core/services/students.service';
import { Student } from '../../../core/models/models';

@Component({
    selector: 'app-student-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './student-list.component.html',
    styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements OnInit {
    students: Student[] = [];
    loading = true;

    constructor(private studentsService: StudentsService) { }

    ngOnInit(): void {
        this.loadStudents();
    }

    loadStudents(): void {
        this.studentsService.getAll().subscribe({
            next: (data) => {
                this.students = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error loading students', err);
                this.loading = false;
            }
        });
    }

    deleteStudent(id: number): void {
        if (confirm('Êtes-vous sûr de vouloir supprimer cet étudiant ?')) {
            this.studentsService.delete(id).subscribe(() => {
                this.students = this.students.filter(s => s.id !== id);
            });
        }
    }
}
