import { mount } from '@vue/test-utils';
import ContactTable from '../src/components/contacts/ContactTable.vue';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';

describe('ContactTable', () => {
  const contacts = [
    {
      id: '1',
      nome: 'Contato 1',
      email: 'c1@teste.com',
      telefone: '11999999999',
      favorito: false
    }
  ];

  it('emite toggleFavorite quando estrela é clicada', async () => {
    const wrapper = mount(ContactTable, {
      props: { contacts, loading: false },
      global: {
        components: { DataTable, Column, Button }
      }
    });

    const star = wrapper.find('.favorite-icon');
    expect(star.exists()).toBe(true);

    await star.trigger('click');

    const emitted = wrapper.emitted('toggleFavorite');
    expect(emitted).toBeTruthy();
    expect(emitted[0][0]).toMatchObject({ id: '1' });
  });
});
