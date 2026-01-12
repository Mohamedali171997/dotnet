import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            {
                path: 'dashboard',
                loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
            },

            // Students Routes
            {
                path: 'students',
                loadComponent: () => import('./features/students/student-list/student-list.component').then(m => m.StudentListComponent)
            },
            {
                path: 'students/new',
                loadComponent: () => import('./features/students/student-form/student-form.component').then(m => m.StudentFormComponent)
            },
            {
                path: 'students/:id/edit',
                loadComponent: () => import('./features/students/student-form/student-form.component').then(m => m.StudentFormComponent)
            },

            // Teachers Routes
            {
                path: 'teachers',
                loadComponent: () => import('./features/teachers/teacher-list/teacher-list.component').then(m => m.TeacherListComponent)
            },
            {
                path: 'teachers/new',
                loadComponent: () => import('./features/teachers/teacher-form/teacher-form.component').then(m => m.TeacherFormComponent)
            },
            {
                path: 'teachers/:id/edit',
                loadComponent: () => import('./features/teachers/teacher-form/teacher-form.component').then(m => m.TeacherFormComponent)
            },

            // Classes Routes
            {
                path: 'classes',
                loadComponent: () => import('./features/classes/class-list/class-list.component').then(m => m.ClassListComponent)
            },
            {
                path: 'classes/new',
                loadComponent: () => import('./features/classes/class-form/class-form.component').then(m => m.ClassFormComponent)
            },
            {
                path: 'classes/:id/edit',
                loadComponent: () => import('./features/classes/class-form/class-form.component').then(m => m.ClassFormComponent)
            },

            // Subjects Routes (New)
            {
                path: 'subjects',
                loadComponent: () => import('./features/subjects/subject-list/subject-list.component').then(m => m.SubjectListComponent)
            },
            {
                path: 'subjects/new',
                loadComponent: () => import('./features/subjects/subject-form/subject-form.component').then(m => m.SubjectFormComponent)
            },
            {
                path: 'subjects/:id/edit',
                loadComponent: () => import('./features/subjects/subject-form/subject-form.component').then(m => m.SubjectFormComponent)
            },

            // Courses Routes
            {
                path: 'courses',
                loadComponent: () => import('./features/courses/course-list/course-list.component').then(m => m.CourseListComponent)
            },
            {
                path: 'courses/new',
                loadComponent: () => import('./features/courses/course-form/course-form.component').then(m => m.CourseFormComponent)
            },
            {
                path: 'courses/:id/edit',
                loadComponent: () => import('./features/courses/course-form/course-form.component').then(m => m.CourseFormComponent)
            },

            // Grades Routes
            {
                path: 'grades',
                loadComponent: () => import('./features/grades/grade-list/grade-list.component').then(m => m.GradeListComponent)
            },
            {
                path: 'grades/new',
                loadComponent: () => import('./features/grades/grade-form/grade-form.component').then(m => m.GradeFormComponent)
            },
            {
                path: 'grades/:id/edit',
                loadComponent: () => import('./features/grades/grade-form/grade-form.component').then(m => m.GradeFormComponent)
            },

            // Attendance Routes
            {
                path: 'attendance',
                loadComponent: () => import('./features/attendance/attendance-list/attendance-list.component').then(m => m.AttendanceListComponent)
            },
            {
                path: 'attendance/new',
                loadComponent: () => import('./features/attendance/attendance-form/attendance-form.component').then(m => m.AttendanceFormComponent)
            },
            {
                path: 'attendance/:id/edit',
                loadComponent: () => import('./features/attendance/attendance-form/attendance-form.component').then(m => m.AttendanceFormComponent)
            },
        ]
    },
    { path: '**', redirectTo: '' }
];
