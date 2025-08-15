# 🟢 Fiotec - SERPRO API

**Descrição:**  
API responsável por consultar informações de CPF utilizando o serviço do SERPRO.  
Fornece informações detalhadas como filiação, endereço, documentos e CNH.

---

## 📌 Funcionalidades

- Consulta de CPF no SERPRO
- Validação de CPF
- Tratamento de exceções padronizado:
  - CPF inválido
  - Serviço indisponível
  - Erros internos
- Logs estruturados com Serilog
- Testes unitários 
- Documentação Swagger

---

## 🛠 Tecnologias

- **.NET 9** (ASP.NET Core Web API)
- **Serilog** para logging
- **Swagger / OpenAPI** para documentação
- **Moq** para testes unitários
- **XUnit** para testes
- **HttpClientFactory** para integração com SERPRO
- **JSON** para serialização de dados

---

## 🚀 Endpoints

### **Consultar CPF**

GET /api/serpro/validar-cpf/{cpf}


**Parâmetros:**  

| Parâmetro | Tipo   | Obrigatório | Descrição                 |
|-----------|--------|-------------|---------------------------|
| cpf       | string | Sim         | CPF a ser consultado      |

**Respostas possíveis:**

| Código | Descrição                   | Exemplo de retorno |
|--------|-----------------------------|------------------|
| 200    | CPF válido e disponível      | `{ "cpfDisponivel": true, "documento": {...} }` |
| 400    | CPF inválido                 | `{ "erro": "CPF inválido" }` |
| 503    | Serviço indisponível         | `{ "erro": "Não foi possível obter token do SERPRO" }` |
| 500    | Erro interno                 | `{ "erro": "Erro interno do servidor" }` |

---


## 📖 Logs

- Utiliza **Serilog** para logs estruturados
- Logs gravados em console e em arquivo:
  
C:\Fiotec\Logs\Serpro\serpro-log-<DATA>.txt

- Possibilidade de ajustar caminho no `appsettings.json` ou em `Program.cs`

---

# 🧪 Testes

Testes unitários usando XUnit e Moq

Cenários testados:
- Erro de conexão com API
- Validação correta do CPF
- Tratamento de exceções
