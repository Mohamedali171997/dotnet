import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Grade } from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class GradesService {
    private apiUrl = `${environment.apiUrl}/grades`;
    private cache$: Observable<Grade[]> | null = null;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Grade[]> {
        if (!this.cache$) {
            this.cache$ = this.http.get<Grade[]>(this.apiUrl).pipe(
                shareReplay(1)
            );
        }
        return this.cache$;
    }

    getById(id: number): Observable<Grade> {
        return this.http.get<Grade>(`${this.apiUrl}/${id}`);
    }

    create(grade: any): Observable<Grade> {
        return this.http.post<Grade>(this.apiUrl, grade).pipe(
            tap(() => this.cache$ = null)
        );
    }

    update(id: number, grade: any): Observable<Grade> {
        return this.http.put<Grade>(`${this.apiUrl}/${id}`, grade).pipe(
            tap(() => this.cache$ = null)
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.cache$ = null)
        );
    }

    getByStudentId(studentId: number): Observable<Grade[]> {
        return this.http.get<Grade[]>(`${this.apiUrl}/student/${studentId}`);
    }
}
