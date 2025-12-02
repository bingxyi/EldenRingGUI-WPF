# 🛡️ Elden Ring Items Manager — API + WPF

> Sistema completo para gerenciamento de itens do jogo Elden Ring, contendo:

✔ API REST em ASP.NET Core

✔ Interface desktop em WPF

✔ CRUD completo de itens e categorias

✔ Integração 100% funcional entre API ↔ WPF

___________

## 📌 Tecnologias Utilizadas

- .NET 9

- ASP.NET Core Web API

- Entity Framework Core

- SQLite (ou SQL Server, dependendo do seu setup)

- WPF (.NET 9)

- HttpClient

_______________

## 🧠 Arquitetura Geral
```pgsql
┌────────────┐        HTTP/JSON        ┌────────────────────────┐
│  WPF App   │  <------------------->  │      Elden Ring API    │
│ (Front-End)│                         │  (ASP.NET Core + EF)   │
└────────────┘                         └────────────────────────┘
```
_______________

## 🎮 Entidades do Sistema
#### 🗃️ Item

> Representa um item do jogo Elden Ring.

Tabela
Campo	Tipo	Descrição
Id	int	Identificador único
Name	string	Nome do item
Rarity	string	Pode ser Comum, Raro, Épico, Lendária
Price	int	Preço em runas
Description	string	Descrição detalhada do item
ItemCategoryId	int	Chave estrangeira para categoria
ItemCategory	Category	Navegação

#### 🏷️ Category

> Categoria/classificação de itens.

Campo	Tipo	Descrição
Id	int	Identificador
Name	string	Nome da categoria (ex: Katana, Escudo, Feitiço)

____________

## 🚀 Como Rodar o Projeto

#### 🛠️ 1. Clonar o repositório
```bash
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
```

#### 🗂️ 2. Ir para o projeto da API
```bash
cd EldenRing.Api
```

#### 📦 3. Criar o banco de dados

> Rodar migrations:
```bash
dotnet ef database update
```

> Se quiser recriar:
```bash
dotnet ef database drop -f
dotnet ef database update
```

#### ▶️ 4. Rodar a API
```bash
dotnet run
```

A API iniciará normalmente em:
```http
http://localhost:5067
```
#### 🖥️ 5. Rodar o projeto WPF

Em outro terminal:
```bash
cd EldenRing.Wpf
dotnet run
```

> A GUI irá carregar automaticamente os itens e categorias da API.

_____________

## 🛣️ Rotas da API (Endpoints)
#### 📌 GET /api/Items

> Retorna todos os itens.

Exemplo de resposta:

```http
[
  {
    "id": 1,
    "name": "Moonveil",
    "rarity": "Lendária",
    "price": 8000,
    "description": "Katana mágica...",
    "itemCategoryId": 1
  }
]
```

#### 📌 GET /api/Items/{id}

> Retorna um item específico.

#### 📌 POST /api/Items

> Cria um novo item.

Corpo esperado:
```http
{
  "name": "Rivers of Blood",
  "rarity": "Lendária",
  "price": 12000,
  "description": "Katana infernal...",
  "itemCategoryId": 1
}
```

#### 📌 DELETE /api/Items/{id}

> Remove um item pelo ID.

#### 📌 GET /api/Categories

> Lista todas as categorias.

___________

## 🧪 Como Testar a API
**1. Via Postman
**
- Importe a collection ou crie requisições GET/POST/DELETE manualmente

- Configure o body como raw → JSON

- URL base: http://localhost:5067/api

**2. Via Thunder Client (VS Code)
**
- Instale a extensão

- Crie requisições simples iguais ao Postman

**3. Via arquivos .http (Recomendado para Devs)
**
- Crie um arquivo test.http:

### Listar itens
GET http://localhost:5067/api/Items

### Criar item
POST http://localhost:5067/api/Items
```Content-Type: application/json```
```http
{
  "name": "Dragon Slayer",
  "rarity": "Épico",
  "price": 6000,
  "description": "Espada forjada em chamas dracônicas.",
  "itemCategoryId": 2
}
```

### Deletar item
DELETE http://localhost:5067/api/Items/1


> No VS Code, clique em Send Request.

____________

## 🖼️ Interface WPF

A aplicação exibe:

✔ Lista de itens

✔ Categoria resolvida automaticamente

✔ Descrição com quebra de linha

✔ Botão de refresh

✔ Botão de excluir

✔ Formulário para adicionar novos itens
