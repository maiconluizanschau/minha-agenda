<template>
  <div class="login-wrapper">
    <div class="login-card surface-50 border-round-xl shadow-2">
      <div class="login-header">
        <h2>Bem-vindo à Agenda</h2>
        <p>Faça login para acessar seus contatos.</p>
      </div>

      <form class="login-form" @submit.prevent="onSubmit">
        <div class="field">
          <label for="username">Usuário</label>
          <InputText
            id="username"
            v-model="username"
            class="w-full"
            autocomplete="off"
          />
        </div>

        <div class="field">
          <label for="password">Senha</label>
          <InputText
            id="password"
            v-model="password"
            type="password"
            class="w-full"
            autocomplete="off"
          />
        </div>

        <Button
          type="submit"
          label="Entrar"
          icon="pi pi-sign-in"
          class="w-full"
          :loading="loading"
        />

        <p class="login-hint">
          <strong>Usuário:</strong> admin &nbsp;&nbsp;
          <strong>Senha:</strong> admin123
        </p>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'primevue/usetoast';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import { useAuthStore } from '../store/useAuthStore';

const router = useRouter();
const toast = useToast();
const auth = useAuthStore();

const username = ref('admin');
const password = ref('admin123');
const loading = ref(false);

async function onSubmit() {
  loading.value = true;

  // 1) Primeiro tentamos logar
  try {
    await auth.login(username.value, password.value);
  } catch (err: any) {
    // Se der erro no login, só mostra o erro e sai
    toast.add({
      severity: 'error',
      summary: 'Falha no login',
      detail: 'Usuário ou senha inválidos.',
      life: 3000
    });
    loading.value = false;
    return;
  }

  // 2) Se chegou aqui, login foi OK
  toast.add({
    severity: 'success',
    summary: 'Login realizado',
    life: 2000
  });

  // 3) Redireciona e ignora qualquer errozinho de navegação
  router.push({ name: 'contatos' }).catch(() => {});

  loading.value = false;
}
</script>

<style scoped>
.login-wrapper {
  min-height: calc(100vh - 4rem);
  display: flex;
  align-items: center;
  justify-content: center;
}

.login-card {
  width: 100%;
  max-width: 420px;
  padding: 2rem;
}

.login-header h2 {
  margin: 0;
  font-size: 1.4rem;
}

.login-header p {
  margin: 0.4rem 0 0;
  font-size: 0.9rem;
  color: var(--p-text-muted-color);
}

.login-form {
  margin-top: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.field label {
  font-size: 0.8rem;
  font-weight: 500;
}

.login-hint {
  margin: 0.75rem 0 0;
  font-size: 0.75rem;
  color: var(--p-text-muted-color);
  text-align: center;
}
</style>
