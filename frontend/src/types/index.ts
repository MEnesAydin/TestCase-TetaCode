export interface User {
  id: string;
  userName: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

export interface RegisterRequest {
  userName: string;
  password: string;
}

export interface Grade {
  id: string;
  courseName: string;
  description: string;
  fileName: string;
  isDeleted: boolean;
  createdAt: string;
  updatedAt: string | null;
  deletedAt: string | null;
}

export interface PagedResult<T> {
  items: T[];
  metadata?: {
    pagination?: {
      totalCount: number;
      totalPages: number;
      currentPage: number;
      pageSize: number;
      hasPrevious: boolean;
      hasNext: boolean;
    };
  };
}

export interface ApiResponse<T> {
  data?: T;
  isSuccessful: boolean;
  errorMessages?: string[];
}

export interface GradeFormData {
  id?: string;
  courseName: string;
  description: string;
  file?: File | null;
}