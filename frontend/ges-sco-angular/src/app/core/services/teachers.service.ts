import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Teacher, Course } from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class TeachersService {
    private apiUrl = `${environment.apiUrl}/teachers`;
    private cache$: Observable<Teacher[]> | null = null;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Teacher[]> {
        if (!this.cache$) {
            this.cache$ = this.http.get<Teacher[]>(this.apiUrl).pipe(
                shareReplay(1)
            );
        }
        return this.cache$;
    }

    getById(id: number): Observable<Teacher> {
        return this.http.get<Teacher>(`${this.apiUrl}/${id}`);
    }

    create(teacher: any): Observable<Teacher> {
        return this.http.post<Teacher>(this.apiUrl, teacher).pipe(
            tap(() => this.cache$ = null)
        );
    }

    update(id: number, teacher: any): Observable<Teacher> {
        return this.http.put<Teacher>(`${this.apiUrl}/${id}`, teacher).pipe(
            tap(() => this.cache$ = null)
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.cache$ = null)
        );
    }

    getTeacherCourses(id: number): Observable<Course[]> {
        return this.http.get<Course[]>(`${this.apiUrl}/${id}/courses`);
    }
}
