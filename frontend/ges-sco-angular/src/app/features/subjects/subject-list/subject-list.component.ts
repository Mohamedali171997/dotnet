import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SubjectsService } from '../../../core/services/subjects.service';
import { Subject } from '../../../core/models/models';

@Component({
    selector: 'app-subject-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './subject-list.component.html',
    styleUrls: ['./subject-list.component.css']
})
export class SubjectListComponent implements OnInit {
    subjects: Subject[] = [];
    loading = true;

    constructor(private subjectsService: SubjectsService) { }

    ngOnInit(): void {
        this.loadSubjects();
    }

    loadSubjects(): void {
        this.subjectsService.getAll().subscribe({
            next: (data) => {
                this.subjects = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error loading subjects', err);
                this.loading = false;
            }
        });
    }

    deleteSubject(id: number): void {
        if (confirm('Êtes-vous sûr de vouloir supprimer cette matière ?')) {
            this.subjectsService.delete(id).subscribe(() => {
                this.subjects = this.subjects.filter(s => s.id !== id);
            });
        }
    }
}
