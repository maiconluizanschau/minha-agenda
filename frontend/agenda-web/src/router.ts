import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import { useAuthStore } from './store/useAuthStore';
import LoginPage from './views/LoginPage.vue';
import ContatosPage from './views/ContatosPage.vue';

const routes: RouteRecordRaw[] = [
  { path: '/login', name: 'login', component: LoginPage },
  { path: '/', name: 'home', component: ContatosPage, meta: { requiresAuth: true } }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, _from, next) => {
  const auth = useAuthStore();
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    next({ name: 'login' });
  } else if (to.name === 'login' && auth.isAuthenticated) {
    next({ name: 'home' });
  } else {
    next();
  }
});

export default router;
