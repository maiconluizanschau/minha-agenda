# Agenda

Este projeto é um frontend **Vue 3 + PrimeVue 4 + Vite** pensado para deixar a sua agenda de contatos.

## Principais destaques

- Layout moderno com **header fixo**, cartão central e responsivo.
- **Tema Aura** do PrimeVue com suporte a **modo claro/escuro** (toggle no topo).
- Tela de **login** simples (usuario/senha de exemplo: `admin` / `admin123`).
- Tela de **contatos** com:
  - Cards de resumo (total de contatos, favoritos).
  - Busca por nome, e-mail ou telefone.
  - Filtro de favoritos.
  - Tabela com paginação, ações de editar/excluir e favorito.
  - **Estado vazio bonito** quando não há contatos.
  - Modal para criar/editar contato.

## Integração com a API

A URL base da API está configurada em `src/services/api.ts`:

```ts
const api = axios.create({
  baseURL: 'http://localhost:5000/api'
});
```

Endpoints esperados (iguais ao seu backend atual):

- `POST /auth/login` → retorna um **JWT** (campo `token` ou o token direto).
- `GET /contatos`
- `POST /contatos`
- `PUT /contatos/{id}`
- `DELETE /contatos/{id}`

O token é salvo em `localStorage` com a chave `agenda_token` e enviado automaticamente no header `Authorization: Bearer ...`.

## Como rodar

Dentro da pasta do projeto:

```bash
npm install
npm run dev
```

Depois é só abrir: `http://localhost:5173`

Se estiver usando Docker para a API, mantenha a API em `http://localhost:5000`.

## Modo escuro

O modo escuro é controlado adicionando a classe `dark` no `<html>`.  
O estado é salvo em `localStorage` com a chave `agenda_theme_dark`.
---
