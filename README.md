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
