<template>
  <div class="login-background">
    <Toast />
    <div class="glass-login p-6 w-full max-w-sm animate__animated animate__fadeInUp">
      <div class="flex items-center gap-3 mb-4">
        <div class="w-9 h-9 rounded-md border border-gray-200 flex items-center justify-center" style="background:#eff6ff;">
          <i class="pi pi-lock" style="color:#2563eb; font-size:1rem;"></i>
        </div>
        <div>
          <h2 class="m-0 text-lg font-semibold" style="color:#111827;">Bem-vindo à Agenda Pro</h2>
          <p class="m-0 text-xs text-muted">
            Autentique-se para gerenciar seus contatos.
          </p>
        </div>
      </div>

      <div class="p-fluid">
        <div class="field mb-3">
          <label for="username" class="text-xs text-muted mb-1 block">Usuário</label>
          <InputText
            id="username"
            v-model="username"
            placeholder="admin ou user"
          />
        </div>

        <div class="field mb-4">
          <label for="password" class="text-xs text-muted mb-1 block">Senha</label>
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
          class="w-full"
          @click="login"
          :loading="loading"
        />

        <p class="mt-4 mb-0 text-xs text-muted">
          Usuários de demo:
          <br />
          <strong>admin / P@ssw0rd</strong> (Admin)
          <br />
          <strong>user / P@ssw0rd</strong> (User - acesso negado ao CRUD)
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
    localStorage.setItem('token', token);
    localStorage.setItem('username', username.value);

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
