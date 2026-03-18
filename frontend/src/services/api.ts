import axios from 'axios';
import { LoginRequest, RegisterRequest, PagedResult, Grade, ApiResponse, LoginResponse } from '../types';

const API_BASE_URL = 'http://localhost:8080';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor to handle errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authApi = {
  login: async (data: LoginRequest) => {
    const response = await api.post<ApiResponse<LoginResponse>>('/auth/login', data);
    return response.data;
  },
  
  register: async (data: RegisterRequest) => {
    const response = await api.post<ApiResponse<string>>('/users/register', data);
    return response.data;
  },
};

export const gradeApi = {
  getAll: async (page: number = 1, pageSize: number = 10, isDeleted?: boolean) => {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });
    if (isDeleted !== undefined) {
      params.append('isDeleted', isDeleted.toString());
    }
    const response = await api.get<ApiResponse<PagedResult<Grade>>>(`/grades/getall?${params}`);
    console.log('Raw API Response:', response.data); // Debug için
    return response.data;
  },
  
  create: async (courseName: string, description: string, file?: File) => {
    const formData = new FormData();
    formData.append('CourseName', courseName);
    formData.append('Description', description);
    if (file) {
      formData.append('File', file);
    }
    
    const response = await api.post<ApiResponse<string>>('/grades/', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  },
  
  update: async (id: string, courseName: string, description: string, file?: File) => {
    const formData = new FormData();
    formData.append('Id', id);
    formData.append('CourseName', courseName);
    formData.append('Description', description);
    if (file) {
      formData.append('File', file);
    }
    
    const response = await api.put<ApiResponse<string>>('/grades/', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  },
  
  delete: async (id: string) => {
    const response = await api.delete<ApiResponse<string>>(`/grades/delete/${id}`);
    return response.data;
  },
};

export default api;