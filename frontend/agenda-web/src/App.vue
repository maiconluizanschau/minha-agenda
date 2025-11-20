<template>
  <div class="layout-root">
    <header class="layout-header surface-0 shadow-2">
      <div class="layout-header-inner">
        <div class="flex align-items-center gap-2">
          <i class="pi pi-calendar layout-logo-icon"></i>
          <div>
            <h1 class="layout-title">Agenda</h1>
            <span class="layout-subtitle">Sua agenda de contatos, do jeito certo.</span>
          </div>
        </div>

        <div class="flex align-items-center gap-3">
          <Button
            :icon="isDark ? 'pi pi-moon' : 'pi pi-sun'"
            class="p-button-text p-button-rounded p-button-sm theme-toggle"
            @click="toggleTheme"
          />

          <span v-if="isAuthenticated" class="layout-user-chip">
            <span class="avatar-circle">
              <span>A</span>
            </span>
            <div class="flex flex-column">
              <span class="user-name">admin</span>
              <span class="user-role">Usuário de teste</span>
            </div>
          </span>

          <Button
            v-if="isAuthenticated"
            label="Sair"
            icon="pi pi-sign-out"
            size="small"
            class="p-button-text p-button-plain"
            @click="logout"
          />
        </div>
      </div>
    </header>

    <main class="layout-main">
      <section class="layout-content surface-card shadow-2 border-round-xl animate-fade-up">
        <RouterView />
      </section>
    </main>

    <Toast />
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch, onMounted } from 'vue';
import { RouterView, useRouter } from 'vue-router';
import Button from 'primevue/button';
import Toast from 'primevue/toast';
import { useAuthStore } from './store/useAuthStore';

const auth = useAuthStore();
const router = useRouter();

const isAuthenticated = computed(() => auth.isAuthenticated);
const isDark = ref(false);

onMounted(() => {
  const saved = localStorage.getItem('agenda_theme_dark');
  if (saved === 'true') {
    isDark.value = true;
  }
});

watch(
  isDark,
  (value) => {
    const root = document.documentElement;
    if (value) {
      root.classList.add('dark');
    } else {
      root.classList.remove('dark');
    }
    localStorage.setItem('agenda_theme_dark', value ? 'true' : 'false');
  },
  { immediate: true }
);

function toggleTheme() {
  isDark.value = !isDark.value;
}

function logout() {
  auth.logout();
  router.push({ name: 'login' });
}
</script>

<style scoped>
.layout-root {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background: radial-gradient(circle at top left, #eff6ff 0, #f9fafb 40%, #eef2ff 100%);
}

:global(html.dark) .layout-root {
  background: radial-gradient(circle at top left, #020617 0, #020617 40%, #020617 100%);
}

.layout-header {
  position: sticky;
  top: 0;
  z-index: 10;
}

.layout-header-inner {
  max-width: 1120px;
  margin: 0 auto;
  padding: 0.75rem 1.5rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.layout-logo-icon {
  font-size: 2rem;
  color: var(--p-primary-color);
}

.layout-title {
  font-size: 1.4rem;
  margin: 0;
}

.layout-subtitle {
  font-size: 0.85rem;
  color: var(--p-text-muted-color);
}

.layout-main {
  flex: 1;
  display: flex;
  justify-content: center;
  padding: 1.5rem 1rem 2rem;
}

.layout-content {
  width: 100%;
  max-width: 1120px;
  padding: 1.75rem;
}

.layout-user-chip {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.35rem 0.75rem;
  border-radius: 999px;
  background: rgba(15, 23, 42, 0.03);
  border: 1px solid rgba(148, 163, 184, 0.3);
}

:global(html.dark) .layout-user-chip {
  background: rgba(15, 23, 42, 0.7);
  border-color: rgba(148, 163, 184, 0.5);
}

.avatar-circle {
  width: 32px;
  height: 32px;
  border-radius: 999px;
  background: var(--p-primary-color);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.9rem;
  color: #fff;
  font-weight: 600;
}

.user-name {
  font-size: 0.85rem;
  font-weight: 600;
}

.user-role {
  font-size: 0.7rem;
  color: var(--p-text-muted-color);
}

.theme-toggle {
  border-radius: 999px;
}

/* Animation */
@keyframes fadeUp {
  from {
    opacity: 0;
    transform: translateY(12px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-fade-up {
  animation: fadeUp 0.3s ease-out;
}

@media (max-width: 768px) {
  .layout-header-inner {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.75rem;
  }

  .layout-content {
    padding: 1.25rem 1rem;
  }
}
</style>
