import '@testing-library/jest-dom';
import { config } from '@vue/test-utils';

import PrimeVue from 'primevue/config';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';

// plugin PrimeVue (cria $primevue com config/aria)
config.global.plugins = [
  ...(config.global.plugins || []),
  [PrimeVue, {
    ripple: false,
    locale: {
      aria: {}, // evita erro this.$primevue.config.aria
    },
  }],
];

// registra os componentes usados nos testes
config.global.components = {
  ...(config.global.components || {}),
  DataTable,
  Column,
  Button,
  Dialog,
  InputText,
};
