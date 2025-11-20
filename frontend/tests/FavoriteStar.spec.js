import { mount } from '@vue/test-utils';
import FavoriteStar from '../src/components/contacts/FavoriteStar.vue';

describe('FavoriteStar', () => {
  it('renders filled star when active', () => {
    const wrapper = mount(FavoriteStar, {
      props: { active: true }
    });

    expect(wrapper.classes()).toContain('favorite-icon');
    expect(wrapper.classes()).toContain('pi-star-fill');
  });

  it('renders outline star when not active', () => {
    const wrapper = mount(FavoriteStar, {
      props: { active: false }
    });

    expect(wrapper.classes()).toContain('favorite-icon');
    expect(wrapper.classes()).toContain('pi-star');
  });

  it('emits toggle event when clicked', async () => {
    const wrapper = mount(FavoriteStar, {
      props: { active: false }
    });

    await wrapper.trigger('click');
    expect(wrapper.emitted().toggle).toBeTruthy();
    expect(wrapper.emitted().toggle.length).toBe(1);
  });
});
