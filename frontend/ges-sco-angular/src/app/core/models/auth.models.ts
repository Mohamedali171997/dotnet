export interface LoginRequest {
    username: string;
    password: string;
}

export interface AuthResponse {
    token: string;
    expiration: string;
    username: string;
    role: string;
    firstName: string;
    lastName: string;
}

export interface User {
    username: string;
    role: string;
    firstName: string;
    lastName: string;
}