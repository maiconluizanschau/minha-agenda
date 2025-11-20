import { mount } from '@vue/test-utils';
import ContactForm from '../src/components/contacts/ContactForm.vue';

describe('ContactForm', () => {
  it('shows validation errors when fields are empty', async () => {
    const wrapper = mount(ContactForm, {
      props: {
        modelValue: true,
        contact: null
      }
    });

    await wrapper.find('button.p-button-label').trigger('click');
    // Botão "Salvar" é o segundo, então vamos disparar o click no último botão
    const buttons = wrapper.findAll('button');
    const saveButton = buttons[buttons.length - 1];
    await saveButton.trigger('click');

    expect(wrapper.html()).toContain('Nome deve ter ao menos 3 caracteres.');
  });
});
