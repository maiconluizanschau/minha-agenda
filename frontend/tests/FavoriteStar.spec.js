import { mount } from '@vue/test-utils';
import { describe, it, expect } from 'vitest';
import FavoriteStar from '../src/components/contacts/FavoriteStar.vue';

describe('FavoriteStar', () => {
  it('emits toggle when clicked', async () => {
    const wrapper = mount(FavoriteStar, { props: { isFavorite: false } });
    const btn = wrapper.get('[data-test="favorite-button"]');
    await btn.trigger('click');
    expect(wrapper.emitted().toggle).toBeTruthy();
  });
});
