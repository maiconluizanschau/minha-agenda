<template>
  <div class="login-background">
    <Toast />
    <div class="glass-login">
      <div class="login-header">
        <div class="login-icon">
          <i class="pi pi-lock"></i>
        </div>
        <div>
          <h2 class="login-title">Bem-vindo à Agenda</h2>
          <p class="login-subtitle">
            Autentique-se para gerenciar seus contatos.
          </p>
        </div>
      </div>

      <div class="p-fluid">
        <div class="field mb-3">
          <label for="username" class="login-label">Usuário</label>
          <InputText
            id="username"
            v-model="username"
            placeholder="admin ou user"
          />
        </div>

        <div class="field mb-4">
          <label for="password" class="login-label">Senha</label>
          <Password
            id="password"
            v-model="password"
            :feedback="false"
            toggleMask
            inputClass="w-full"
          />
        </div>

        <Button
          label="Entrar"
          class="login-button"
          @click="login"
          :loading="loading"
        />

        <p class="login-demo">
          Usuários de demo:
          <br />
          <strong>admin / P@ssw0rd</strong> (Admin)
          <br />
          <strong>user / P@ssw0rd</strong> (User – Apenas generico)
        </p>
      </div>
    </div>
  </div>
</template>


<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'primevue/usetoast';
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Button from 'primevue/button';
import axios from 'axios';

const router = useRouter();
const toast = useToast();

const username = ref('admin');
const password = ref('P@ssw0rd');
const loading = ref(false);

const baseUrl = (import.meta.env.VITE_API_BASE_URL && import.meta.env.VITE_API_BASE_URL.trim() !== '')
  ? import.meta.env.VITE_API_BASE_URL
  : 'http://localhost:5000/api';

async function login() {
  try {
    loading.value = true;
    const response = await axios.post(`${baseUrl}/auth/login`, {
      username: username.value,
      password: password.value
    });

    const token = response.data.token;
    const role = response.data.role;
    localStorage.setItem('token', token);
    localStorage.setItem('username', username.value);
    if (role) {
      localStorage.setItem('role', role);
    }

    router.push({ name: 'Contacts' });
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: 'Erro',
      detail: 'Login inválido',
      life: 3000
    });
  } finally {
    loading.value = false;
  }
}
</script>
