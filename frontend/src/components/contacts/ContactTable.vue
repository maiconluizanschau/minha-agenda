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
            :active="slotProps.data.favorito"
            @toggle="$emit('toggleFavorite', slotProps.data)"
          />
        </div>
      </template>
    </Column>

    <Column field="nome" header="Nome" sortable />
    <Column field="email" header="E-mail" sortable />
    <Column field="telefone" header="Telefone" />

    <Column header="Ações" style="width: 160px">
      <template #body="slotProps">
        <div class="flex gap-2 justify-end">
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
  }
});

defineEmits(['edit', 'delete', 'toggleFavorite']);
</script>
