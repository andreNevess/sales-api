# Sales API - Developer Evaluation

Este projeto é uma API para gerenciamento de vendas, desenvolvida como parte de um desafio técnico.

## Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core (com PostgreSQL e InMemory para testes)
- AutoMapper
- xUnit + FluentAssertions
- Docker + Docker Compose
- Bogus (geração de dados fake)

## Regras de negócio

- Compras com:
  - 4 a 9 unidades do mesmo produto: 10% de desconto
  - 10 a 20 unidades: 20% de desconto
  - Acima de 20: não permitido
  - Abaixo de 4: sem desconto

## Funcionalidades

- [x] Criar venda `POST /api/sales`
- [x] Listar todas as vendas `GET /api/sales`
- [x] Buscar venda por ID `GET /api/sales/{id}`
- [x] Cancelar venda `PUT /api/sales/{id}/cancel`
- [x] Eventos simulados no console (`SaleCreated`, `SaleCancelled`)
- [x] Testes unitários com banco em memória
- [x] Endpoint de seeding: `POST /api/sales/seed`
- [x] Seeding automático com variável `SEED_FAKE_DATA=true`

## Como executar

### Pré-requisitos

- Docker + Docker Compose
- .NET 8 SDK

### Subir o banco:

docker-compose up -d
