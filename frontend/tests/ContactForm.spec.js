import { mount } from '@vue/test-utils';
import ContactForm from '../src/components/contacts/ContactForm.vue';

describe('ContactForm', () => {
  it('does not emit save when fields are empty', async () => {
    const wrapper = mount(ContactForm, {
      props: {
        modelValue: true,
        contact: null
      },
      global: {
        stubs: {
          // Dialog simples que só renderiza o conteúdo e o footer
          Dialog: {
            template: `
              <div data-test="dialog">
                <slot />
                <slot name="footer" />
              </div>
            `,
            props: ['visible', 'header', 'modal', 'style', 'closable']
          },
          // InputText simples, não precisamos da implementação real
          InputText: {
            template: '<input />',
            props: ['modelValue']
          },
          // Button stub com data-test para o teste poder clicar
          Button: {
            template: '<button data-test="save-contact"><slot /></button>',
            props: ['label', 'text']
          }
        }
      }
    });

    // Clica em "Salvar" com todos os campos vazios
    await wrapper.get('[data-test="save-contact"]').trigger('click');

    // Se a validação funcionar, NÃO deve emitir o evento 'save'
    expect(wrapper.emitted('save')).toBeUndefined();
  });
});
