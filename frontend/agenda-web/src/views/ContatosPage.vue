<template>
  <div class="contatos-page">
    <div class="page-header">
      <div>
        <h2 class="page-title">Contatos</h2>
        <p class="page-description">
          Gerencie seus contatos pessoais e profissionais com rapidez e organização.
        </p>
      </div>
      <div class="flex gap-2">
        <Button
          label="Atualizar"
          icon="pi pi-refresh"
          class="p-button-text p-button-sm"
          @click="carregar"
        />
        <Button
          label="Novo contato"
          icon="pi pi-plus"
          class="p-button-sm"
          @click="novoContato"
        />
      </div>
    </div>

    <div class="stats-grid">
      <div class="stat-card surface-50 border-round-lg">
        <div class="stat-header">
          <span>Total de contatos</span>
          <i class="pi pi-users stat-icon"></i>
        </div>
        <div class="stat-value">{{ contatos.length }}</div>
      </div>

      <div class="stat-card surface-50 border-round-lg">
        <div class="stat-header">
          <span>Favoritos</span>
          <i class="pi pi-star-fill stat-icon favorite"></i>
        </div>
        <div class="stat-value">
          {{ contatos.filter(c => c.favorito).length }}
        </div>
      </div>
    </div>

    <div class="toolbar">
      <span class="p-input-icon-left w-full md:w-30rem">
        <i class="pi pi-search" />
        <InputText
          v-model="filtro"
          placeholder="Buscar por nome, email ou telefone..."
          class="w-full"
        />
      </span>

      <div class="flex align-items-center gap-2">
        <Checkbox v-model="mostrarFavoritos" :binary="true" input-id="chk-fav" />
        <label for="chk-fav">Mostrar apenas favoritos</label>
      </div>
    </div>

    <div
      v-if="contatosFiltrados.length === 0 && !filtro && !carregando"
      class="empty-state surface-50 border-round-lg"
    >
      <div class="empty-icon">
        <i class="pi pi-address-book"></i>
      </div>
      <h3>Nenhum contato ainda</h3>
      <p>Comece criando o seu primeiro contato. É rápido e simples.</p>
      <Button
        label="Criar primeiro contato"
        icon="pi pi-plus"
        @click="novoContato"
      />
    </div>

    <DataTable
      v-else
      :value="contatosFiltrados"
      dataKey="id"
      class="w-full"
      stripedRows
      :paginator="true"
      :rows="8"
      :rowsPerPageOptions="[5, 8, 15, 30]"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
      :loading="carregando"
      responsiveLayout="stack"
      breakpoint="768px"
      emptyMessage="Nenhum contato encontrado."
    >
      <Column field="nome" header="Nome" sortable></Column>
      <Column field="email" header="Email" sortable></Column>
      <Column field="telefone" header="Telefone" sortable></Column>
      <Column header="Favorito" style="width: 8rem; text-align: center;">
        <template #body="slotProps">
          <Button
            class="p-button-text p-button-rounded p-button-sm"
            :icon="slotProps.data.favorito ? 'pi pi-star-fill' : 'pi pi-star'"
            :severity="slotProps.data.favorito ? 'warning' : undefined"
            @click="alternarFavorito(slotProps.data)"
          />
        </template>
      </Column>
      <Column header="Ações" style="width: 10rem">
        <template #body="slotProps">
          <div class="flex gap-2">
            <Button
              icon="pi pi-pencil"
              class="p-button-rounded p-button-text p-button-sm"
              @click="editarContato(slotProps.data)"
            />
            <Button
              icon="pi pi-trash"
              class="p-button-rounded p-button-text p-button-danger p-button-sm"
              @click="excluirContato(slotProps.data)"
            />
          </div>
        </template>
      </Column>
    </DataTable>

    <Dialog
      v-model:visible="showDialog"
      modal
      :header="editingId ? 'Editar contato' : 'Novo contato'"
      :style="{ width: '32rem', maxWidth: '95vw' }"
      :breakpoints="{ '960px': '90vw' }"
    >
      <form class="form-grid" @submit.prevent="salvar">
        <div class="field">
          <label for="nome">Nome</label>
          <InputText
            id="nome"
            v-model="form.nome"
            class="w-full"
            autocomplete="off"
          />
        </div>

        <div class="field">
          <label for="email">Email</label>
          <InputText
            id="email"
            v-model="form.email"
            class="w-full"
            autocomplete="off"
          />
        </div>

        <div class="field">
          <label for="telefone">Telefone</label>
          <InputText
            id="telefone"
            v-model="form.telefone"
            class="w-full"
            autocomplete="off"
          />
        </div>

        <div class="field">
          <label for="obs">Observações</label>
          <InputText
            id="obs"
            v-model="form.observacoes"
            class="w-full"
          />
        </div>

        <div class="field flex align-items-center gap-2">
          <Checkbox v-model="form.favorito" :binary="true" input-id="fav-dialog" />
          <label for="fav-dialog">Marcar como favorito</label>
        </div>

        <div class="dialog-footer">
          <Button
            type="button"
            label="Cancelar"
            class="p-button-text"
            @click="showDialog = false"
          />
          <Button
            type="submit"
            :label="editingId ? 'Salvar alterações' : 'Salvar contato'"
            icon="pi pi-check"
          />
        </div>
      </form>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import api from '../services/api';
import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Checkbox from 'primevue/checkbox';

interface Contato {
  id?: string;
  nome: string;
  email: string;
  telefone: string;
  observacoes?: string;
  favorito?: boolean;
}

const toast = useToast();
const contatos = ref<Contato[]>([]);
const filtro = ref('');
const mostrarFavoritos = ref(false);
const showDialog = ref(false);
const editingId = ref<string | null>(null);
const carregando = ref(false);

const form = reactive<Contato>({
  nome: '',
  email: '',
  telefone: '',
  observacoes: '',
  favorito: false
});

const contatosFiltrados = computed(() => {
  let lista = [...contatos.value];

  if (filtro.value.trim()) {
    const f = filtro.value.trim().toLowerCase();
    lista = lista.filter(c =>
      c.nome.toLowerCase().includes(f) ||
      c.email.toLowerCase().includes(f) ||
      c.telefone.toLowerCase().includes(f)
    );
  }

  if (mostrarFavoritos.value) {
    lista = lista.filter(c => c.favorito);
  }

  return lista;
});

async function carregar() {
  try {
    carregando.value = true;
    const { data } = await api.get<Contato[]>('/contatos');
    contatos.value = data;
  } catch (err: any) {
    toast.add({
      severity: 'error',
      summary: 'Erro ao carregar',
      detail: 'Não foi possível carregar os contatos.',
      life: 3000
    });
  } finally {
    carregando.value = false;
  }
}

function novoContato() {
  editingId.value = null;
  Object.assign(form, {
    nome: '',
    email: '',
    telefone: '',
    observacoes: '',
    favorito: false
  });
  showDialog.value = true;
}

function editarContato(contato: Contato) {
  editingId.value = contato.id ?? null;
  Object.assign(form, contato);
  showDialog.value = true;
}

async function excluirContato(contato: Contato) {
  if (!contato.id) return;
  try {
    await api.delete(`/contatos/${contato.id}`);
    toast.add({
      severity: 'success',
      summary: 'Contato excluído',
      life: 2000
    });
    await carregar();
  } catch (err: any) {
    toast.add({
      severity: 'error',
      summary: 'Erro ao excluir',
      detail: 'Não foi possível excluir o contato.',
      life: 3000
    });
  }
}

async function alternarFavorito(contato: Contato) {
  if (!contato.id) return;
  try {
    const payload = { ...contato, favorito: !contato.favorito };
    await api.put(`/contatos/${contato.id}`, payload);
    await carregar();
  } catch (err: any) {
    toast.add({
      severity: 'error',
      summary: 'Erro ao atualizar favorito',
      life: 3000
    });
  }
}

async function salvar() {
  try {
    if (editingId.value) {
      await api.put(`/contatos/${editingId.value}`, form);
      toast.add({
        severity: 'success',
        summary: 'Contato atualizado',
        life: 2000
      });
    } else {
      await api.post('/contatos', form);
      toast.add({
        severity: 'success',
        summary: 'Contato criado',
        life: 2000
      });
    }
    showDialog.value = false;
    await carregar();
  } catch (err: any) {
    toast.add({
      severity: 'error',
      summary: 'Erro ao salvar',
      detail: err?.response?.data?.message || 'Não foi possível salvar o contato.',
      life: 3000
    });
  }
}

onMounted(() => {
  carregar();
});
</script>

<style scoped>
.contatos-page {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.page-title {
  margin: 0;
  font-size: 1.4rem;
}

.page-description {
  margin: 0.25rem 0 0;
  font-size: 0.9rem;
  color: var(--p-text-muted-color);
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 1rem;
}

.stat-card {
  padding: 1rem;
}

.stat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.85rem;
  color: var(--p-text-muted-color);
}

.stat-icon {
  font-size: 1.2rem;
  color: var(--p-primary-color);
}

.stat-icon.favorite {
  color: #fbbf24;
}

.stat-value {
  margin-top: 0.35rem;
  font-size: 1.4rem;
  font-weight: 600;
}

.toolbar {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  align-items: center;
  justify-content: space-between;
}

.empty-state {
  margin-top: 0.5rem;
  padding: 2rem 1.5rem;
  text-align: center;
  border-style: dashed;
  border-width: 1px;
  border-color: rgba(148, 163, 184, 0.6);
}

.empty-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 56px;
  height: 56px;
  margin: 0 auto 0.75rem;
  border-radius: 999px;
  background: rgba(59, 130, 246, 0.08);
  color: var(--p-primary-color);
  font-size: 1.5rem;
}

.empty-state h3 {
  margin: 0;
}

.empty-state p {
  margin: 0.25rem 0 1rem;
  font-size: 0.9rem;
  color: var(--p-text-muted-color);
}

.form-grid {
  display: flex;
  flex-direction: column;
  gap: 0.9rem;
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

.dialog-footer {
  margin-top: 0.5rem;
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
}

@media (max-width: 768px) {
  .page-header {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
