<template>
  <Button
    :icon="isDark ? 'pi pi-moon' : 'pi pi-sun'"
    rounded
    text
    size="small"
    @click="toggle"
    :aria-label="isDark ? 'Ativar modo claro' : 'Ativar modo escuro'"
  />
</template>

<script setup>
import { computed, onMounted, ref } from 'vue';

const isDark = ref(false);

onMounted(() => {
  const stored = localStorage.getItem('theme-dark');
  isDark.value = stored === 'true';
  applyTheme();
});

const toggle = () => {
  isDark.value = !isDark.value;
  localStorage.setItem('theme-dark', String(isDark.value));
  applyTheme();
};

const applyTheme = () => {
  if (isDark.value) {
    document.documentElement.classList.add('dark-theme');
  } else {
    document.documentElement.classList.remove('dark-theme');
  }
};

</script>

<style>
.dark-theme body {
  background: radial-gradient(circle at top, #020617 0, #020617 40%, #000000 100%);
}
</style>
