import { defineStore } from 'pinia';
import api from '../services/api';

interface AuthState {
  token: string | null;
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    token: localStorage.getItem('agenda_token')
  }),
  getters: {
    isAuthenticated: (state) => !!state.token
  },
  actions: {
    async login(username: string, password: string) {
      const response = await api.post('/auth/login', {
        username,
        password
      });

      const token = response.data?.token ?? response.data;
      this.token = token;
      localStorage.setItem('agenda_token', token);
    },
    logout() {
      this.token = null;
      localStorage.removeItem('agenda_token');
    }
  }
});
