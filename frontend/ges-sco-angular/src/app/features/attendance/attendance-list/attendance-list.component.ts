import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AttendanceService } from '../../../core/services/attendance.service';
import { Attendance } from '../../../core/models/models';

@Component({
    selector: 'app-attendance-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './attendance-list.component.html',
    styleUrls: ['./attendance-list.component.css']
})
export class AttendanceListComponent implements OnInit {
    attendances: Attendance[] = [];
    loading = true;

    constructor(private attendanceService: AttendanceService) { }

    ngOnInit(): void {
        this.loadAttendances();
    }

    loadAttendances(): void {
        this.attendanceService.getAll().subscribe({
            next: (data) => {
                this.attendances = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error loading attendance', err);
                this.loading = false;
            }
        });
    }

    deleteAttendance(id: number): void {
        if (confirm('Êtes-vous sûr de vouloir supprimer cette présence ?')) {
            this.attendanceService.delete(id).subscribe(() => {
                this.attendances = this.attendances.filter(a => a.id !== id);
            });
        }
    }
}
