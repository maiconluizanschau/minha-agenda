<template>
  <div class="app-shell">
    <Navbar :username="username" :role="role" @logout="logout" />

    <main class="main-content px-3 py-5">
      <Toast />
      <ConfirmDialog />

      <header class="page-header">
        <h1 class="page-title">Agenda de Contatos</h1>
        <p class="page-subtitle">
          Gerencie sua base de contatos com favoritos, filtros e um resumo rápido no topo.
        </p>
      </header>

      <section class="mb-5 kpi-grid">
        <DashboardCard
          title="Total de contatos"
          :value="contacts.length"
          subtitle="Inclui ativos e favoritos"
        >
          <template #icon>
            <i class="pi pi-users"></i>
          </template>
        </DashboardCard>

        <DashboardCard
          title="Favoritos"
          :value="favoritesCount"
          subtitle="Contatos marcados com estrela"
        >
          <template #icon>
            <i class="pi pi-star-fill"></i>
          </template>
        </DashboardCard>

        <DashboardCard
          title="Último criado"
          :value="lastContactName"
          :subtitle="lastContactSubtitle"
        >
          <template #icon>
            <i class="pi pi-clock"></i>
          </template>
        </DashboardCard>
      </section>

      <section class="glass-card p-4">
        <div class="flex justify-between items-center mb-4 gap-4">
          <div>
            <h2 class="text-base font-semibold">Contatos</h2>
            <p class="section-hint">
              Clique na estrela para favoritar. Favoritos aparecem primeiro na lista.
              <span v-if="showOnlyFavorites">
                · Filtro de favoritos ativo.
              </span>
            </p>
          </div>
          <div class="flex gap-3 items-center flex-wrap justify-end">
            <div class="flex items-center gap-2 text-xs text-muted">
              <i class="pi" :class="showOnlyFavorites ? 'pi-star-fill' : 'pi-star'" />
              <span>Mostrar só favoritos</span>
              <Button
                :label="showOnlyFavorites ? 'Ativo' : 'Inativo'"
                size="small"
                text
                :severity="showOnlyFavorites ? 'warning' : 'secondary'"
                @click="toggleOnlyFavorites"
              />
            </div>
            <span class="hidden sm:inline text-xs text-muted mr-2">
              {{ filteredContacts.length }} contato(s) exibido(s)
            </span>
            <Button
              v-if="isAdmin"
              label="Novo contato"
              icon="pi pi-plus"
              @click="openNew"
            />
          </div>
        </div>

        <div v-if="loading" class="space-y-2">
          <div v-for="i in 5" :key="i" class="skeleton-row"></div>
        </div>

        <div v-else>
          <ContactTable
            :contacts="filteredContacts"
            :loading="loading"
            :can-manage="isAdmin"
            @edit="onEdit"
            @delete="onDelete"
            @toggleFavorite="onToggleFavorite"
          />
        </div>
      </section>

      <ContactForm
        v-model="showForm"
        :contact="selectedContact"
        @save="onSave"
      />
    </main>
  </div>
</template>

<script setup>
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';
import { contactService } from '../services/api';
import ContactTable from '../components/contacts/ContactTable.vue';
import ContactForm from '../components/contacts/ContactForm.vue';
import Navbar from '../components/ui/Navbar.vue';
import DashboardCard from '../components/ui/DashboardCard.vue';

const router = useRouter();
const toast = useToast();
const confirm = useConfirm();

const contacts = ref([]);
const loading = ref(false);
const showForm = ref(false);
const selectedContact = ref(null);
const username = ref(localStorage.getItem('username') || 'usuário');
const role = ref(localStorage.getItem('role') || 'Admin');
const showOnlyFavorites = ref(false);

const isAdmin = computed(() => role.value === 'Admin');
const favoritesCount = computed(() => contacts.value.filter(c => c.favorito).length);

const lastContact = computed(() => {
  if (!contacts.value.length) return null;
  return contacts.value[contacts.value.length - 1];
});

const lastContactName = computed(() => lastContact.value?.nome || '-');
const lastContactSubtitle = computed(() =>
  lastContact.value ? lastContact.value.email : 'Nenhum contato cadastrado'
);

const filteredContacts = computed(() => {
  if (!showOnlyFavorites.value) return contacts.value;
  return contacts.value.filter(c => c.favorito);
});

function ensureAdmin() {
  if (isAdmin.value) return true;

  toast.add({
    severity: 'warn',
    summary: 'Acesso restrito',
    detail:
      'Este usuário não possui permissão para gerenciar contatos. Use a conta admin para CRUD completo.',
    life: 4000
  });
  return false;
}

async function loadContacts() {
  if (!isAdmin.value) {
    // Usuário demo "user" não carrega contatos, apenas vê tela vazia e aviso
    return;
  }

  loading.value = true;
  try {
    contacts.value = await contactService.getAll();
  } catch (err) {
    toast.add({
      severity: 'error',
      summary: 'Erro',
      detail: 'Falha ao carregar contatos',
      life: 3000
    });
  } finally {
    loading.value = false;
  }
}

function openNew() {
  if (!ensureAdmin()) return;
  selectedContact.value = null;
  showForm.value = true;
}

function onEdit(contact) {
  if (!ensureAdmin()) return;
  selectedContact.value = { ...contact };
  showForm.value = true;
}

function onDelete(contact) {
  if (!ensureAdmin()) return;

  confirm.require({
    message: `Deseja realmente excluir o contato "${contact.nome}"?`,
    header: 'Confirmação',
    icon: 'pi pi-exclamation-triangle',
    accept: async () => {
      try {
        await contactService.remove(contact.id);
        toast.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: 'Contato excluído',
          life: 3000
        });
        await loadContacts();
      } catch {
        toast.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Falha ao excluir contato',
          life: 3000
        });
      }
    }
  });
}

async function onSave(payload) {
  if (!ensureAdmin()) return;

  try {
    if (payload.id) {
      await contactService.update(payload.id, {
        nome: payload.nome,
        telefone: payload.telefone
      });
      toast.add({
        severity: 'success',
        summary: 'Sucesso',
        detail: 'Contato atualizado',
        life: 3000
      });
    } else {
      await contactService.create({
        nome: payload.nome,
        email: payload.email,
        telefone: payload.telefone
      });
      toast.add({
        severity: 'success',
        summary: 'Sucesso',
        detail: 'Contato criado',
        life: 3000
      });
    }

    showForm.value = false;
    await loadContacts();
  } catch (err) {
    toast.add({
      severity: 'error',
      summary: 'Erro',
      detail: 'Falha ao salvar contato',
      life: 3000
    });
  }
}

async function onToggleFavorite(contact) {
  if (!ensureAdmin()) return;

  try {
    const updated = await contactService.toggleFavorite(contact.id);
    toast.add({
      severity: updated.favorito ? 'info' : 'secondary',
      summary: updated.favorito ? 'Favorito' : 'Removido dos favoritos',
      detail: updated.favorito
        ? `Contato "${updated.nome}" marcado como favorito.`
        : `Contato "${updated.nome}" removido dos favoritos.`,
      life: 2500
    });
    await loadContacts();
  } catch {
    toast.add({
      severity: 'error',
      summary: 'Erro',
      detail: 'Falha ao atualizar favorito',
      life: 3000
    });
  }
}

function toggleOnlyFavorites() {
  showOnlyFavorites.value = !showOnlyFavorites.value;
}

function logout() {
  localStorage.removeItem('token');
  localStorage.removeItem('username');
  localStorage.removeItem('role');
  router.push({ name: 'Login' });
}

onMounted(async () => {
  if (!isAdmin.value) {
    toast.add({
      severity: 'info',
      summary: 'Conta de demonstração',
      detail:
        'Usuário "user" é apenas para demonstrar acesso negado ao CRUD. Use "admin" para gerenciar contatos.',
      life: 5000
    });
    return;
  }

  await loadContacts();
});
</script>
