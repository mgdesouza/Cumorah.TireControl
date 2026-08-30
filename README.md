# 🛞 PneuControl

Sistema para gerenciamento e controle do ciclo de vida de pneus utilizados em máquinas e equipamentos pesados.

O **PneuControl** permite acompanhar todo o histórico do pneu, desde sua entrada no estoque até sua montagem, inspeções, movimentações, reformas e descarte.

O sistema será composto por uma API central, uma aplicação Web administrativa e um aplicativo Mobile para utilização em campo.

---

# 🎯 Objetivo

Desenvolver uma plataforma para controle completo da utilização e vida útil dos pneus em máquinas pesadas.

O sistema deverá permitir:

- Cadastro e identificação de pneus
- Controle de estoque
- Cadastro de máquinas e equipamentos
- Controle de montagem e desmontagem
- Rodízio e transferência de pneus
- Inspeções periódicas
- Controle de pressão e profundidade dos sulcos
- Registro de horímetro e quilometragem
- Histórico completo do pneu
- Controle de reformas e recapagens
- Controle de custos
- Alertas e notificações
- Dashboards e indicadores
- Relatórios operacionais e gerenciais
- Utilização através de aplicação Web
- Utilização em campo através de aplicativo Mobile
- Operação offline e sincronização futura

---

# 🏗️ Arquitetura

O projeto utiliza uma arquitetura baseada nos princípios de **Clean Architecture**, separando claramente as responsabilidades da aplicação.

```text
┌─────────────────────────────────────────────┐
│                 PneuControl                 │
├─────────────────────────────────────────────┤
│                                             │
│              Web        Mobile              │
│               │            │                │
│               └──────┬─────┘                │
│                      │                      │
│                     API                     │
│                      │                      │
│                Application                  │
│                      │                      │
│                    Domain                   │
│                      ▲                      │
│                      │                      │
│                Infrastructure               │
│                      │                      │
│                 SQL Server                  │
│                                             │
└─────────────────────────────────────────────┘
```

---

## 🔐 Autenticação e autorização da API

A API usa tokens JWT *Bearer*. Todos os endpoints são protegidos por padrão; apenas
`POST /api/auth/login` permite acesso anônimo. O token inclui as *roles* e as permissões
associadas ao usuário ativo, para que endpoints possam exigir uma permissão específica,
por exemplo: `[Authorize(Policy = AuthorizationPermissions.PneuCreate)]`.

Antes de iniciar a API, configure a chave de assinatura JWT fora do repositório. Ela deve
ter ao menos 32 caracteres:

```bash
export Jwt__Key='uma-chave-secreta-com-pelo-menos-32-caracteres'
export ConnectionStrings__Default='Server=...;Database=...;...'
```

As senhas dos usuários devem ser gravadas usando `IPasswordHasher<Usuario>`. O login
aceita e-mail e senha e responde com o `accessToken`, cujo uso é feito por meio do cabeçalho
`Authorization: Bearer <accessToken>`.
