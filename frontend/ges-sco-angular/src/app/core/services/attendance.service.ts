import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Attendance } from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class AttendanceService {
    private apiUrl = `${environment.apiUrl}/attendance`;
    private cache$: Observable<Attendance[]> | null = null;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Attendance[]> {
        if (!this.cache$) {
            this.cache$ = this.http.get<Attendance[]>(this.apiUrl).pipe(
                shareReplay(1)
            );
        }
        return this.cache$;
    }

    getById(id: number): Observable<Attendance> {
        return this.http.get<Attendance>(`${this.apiUrl}/${id}`);
    }

    create(attendance: any): Observable<Attendance> {
        return this.http.post<Attendance>(this.apiUrl, attendance).pipe(
            tap(() => this.cache$ = null)
        );
    }

    createBulk(bulk: any): Observable<Attendance[]> {
        return this.http.post<Attendance[]>(`${this.apiUrl}/bulk`, bulk).pipe(
            tap(() => this.cache$ = null)
        );
    }

    update(id: number, attendance: any): Observable<Attendance> {
        return this.http.put<Attendance>(`${this.apiUrl}/${id}`, attendance).pipe(
            tap(() => this.cache$ = null)
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.cache$ = null)
        );
    }
}
