import axios from 'axios';

const baseURL = import.meta.env.VITE_PORTAL_API_URL ?? 'https://localhost:50865';

export const http = axios.create({
  baseURL
});

http.interceptors.request.use((config) => {
  const token = localStorage.getItem('apiforge_token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
