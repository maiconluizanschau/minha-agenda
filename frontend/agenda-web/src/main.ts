import { createApp } from 'vue';
import { createPinia } from 'pinia';
import PrimeVue from 'primevue/config';
import ToastService from 'primevue/toastservice';

import Aura from '@primevue/themes/aura';
import 'primeicons/primeicons.css';

import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Checkbox from 'primevue/checkbox';
import Toast from 'primevue/toast';

import App from './App.vue';
import router from './router';

const app = createApp(App);

app.use(createPinia());
app.use(router);

app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      cssLayer: true,
      darkModeSelector: 'html.dark'
    }
  }
});

app.use(ToastService);

app.component('Button', Button);
app.component('Dialog', Dialog);
app.component('InputText', InputText);
app.component('DataTable', DataTable);
app.component('Column', Column);
app.component('Checkbox', Checkbox);
app.component('Toast', Toast);

app.mount('#app');
