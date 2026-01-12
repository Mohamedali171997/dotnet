import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Student, StudentGradesReport, StudentAttendanceReport } from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class StudentsService {
    private apiUrl = `${environment.apiUrl}/students`;
    private cache$: Observable<Student[]> | null = null;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Student[]> {
        if (!this.cache$) {
            this.cache$ = this.http.get<Student[]>(this.apiUrl).pipe(
                shareReplay(1)
            );
        }
        return this.cache$;
    }

    getById(id: number): Observable<Student> {
        return this.http.get<Student>(`${this.apiUrl}/${id}`);
    }

    create(student: any): Observable<Student> {
        return this.http.post<Student>(this.apiUrl, student).pipe(
            tap(() => this.cache$ = null)
        );
    }

    update(id: number, student: any): Observable<Student> {
        return this.http.put<Student>(`${this.apiUrl}/${id}`, student).pipe(
            tap(() => this.cache$ = null)
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.cache$ = null)
        );
    }

    getGradesReport(id: number): Observable<StudentGradesReport> {
        return this.http.get<StudentGradesReport>(`${this.apiUrl}/${id}/grades`);
    }

    getAttendanceReport(id: number): Observable<StudentAttendanceReport> {
        return this.http.get<StudentAttendanceReport>(`${this.apiUrl}/${id}/attendance`);
    }
}
