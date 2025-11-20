# Agenda de Contatos (.NET 8 + Vue 3)

- Backend em **.NET 8 Web API**
  - Entity Framework Core + SQLite
  - Padrão de repositório + camada de aplicação
  - CQRS simples com MediatR
  - AutoMapper
  - FluentValidation
  - JWT para autenticação
  - Swagger
- Frontend em **Vue 3 + Vite**
  - PrimeVue + PrimeFlex
  - Pinia para estado global
  - Axios com interceptor de JWT
  - Componentização de tela de login e CRUD de contatos
- Docker + docker-compose para subir tudo rapidamente

## Como rodar com Docker

Pré-requisitos:

- Docker
- docker-compose

Na raiz do projeto:

```bash
docker-compose build
docker-compose up
```

Serviços:

- Backend: http://localhost:5000/swagger
- Frontend: http://localhost:5173

Usuário de teste:

- **Usuário:** `admin`
- **Senha:** `admin123`

## Como rodar localmente (sem Docker)

### Backend

```bash
cd src/Agenda.Api
dotnet restore
dotnet ef database update  # opcional, já há migrations
dotnet run
```

API disponível em: http://localhost:5000

Swagger: http://localhost:5000/swagger

### Frontend

```bash
cd frontend/agenda-web
npm install
npm run dev
```

Frontend disponível em: http://localhost:5173

## Fluxo principal

1. Acessar `/login` no frontend
2. Autenticar com `admin` / `admin123`
3. CRUD completo de contatos:
   - Listar
   - Criar
   - Editar
   - Excluir

Toda chamada ao backend `/api/contatos` exige JWT (`[Authorize]`), e o frontend injeta o token automaticamente via interceptor do Axios.
