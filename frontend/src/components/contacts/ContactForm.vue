<template>
  <Dialog
    v-model:visible="visibleInternal"
    :header="isEdit ? 'Editar Contato' : 'Novo Contato'"
    :modal="true"
    :style="{ width: '450px' }"
    :closable="true"
  >
    <div class="p-fluid">
      <div class="field">
        <label for="nome">Nome</label>
        <InputText
          id="nome"
          v-model="form.nome"
          :class="{ 'p-invalid': errors.nome }"
        />
        <small v-if="errors.nome" class="p-error">{{ errors.nome }}</small>
      </div>

      <div class="field" v-if="!isEdit">
        <label for="email">E-mail</label>
        <InputText
          id="email"
          v-model="form.email"
          :class="{ 'p-invalid': errors.email }"
        />
        <small v-if="errors.email" class="p-error">{{ errors.email }}</small>
      </div>

      <div class="field">
        <label for="telefone">Telefone</label>
        <InputText
          id="telefone"
          v-model="form.telefone"
          :class="{ 'p-invalid': errors.telefone }"
        />
        <small v-if="errors.telefone" class="p-error">{{ errors.telefone }}</small>
      </div>
    </div>

    <template #footer>
      <Button label="Cancelar" text @click="onCancel" />
      <Button label="Salvar" @click="onSave" />
    </template>
  </Dialog>
</template>

<script setup>
import { computed, reactive, watch } from 'vue';

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  contact: {
    type: Object,
    default: () => ({})
  }
});

const emit = defineEmits(['update:modelValue', 'save']);

const visibleInternal = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val)
});

const isEdit = computed(() => !!props.contact?.id);

const form = reactive({
  id: null,
  nome: '',
  email: '',
  telefone: ''
});

const errors = reactive({
  nome: '',
  email: '',
  telefone: ''
});

watch(
  () => props.contact,
  (val) => {
    form.id = val?.id || null;
    form.nome = val?.nome || '';
    form.email = val?.email || '';
    form.telefone = val?.telefone || '';
    clearErrors();
  },
  { immediate: true }
);

function clearErrors() {
  errors.nome = '';
  errors.email = '';
  errors.telefone = '';
}

function validate() {
  clearErrors();
  let valid = true;

  if (!form.nome || form.nome.length < 3) {
    errors.nome = 'Nome deve ter ao menos 3 caracteres.';
    valid = false;
  }

  if (!isEdit.value) {
    if (!form.email) {
      errors.email = 'E-mail é obrigatório.';
      valid = false;
    } else if (!/^\S+@\S+\.\S+$/.test(form.email)) {
      errors.email = 'E-mail inválido.';
      valid = false;
    }
  }

  if (!form.telefone || !/^\d{10,11}$/.test(form.telefone)) {
    errors.telefone = 'Telefone deve conter 10 ou 11 dígitos.';
    valid = false;
  }

  return valid;
}

function onSave() {
  if (!validate()) return;
  emit('save', { ...form });
}

function onCancel() {
  visibleInternal.value = false;
}
</script>
