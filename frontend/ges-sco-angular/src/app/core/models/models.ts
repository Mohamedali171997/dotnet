export interface User {
    id: number;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
}

export interface AuthResponse {
    token: string;
    expiration: string;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
}

export interface Student {
    id: number;
    userId: number;
    firstName: string;
    lastName: string;
    fullName: string;
    email: string;
    dateOfBirth: string;
    address?: string;
    phone?: string;
    parentContact?: string;
    medicalInfo?: string;
    studentIdentificationNumber?: string;
    classId: number;
    className: string;
    isActive: boolean;
}

export interface Teacher {
    id: number;
    userId: number;
    firstName: string;
    lastName: string;
    fullName: string;
    email: string;
    specialization?: string;
    qualification?: string;
    hireDate: string;
    isActive: boolean;
    coursesCount: number;
}

export interface Class {
    id: number;
    name: string;
    level: string;
    academicYear: string;
    capacity: number;
    studentsCount: number;
    coursesCount: number;
}

export interface Subject {
    id: number;
    name: string;
    code: string;
    coefficient: number;
    credits: number;
    description?: string;
    coursesCount: number;
}

export interface Course {
    id: number;
    subjectId: number;
    subjectName: string;
    subjectCode: string;
    classId: number;
    className: string;
    teacherId: number;
    teacherName: string;
    coefficient: number;
    credits: number;
}

export interface Grade {
    id: number;
    value: number;
    coefficient: number;
    type: string;
    comment?: string;
    date: string;
    studentId: number;
    studentName: string;
    courseId: number;
    subjectName: string;
}

export interface Attendance {
    id: number;
    date: string;
    status: 'Present' | 'Absent' | 'Late';
    isJustified: boolean;
    justification?: string;
    studentId: number;
    studentName: string;
    courseId: number;
    subjectName: string;
}

export interface DashboardStats {
    totalStudents: number;
    totalTeachers: number;
    totalClasses: number;
    totalSubjects: number;
    totalCourses: number;
    averageAttendanceRate: number;
    averageGrade: number;
    activeUsersToday: number;
}

export interface StudentGradesReport {
    studentId: number;
    studentName: string;
    className: string;
    academicYear: string;
    subjectGrades: SubjectGrades[];
    generalAverage: number;
    totalCredits: number;
    mention: string;
}

export interface SubjectGrades {
    subjectName: string;
    subjectCode: string;
    coefficient: number;
    credits: number;
    grades: Grade[];
    average: number;
    isPassed: boolean;
}

export interface StudentAttendanceReport {
    studentId: number;
    studentName: string;
    className: string;
    totalSessions: number;
    presentCount: number;
    absentCount: number;
    lateCount: number;
    justifiedAbsences: number;
    attendanceRate: number;
    attendances: Attendance[];
}
