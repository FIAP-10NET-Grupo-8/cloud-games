## Guia de Processo: Spikes e Provas de Conceito (POCs) com GitHub

Este documento descreve o nosso processo padrão para investigar incertezas técnicas, tomar decisões de arquitetura informadas e manter um registro dessas decisões usando o ecossistema do GitHub.

### Conceitos Fundamentais

  * **Spike:** Uma tarefa de investigação focada em responder uma pergunta específica para reduzir o risco técnico. O resultado de um Spike é **conhecimento documentado**.
  * **POC (Prova de Conceito):** Um projeto de código pequeno e isolado, criado para provar que uma tecnologia ou abordagem funciona na prática. O resultado de uma POC é **código experimental**.

### O Fluxo de Trabalho Passo a Passo

Siga estes passos sempre que encontrar uma incerteza que impeça o progresso ou a estimativa de uma tarefa.

#### Passo 1: Identificação e Criação da Issue

1.  Ao identificar uma incerteza, crie uma nova **Issue** em nosso repositório GitHub para rastrear o Spike. Use a label `spike` para facilitar a filtragem.
2.  Use o **número da Issue** (ex: `#451`) como prefixo para toda a documentação e código gerado. Para maior clareza em nomes de pastas e branches, usaremos o formato `GH-[Número]`.

#### Passo 2: Preparação da Estrutura de Pastas

Para manter a organização, toda a investigação deve residir em uma estrutura de pastas padronizada.

1.  No sistema de arquivos, crie a seguinte hierarquia de pastas:
    ```
    /spikes-e-pocs
        └── /[NomeDoDominio]
            └── /GH-[NúmeroDaIssue]-[Descricao-Curta]/
    ```
2.  No Visual Studio, replique essa mesma estrutura usando "Pastas de Solução" para manter a organização visual consistente com a física.

**Exemplo:**
`spikes-e-pocs/User/GH-451-Investigacao-Hashing-de-Senhas/`

#### Passo 3: A Investigação (O Documento do Spike)

A documentação é o artefato central do Spike.

1.  Localize o arquivo `TEMPLATE-SPIKE.md` na raiz da pasta `spikes-e-pocs`.
2.  Copie este template para a pasta da sua investigação recém-criada.
3.  Renomeie a cópia para **`SPIKE.md`**.
4.  Preencha o documento com sua pesquisa, as alternativas consideradas, os prós e contras, e a sua recomendação final.

#### Passo 4: A Prova de Conceito (A POC) - *Se Necessário*

Se a pesquisa teórica não for suficiente, crie uma POC para validar a abordagem recomendada.

1.  Dentro da mesma pasta da sua investigação, crie um novo projeto do tipo **Aplicativo de Console**.
2.  Nomeie o projeto seguindo o padrão: `[Nome.Do.Projeto].POCs.[Dominio].[Topico]` (ex: `Fiap.CloudGames.POCs.User.Hashing`).
3.  O código da POC deve ser focado e simples, com o único objetivo de provar o ponto da investigação.

#### Passo 5: Revisão e Decisão

Compartilhe suas descobertas com a equipe para validação.

  * O documento `SPIKE.md` serve como base para a discussão.
  * Se uma POC foi criada, abra um **Pull Request (PR) que não será mesclado**. O PR é a ferramenta perfeita para a revisão do código e a discussão. Vincule o PR à Issue original para manter a rastreabilidade completa dentro do GitHub.

#### Passo 6: Finalização

1.  Após a decisão da equipe, atualize a seção "Decisão" no arquivo `SPIKE.md`.
2.  Garanta que a seção "Prova de Conceito (POC)" esteja corretamente preenchida.
3.  Faça o commit de todos os artefatos (o `SPIKE.md` e a pasta do projeto da POC) e envie-os para o repositório.
4.  Feche o Pull Request (se houver) e, por fim, a **Issue** do Spike no GitHub.