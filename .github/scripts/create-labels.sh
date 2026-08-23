#!/bin/bash

# Script para criar labels no repositório
# Uso: ./create-labels.sh <token> <owner> <repo>

TOKEN=$1
OWNER=$2
REPO=$3

if [ -z "$TOKEN" ] || [ -z "$OWNER" ] || [ -z "$REPO" ]; then
  echo "Uso: $0 <token> <owner> <repo>"
  exit 1
fi

# Array de labels
declare -a LABELS=(
  "Type: Epic|3E1414|Grande iniciativa que compreende múltiplas features"
  "Type: Feature|0366D6|Nova funcionalidade a ser implementada"
  "Type: Bug|D73A49|Problema ou defeito identificado"
  "Type: Task|A2EEEF|Tarefa de suporte ou manutenção"
  "Type: SubTask|7057FF|Subtarefa de uma issue maior"
  "Phase: Analysis|FBCA04|Fase de análise e planejamento"
  "Phase: Development|0075CA|Fase de desenvolvimento"
  "Phase: Testing|E99695|Fase de testes e qualidade"
  "Phase: Deploy|34A853|Fase de implantação em produção"
  "Priority: Critical|B60205|Bloqueador - deve ser resolvido imediatamente"
  "Priority: High|D93F0B|Alta prioridade"
  "Priority: Medium|FBCA04|Prioridade média"
  "Priority: Low|0E8A16|Baixa prioridade"
  "Status: Backlog|CFD3D7|Aguardando início"
  "Status: In Progress|0366D6|Atualmente em desenvolvimento"
  "Status: Review|1D76DB|Aguardando revisão"
  "Status: Blocked|B60205|Bloqueada por outra issue"
  "Status: Done|28A745|Concluído e validado"
  "Area: Backend|1D76DB|Relacionado ao backend/API"
  "Area: Frontend|C2E0C6|Relacionado ao frontend/UI"
  "Area: Database|D4C5F9|Relacionado a banco de dados"
  "Area: Infrastructure|FCE1A9|Relacionado a infraestrutura e deploy"
  "Area: Documentation|E6F7F5|Relacionado a documentação"
  "Area: Security|4C3E37|Relacionado a segurança"
)

# Criar labels
for label in "${LABELS[@]}"; do
  IFS='|' read -r name color description <<< "$label"
  
  curl -X POST \
    -H "Authorization: token $TOKEN" \
    -H "Accept: application/vnd.github.v3+json" \
    "https://api.github.com/repos/$OWNER/$REPO/labels" \
    -d "{\"name\":\"$name\",\"color\":\"$color\",\"description\":\"$description\"}"
  
  echo "Label '$name' criado"
done

echo "✅ Todos os labels foram criados!"
