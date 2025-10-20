# Processo de Colaboração — FIAP Cloud Games

Este documento descreve o fluxo de trabalho recomendado para colaborar no repositório. Use-o como referência para criar issues, branches e pull requests de forma padronizada.

## Visão geral do fluxo

1. Criar a Issue no GitHub.
2. Criar branch baseada no número da Issue.
3. Iniciar trabalho (atualizar status e assumir a Issue).
4. Desenvolver com commits claros e pequenos.
5. Abrir Pull Request vinculado à Issue.
6. Revisão, aprovação e merge.

## Regras para Issues

- Sempre crie uma Issue para todo trabalho novo.
- O título da Issue deve começar com o tipo: `feature:`, `bug:`, `docs:`, `chore:`, `spike:`, etc.
  - Exemplo: `feature: endpoint de login com JWT`.
- A Issue deve ter ao menos uma label relevante.
- Adicione a Issue ao projeto `Cloud Games (TC1)` e marque com o status `To do (Ready)`.
- Se a Issue depende de outra, registre o relacionamento em *Relationships* com `blocked by #NúmeroDaIssue` ou `blocks #NúmeroDaIssue`.

## Branch

- Crie a branch logo após abrir a Issue, seguindo o padrão:
  - `GH-[NúmeroDaIssue]-[DescricaoCurta]` (ex: `GH-123-ajustar-login`).
  - Use `-` para separar palavras; mantenha a descrição curta e sem caracteres especiais.

## Início do trabalho

- Ao começar, atualize o cartão da Issue para `In Progress` no projeto `Cloud Games (TC1)`.
- Adicione seu usuário em `Assignees` para sinalizar quem está trabalhando.

## Commits

- Prefira commits pequenos e atômicos.
- Use mensagens claras. Sugestão de formato: `tipo(scope): descrição curta` (opcional):
  - `feat(auth): adicionar rota de refresh token`
  - `fix(users): corrigir validação de e-mail`
- Inclua notas importantes no corpo do commit quando necessário.
- Se o merge fechar a Issue, inclua no último commit ou no PR: `Closes #NúmeroDaIssue`.

## Pull Request (PR)

- Ao finalizar, atualize o status da Issue para `Review / QA`.
- Abra um PR vinculado à Issue e selecione ao menos um revisor.
- No PR inclua:
  - Descrição do que foi feito e por quê.
  - Referência à Issue (ex: `Closes #123`).
  - Checklist de validação (ex.: testes unitários, passos manuais).
  - Capturas de tela ou logs quando relevantes.
- Marque reviewers e, se aplicável, assignees para QA.

## Revisão e Merge

- Aguarde aprovação de pelo menos um revisor.
- Realize o merge seguindo a estratégia do time (squash, rebase ou merge).
- Verifique se a Issue foi fechada automaticamente; caso negativo, feche manualmente.

## Boas práticas e recomendações

- Escreva ou atualize testes automatizados quando aplicável.
- Mantenha PRs pequenos e focados.
- Documente decisões importantes no PR ou na Issue.
- Para spikes e POCs: siga o padrão em `spikes-e-pocs/README.md` e inclua o número da Issue no nome da pasta e da branch.
- Use labels e relationships para comunicar bloqueios e dependências.

## Modelos úteis (sugestões)

- Título de Issue: `tipo: descrição curta` (ex: `docs: atualizar README de deploy`).
- Branch: `GH-<Número>-<descrição>` (ex: `GH-42-corrigir-timezone`).
- Mensagem de commit final: `feat(api): implementar login — Closes #42`.

---

Este processo pode evoluir. Proponha melhorias criando uma Issue com `docs:` para discussão.