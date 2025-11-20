import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import ToastService from 'primevue/toastservice';
import Toast from 'primevue/toast';
import ConfirmDialog from 'primevue/confirmdialog';
import ConfirmationService from 'primevue/confirmationservice';

import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';
import './assets/main.css';

const app = createApp(App);

app.use(router);
app.use(PrimeVue);
app.use(ToastService);
app.use(ConfirmationService);

app.component('Button', Button);
app.component('DataTable', DataTable);
app.component('Column', Column);
app.component('Dialog', Dialog);
app.component('InputText', InputText);
app.component('Toast', Toast);
app.component('ConfirmDialog', ConfirmDialog);

app.mount('#app');
