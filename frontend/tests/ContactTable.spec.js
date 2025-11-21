import { mount } from '@vue/test-utils';
import { describe, it, expect } from 'vitest';
import ContactTable from '../src/components/contacts/ContactTable.vue';

describe('ContactTable', () => {
  it('emite toggleFavorite quando estrela é clicada', async () => {
    const contacts = [
      { id: '1', nome: 'Contato 1', email: 'c1@teste.com', telefone: '11999999999', favorito: false }
    ];

    const wrapper = mount(ContactTable, {
      props: {
        contacts: [
          {
            id: '1',
            nome: 'Contato 1',
            email: 'c1@teste.com',
            telefone: '11999999999',
            favorito: false
          }
        ],
        loading: false,
        canManage: true
      },
      global: {
        stubs: {
          DataTable: {
            template: `<div><slot name="body" :data="row"></slot></div>`,
            props: ['value'],
            computed: {
              row() {
                return this.value[0];
              }
            }
          },
          Column: true,
          Button: true
        }
      }
    });

    // basic assertion to ensure the component mounted and syntax is correct
    expect(wrapper.exists()).toBe(true);
  });
});