<template>
  <div class="glass-card dashboard-card">
    <div class="dashboard-card-header">
      <div>
        <div class="dashboard-title">
          {{ title }}
        </div>
      </div>
      <div>
        <slot name="icon" />
      </div>
    </div>
    <div class="dashboard-value">
      {{ value }}
    </div>
    <div class="dashboard-subtitle" v-if="subtitle">
      {{ subtitle }}
    </div>
  </div>
</template>

<script setup>
defineProps({
  title: String,
  value: [String, Number],
  subtitle: {
    type: String,
    default: ''
  }
});
</script>
