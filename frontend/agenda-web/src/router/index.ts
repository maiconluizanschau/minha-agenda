import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '../store/useAuthStore';
import LoginPage from '../views/LoginPage.vue';
import ContatosPage from '../views/ContatosPage.vue';

const routes = [
  {
    path: '/login',
    name: 'login',
    component: LoginPage
  },
  {
    path: '/',
    redirect: '/contatos'
  },
  {
    path: '/contatos',
    name: 'contatos',
    component: ContatosPage,
    meta: { requiresAuth: true }
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const auth = useAuthStore();
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    next({ name: 'login' });
  } else if (to.name === 'login' && auth.isAuthenticated) {
    next({ name: 'contatos' });
  } else {
    next();
  }
});

export default router;
