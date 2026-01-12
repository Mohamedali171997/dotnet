import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Class, Student, Course } from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class ClassesService {
    private apiUrl = `${environment.apiUrl}/classes`;
    private cache$: Observable<Class[]> | null = null;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Class[]> {
        if (!this.cache$) {
            this.cache$ = this.http.get<Class[]>(this.apiUrl).pipe(
                shareReplay(1)
            );
        }
        return this.cache$;
    }

    getById(id: number): Observable<Class> {
        return this.http.get<Class>(`${this.apiUrl}/${id}`);
    }

    create(classObj: any): Observable<Class> {
        return this.http.post<Class>(this.apiUrl, classObj).pipe(
            tap(() => this.cache$ = null)
        );
    }

    update(id: number, classObj: any): Observable<Class> {
        return this.http.put<Class>(`${this.apiUrl}/${id}`, classObj).pipe(
            tap(() => this.cache$ = null)
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.cache$ = null)
        );
    }

    getClassStudents(id: number): Observable<Student[]> {
        return this.http.get<Student[]>(`${this.apiUrl}/${id}/students`);
    }

    getClassCourses(id: number): Observable<Course[]> {
        return this.http.get<Course[]>(`${this.apiUrl}/${id}/courses`);
    }
}
