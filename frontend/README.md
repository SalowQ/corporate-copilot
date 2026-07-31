# Corporate Copilot AI

Frontend do assistente corporativo com IA. Interface no estilo ChatGPT, visual corporativo (azul/branco), consumindo a API `CorporateCopilot.Api`.

Faz parte do monorepo **CorporateCopilot**. Documentação da API e visão geral: [README raiz](../README.md).

---

## Preciso de um repositório?

**Sim.** A Vercel publica a partir do GitHub/GitLab.

Neste projeto o frontend fica na pasta `frontend/` do **mesmo repositório** da API. Na Vercel você aponta o **Root Directory** para `frontend`.

Não é necessário um repositório separado.

---

## Stack

- Vue 3 (Composition API + `<script setup>`)
- Vite + TypeScript
- Pinia + Vue Router
- Axios
- Tailwind CSS + Heroicons
- marked + DOMPurify + highlight.js

## Instalação

```bash
cd frontend
npm install
```

## Execução local

1. Suba a API em `http://localhost:8080` (ver README raiz)
2. O arquivo `.env.development` já contém:

```env
VITE_API_BASE_URL=http://localhost:8080/api
VITE_APP_VERSION=1.0.0
```

3. Inicie o frontend:

```bash
npm run dev
```

Acesse: [http://localhost:5173](http://localhost:5173)

## Scripts

| Comando | Descrição |
| --- | --- |
| `npm run dev` | Desenvolvimento |
| `npm run build` | Build de produção |
| `npm run preview` | Preview do build |

## Estrutura

```text
frontend/
├── public/
├── src/
│   ├── components/
│   │   ├── layout/      # Sidebar
│   │   ├── chat/        # ChatWindow, ChatMessage, ChatInput, Loading, EmptyState
│   │   └── common/      # Markdown, Copiar, Erro
│   ├── views/ChatView.vue
│   ├── stores/conversation.ts
│   ├── services/api.ts, chatService.ts
│   ├── types/chat.ts
│   ├── router/
│   ├── App.vue
│   └── main.ts
├── .env.development
├── .env.example
├── vercel.json
└── README.md
```

## Consumo da API

| Ambiente | `VITE_API_BASE_URL` |
| --- | --- |
| Local | `http://localhost:8080/api` |
| Produção | `https://SUA-API.onrender.com/api` |

Endpoint:

```http
POST /chat
Content-Type: application/json

{ "message": "Como solicitar férias?" }
```

Resposta:

```json
{ "answer": "..." }
```

## Variáveis de ambiente

Arquivo de referência: `.env.example`

| Variável | Obrigatória | Descrição |
| --- | --- | --- |
| `VITE_API_BASE_URL` | Sim | URL base da API **incluindo** `/api` |
| `VITE_APP_VERSION` | Não | Versão exibida na sidebar (padrão `1.0.0`) |

> Variáveis `VITE_*` são embutidas no build. Depois de alterar na Vercel, é preciso **Redeploy**.

---

## Publicar na Vercel (passo a passo)

### Pré-requisitos

1. Código no GitHub (mesmo repo da API)
2. API já publicada no Render (ou publique o front depois e atualize a env)
3. Conta gratuita na [Vercel](https://vercel.com)

### 1. Importar o projeto

1. Acesse [vercel.com/new](https://vercel.com/new)
2. Importe o repositório `CorporateCopilot`
3. Antes de Deploy, abra **Build and Output Settings** / **Root Directory**:
   - **Root Directory:** `frontend`
   - **Framework Preset:** Vite
   - **Build Command:** `npm run build`
   - **Output Directory:** `dist`
   - **Install Command:** `npm install`

### 2. Environment Variables (Production)

| Name | Value (exemplo) |
| --- | --- |
| `VITE_API_BASE_URL` | `https://corporate-copilot-api.onrender.com/api` |
| `VITE_APP_VERSION` | `1.0.0` |

Substitua pela URL real da sua API no Render.  
**Importante:** termine com `/api` (sem barra no final).

### 3. Deploy

1. Clique em **Deploy**
2. Aguarde o build
3. Copie a URL, ex.: `https://corporate-copilot-ai.vercel.app`

### 4. Liberar CORS na API (Render)

No dashboard do Render → Environment da API:

| Key | Value |
| --- | --- |
| `Cors__AllowedOrigins__0` | `https://corporate-copilot-ai.vercel.app` |

(use exatamente a URL do front, sem barra no final)

Depois: **Manual Deploy → Deploy latest commit** (ou Restart) na API.

Se usar domínio customizado ou URL de preview da Vercel, adicione também:

| Key | Value |
| --- | --- |
| `Cors__AllowedOrigins__1` | `https://seu-preview.vercel.app` |

### 5. Validar produção

1. Abra o site na Vercel
2. Envie: “Como solicitar férias?”
3. Se a API estiver “dormindo” (Render Free), aguarde o cold start na primeira chamada

O arquivo `vercel.json` já faz rewrite SPA (`/(.*)` → `/index.html`).

---

## Ordem recomendada de publicação

```text
1. Push no GitHub
2. Deploy da API no Render  → obter URL da API
3. Deploy do frontend na Vercel com VITE_API_BASE_URL
4. Configurar Cors__AllowedOrigins__0 no Render com URL da Vercel
5. Redeploy da API
6. Testar o chat em produção
```

---

## Prints esperados

### Tela inicial

- Sidebar azul com **Corporate Copilot**
- Botões Nova conversa / Limpar conversa
- Sugestões de perguntas
- Input fixo na parte inferior

### Conversa

- Mensagens com avatar, nome e data
- Markdown na resposta da IA
- Botão Copiar
- “A IA está pensando...” durante o loading

### Mobile

- Menu hambúrguer + sidebar em overlay

---

## Qualidade

- Composition API + TypeScript
- Componentes pequenos (< ~200 linhas)
- Store Pinia + interceptor Axios
- Markdown sanitizado (DOMPurify)
- Layout responsivo
