import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Course, Grade, Attendance } from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class CoursesService {
    private apiUrl = `${environment.apiUrl}/courses`;
    private cache$: Observable<Course[]> | null = null;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Course[]> {
        if (!this.cache$) {
            this.cache$ = this.http.get<Course[]>(this.apiUrl).pipe(
                shareReplay(1)
            );
        }
        return this.cache$;
    }

    getById(id: number): Observable<Course> {
        return this.http.get<Course>(`${this.apiUrl}/${id}`);
    }

    create(course: any): Observable<Course> {
        return this.http.post<Course>(this.apiUrl, course).pipe(
            tap(() => this.cache$ = null)
        );
    }

    update(id: number, course: any): Observable<Course> {
        return this.http.put<Course>(`${this.apiUrl}/${id}`, course).pipe(
            tap(() => this.cache$ = null)
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.cache$ = null)
        );
    }

    getCourseGrades(id: number): Observable<Grade[]> {
        return this.http.get<Grade[]>(`${this.apiUrl}/${id}/grades`);
    }

    getCourseAttendance(id: number): Observable<Attendance[]> {
        return this.http.get<Attendance[]>(`${this.apiUrl}/${id}/attendance`);
    }
}
