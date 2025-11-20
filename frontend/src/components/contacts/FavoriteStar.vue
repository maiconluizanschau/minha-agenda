<template>
  <i
    class="pi favorite-icon"
    :class="iconClass"
    :style="iconStyle"
    @click="$emit('toggle')"
  />
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  active: {
    type: Boolean,
    default: false
  }
});

const iconClass = computed(() => (props.active ? 'pi-star-fill' : 'pi-star'));

const iconStyle = computed(() => ({
  color: props.active ? '#facc15' : '#64748b',
  fontSize: '1.1rem'
}));
</script>
