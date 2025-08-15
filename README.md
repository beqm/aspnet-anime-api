# Anime API

Como iniciar a aplicação **Anime API** utilizando Docker Compose, incluindo banco de dados SQL Server, CloudBeaver e a API.

---

## Requisitos

- Docker
- Docker Compose

ou

- Podman
- podman-compose

---

## Estrutura do Docker Compose

O `docker-compose.yml` inclui três serviços:

1. **sqlserver** - SQL Server para armazenar os dados.
2. **cloudbeaver** - Interface web para gerenciar o banco.
3. **api** - API .NET da aplicação Anime.

Além disso, volumes nomeados para persistência de dados e logs:

- `sqlserver-volume` → dados do SQL Server
- `cloudbeaver-volume` → workspace do CloudBeaver
- `log-volume` → logs de erro da API

---

## Como iniciar a aplicação

No terminal, execute:

```bash
docker-compose up --build
```

# Acessando os serviços

## API Swagger

- URL: http://localhost:5000/swagger/index.html

Permite testar todos os endpoints da API diretamente pelo navegador.

## Host do servidor: sqlserver

- Porta do servidor: 1433
- Usuário: sa
- Senha: 3Hq2LN0o6i

## CloudBeaver (Interface de gerenciamento SQL)

- URL: http://localhost:5100

Será necessário configurar manualmente usuário e senha para acessar o banco.
