import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api'
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const authService = {
  async login(username, password) {
    const { data } = await api.post('/auth/login', { username, password });
    return data;
  }
};

export const contactService = {
  async getAll() {
    const { data } = await api.get('/contacts');
    return data;
  },
  async create(payload) {
    const { data } = await api.post('/contacts', payload);
    return data;
  },
  async update(id, payload) {
    const { data } = await api.put(`/contacts/${id}`, payload);
    return data;
  },
  async remove(id) {
    await api.delete(`/contacts/${id}`);
  },
  async toggleFavorite(id) {
    const { data } = await api.patch(`/contacts/${id}/favorite`);
    return data;
  }
};

export default api;
