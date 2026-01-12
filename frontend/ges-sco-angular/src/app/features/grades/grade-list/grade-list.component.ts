import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { GradesService } from '../../../core/services/grades.service';
import { Grade } from '../../../core/models/models';

@Component({
    selector: 'app-grade-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './grade-list.component.html',
    styleUrls: ['./grade-list.component.css']
})
export class GradeListComponent implements OnInit {
    grades: Grade[] = [];
    loading = true;

    constructor(private gradesService: GradesService) { }

    ngOnInit(): void {
        this.loadGrades();
    }

    loadGrades(): void {
        this.gradesService.getAll().subscribe({
            next: (data) => {
                this.grades = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error loading grades', err);
                this.loading = false;
            }
        });
    }

    deleteGrade(id: number): void {
        if (confirm('Êtes-vous sûr de vouloir supprimer cette note ?')) {
            this.gradesService.delete(id).subscribe(() => {
                this.grades = this.grades.filter(g => g.id !== id);
            });
        }
    }
}
