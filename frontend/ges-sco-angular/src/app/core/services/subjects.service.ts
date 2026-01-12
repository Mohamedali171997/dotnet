import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Subject } from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class SubjectsService {
    private apiUrl = `${environment.apiUrl}/subjects`;
    private cache$: Observable<Subject[]> | null = null;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Subject[]> {
        if (!this.cache$) {
            this.cache$ = this.http.get<Subject[]>(this.apiUrl).pipe(
                shareReplay(1)
            );
        }
        return this.cache$;
    }

    getById(id: number): Observable<Subject> {
        return this.http.get<Subject>(`${this.apiUrl}/${id}`);
    }

    create(subject: any): Observable<Subject> {
        return this.http.post<Subject>(this.apiUrl, subject).pipe(
            tap(() => this.cache$ = null)
        );
    }

    update(id: number, subject: any): Observable<Subject> {
        return this.http.put<Subject>(`${this.apiUrl}/${id}`, subject).pipe(
            tap(() => this.cache$ = null)
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.cache$ = null)
        );
    }
}
