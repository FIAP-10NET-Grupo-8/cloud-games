# Spike: Investigação de Biblioteca para Hashing de Senhas

**Data:** 2025-10-17

---

### 1. Objetivo (A Pergunta)

> O projeto precisa armazenar senhas de usuários de forma segura. É necessário escolher uma biblioteca de hashing que seja padrão de mercado, robusta contra ataques de força bruta e de simples implementação em .NET. Qual é a melhor abordagem?

---

### 2. Investigação e Alternativas Consideradas

Foram pesquisadas alternativas para hashing de senhas, com foco em algoritmos que incluem "salt" automaticamente para prevenir ataques.

* **Alternativa A: `System.Security.Cryptography` nativo do .NET**
    * **Descrição:** Usar a classe `Rfc2898DeriveBytes` que implementa o algoritmo PBKDF2.
    * **Prós:**
        * Não requer dependências externas, faz parte do framework .NET.
    * **Contras:**
        * **Complexidade:** Requer gerenciamento manual do "salt" (gerar, armazenar e recuperar o salt junto com o hash).
        * **Risco de Erro:** A complexidade manual aumenta a chance de uma implementação insegura.
        * **Verbosidade:** O código para gerar e verificar o hash é significativamente mais longo e complexo.
    * **Fontes:**
        * Microsoft Docs — `Rfc2898DeriveBytes` (PBKDF2) — descreve uso e exemplos, confirmando que é nativo e exige gerenciamento de salt/parametrização: https://learn.microsoft.com/dotnet/api/system.security.cryptography.rfc2898derivebytes
        * Microsoft Docs — `CryptographicOperations.FixedTimeEquals` — recomenda comparação em tempo fixo para evitar ataques de timing (aplicável ao método `Verify` na implementação nativa): https://learn.microsoft.com/dotnet/api/system.security.cryptography.cryptographicoperations.fixedtimeequals
        * OWASP — Password Storage Cheat Sheet — explica recomendações (salt, iterações, algoritmos) e aborda prós/contras de implementar PBKDF2 manualmente: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
        * RFC 2898 — PKCS #5: Password-Based Cryptography Specification (descrição do PBKDF2): https://datatracker.ietf.org/doc/html/rfc2898

* **Alternativa B: Biblioteca `BCrypt.Net-Next`**
    * **Descrição:** Implementação do algoritmo BCrypt, amplamente considerado o padrão da indústria para hashing de senhas.
    * **Prós:**
        * **Segurança:** Desenhado para ser lento, o que o torna resistente a ataques de força bruta.
        * **Simplicidade:** A API é extremamente simples (`HashPassword` e `Verify`).
        * **Gerenciamento de Salt:** Gera e embute o "salt" automaticamente no hash final, eliminando a necessidade de gerenciamento manual e o risco de erros.
    * **Contras:** Requer a adição de uma dependência externa (pacote NuGet).
    * **Fontes:**
        * Repositório `BCrypt.Net-Next` (README) — documenta a API `HashPassword`/`Verify` e o comportamento do salt embutido: https://github.com/BcryptNet/bcrypt.net
        * OWASP — Password Storage Cheat Sheet — lista bcrypt como uma opção aprovada e explica trade-offs entre algoritmos e dependências: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
        * Página do pacote NuGet `BCrypt.Net-Next` — mostra manutenção e uso comum em .NET: https://www.nuget.org/packages/BCrypt.Net-Next/

---

### 3. Conclusão da Pesquisa

A biblioteca `BCrypt.Net-Next` é a escolha superior. A simplicidade de sua API abstrai a complexidade do gerenciamento de "salt" e garante uma implementação segura com muito menos esforço e risco de erro. O pequeno custo de adicionar uma dependência é amplamente superado pelos benefícios de segurança e simplicidade.

---

### 4. Decisão

Adotaremos a biblioteca `BCrypt.Net-Next` como padrão para hashing de senhas no projeto. A lógica será encapsulada dentro do Value Object `Password`, no Domínio `User`.

---

### 5. POC Relacionada (Se Aplicável)

* **[ ] Não foi necessária.** A decisão foi tomada com base na documentação e análise teórica.
* **[X] Sim.** O código que demonstra a viabilidade da solução está no projeto de POC