<template>
  <DataTable
    :value="contacts"
    dataKey="id"
    paginator
    :rows="10"
    :loading="loading"
    responsiveLayout="scroll"
  >
    <Column header="" style="width: 60px; text-align: center">
      <template #body="slotProps">
        <div class="flex justify-center">
          <FavoriteStar
            v-if="canManage"
            :active="slotProps.data.favorito"
            @toggle="$emit('toggleFavorite', slotProps.data)"
          />
          <i
            v-else
            class="pi pi-star-fill"
            v-show="slotProps.data.favorito"
            style="font-size: 0.9rem; color: #f59e0b;"
          />
        </div>
      </template>
    </Column>

    <Column field="nome" header="Nome" sortable />
    <Column field="email" header="E-mail" sortable />
    <Column field="telefone" header="Telefone" />

    <Column header="Ações" style="width: 160px">
      <template #body="slotProps">
        <div class="flex gap-2 justify-end" v-if="canManage">
          <Button
            icon="pi pi-pencil"
            text
            @click="$emit('edit', slotProps.data)"
          />
          <Button
            icon="pi pi-trash"
            severity="danger"
            text
            @click="$emit('delete', slotProps.data)"
          />
        </div>
        <div v-else class="text-xs text-muted text-right pr-2">
          Somente leitura
        </div>
      </template>
    </Column>
  </DataTable>
</template>

<script setup>
import FavoriteStar from './FavoriteStar.vue';

defineProps({
  contacts: {
    type: Array,
    default: () => []
  },
  loading: {
    type: Boolean,
    default: false
  },
  canManage: {
    type: Boolean,
    default: true
  }
});

defineEmits(['edit', 'delete', 'toggleFavorite']);
</script>
