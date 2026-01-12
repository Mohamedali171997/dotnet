import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ClassesService } from '../../../core/services/classes.service';
import { Class } from '../../../core/models/models';

@Component({
    selector: 'app-class-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './class-list.component.html',
    styleUrls: ['./class-list.component.css']
})
export class ClassListComponent implements OnInit {
    classes: Class[] = [];
    loading = true;

    constructor(private classesService: ClassesService) { }

    ngOnInit(): void {
        this.loadClasses();
    }

    loadClasses(): void {
        this.classesService.getAll().subscribe({
            next: (data) => {
                this.classes = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error loading classes', err);
                this.loading = false;
            }
        });
    }

    deleteClass(id: number): void {
        if (confirm('Êtes-vous sûr de vouloir supprimer cette classe ?')) {
            this.classesService.delete(id).subscribe(() => {
                this.classes = this.classes.filter(c => c.id !== id);
            });
        }
    }
}
