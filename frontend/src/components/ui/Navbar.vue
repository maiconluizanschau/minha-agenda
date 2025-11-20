<template>
  <header class="navbar w-full">
    <div class="main-content flex items-center justify-between py-3">
      <div class="flex items-center gap-3">
        <div class="w-9 h-9 rounded-md border border-gray-200 flex items-center justify-center" style="background: #eff6ff;">
          <i class="pi pi-calendar" style="font-size: 0.9rem; color: #2563eb;"></i>
        </div>
        <div>
          <div class="navbar-title">Agenda Pro</div>
          <div class="nav-subtitle">
            CQRS · JWT · RabbitMQ · Vue 3
          </div>
        </div>
      </div>

      <div class="flex items-center gap-4">
        <span class="badge-pill flex items-center gap-1">
          <i class="pi pi-shield"></i>
          Admin
        </span>
        <div class="flex items-center gap-2">
          <div class="nav-avatar">
            {{ initials }}
          </div>
          <button
            class="text-muted text-xs border-0 bg-transparent cursor-pointer hover:underline"
            type="button"
            @click="$emit('logout')"
          >
            Sair
          </button>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  username: {
    type: String,
    default: 'usuário'
  }
});

defineEmits(['logout']);

const initials = computed(() => {
  if (!props.username) return '?';
  return props.username.charAt(0).toUpperCase();
});
</script>
