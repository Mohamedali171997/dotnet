export interface LoginRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    token: string;
    expiration: string;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
}

export interface User {
    email: string;
    role: string;
    firstName: string;
    lastName: string;
}