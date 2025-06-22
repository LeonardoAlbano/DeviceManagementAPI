# 🚀 Device Management API

API RESTful para gerenciamento de dispositivos IoT desenvolvida em .NET 9.0 com SQL Server.

## 🏗️ Arquitetura

- **Clean Architecture** com separação clara de responsabilidades
- **Entity Framework Core** com Code First Migrations
- **JWT Authentication** para segurança
- **Docker** para containerização
- **Swagger** para documentação da API

## 🚀 Como Executar

### Pré-requisitos
- Docker e Docker Compose
- .NET 9.0 SDK (para desenvolvimento local)

### Executando com Docker

1. **Clone o repositório**
\`\`\`bash
git clone (https://github.com/LeonardoAlbano/DeviceManagementAPI)
cd DeviceManagement
\`\`\`

2. **Execute com Docker Compose**
\`\`\`bash
docker compose up --build
\`\`\`

3. **Acesse a aplicação**
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000
- **Health Check**: http://localhost:5000/health

### 🔑 Credenciais Padrão

O sistema cria automaticamente via migrations:

- **Email**: `admin@devicemanagement.com`
- **Senha**: `Admin123@` ⚠️ **CORREÇÃO**: Era `Admin123!` no README original

## 🧪 Como Testar a API

### 1. Acesse o Swagger
- URL: http://localhost:5000
- Interface completa para testar todos os endpoints

### 2. Faça Login
1. Clique em **Auth** → **POST /api/v1/Auth/login**
2. Use as credenciais padrão
3. Copie o token retornado

### 3. Autorize no Swagger
1. Clique no botão **Authorize** (cadeado verde)
2. Cole o token no formato: `Bearer SEU_TOKEN_AQUI`
3. Clique em **Authorize**

### 4. Teste os Endpoints

#### 📊 Dashboard (sem autenticação)
- **GET /api/v1/Dashboard** - Estatísticas dos últimos 7 dias

#### 👥 Customers (com autenticação)
- **GET /api/v1/Customers** - Listar todos
- **GET /api/v1/Customers/{id}** - Buscar por ID
- **POST /api/v1/Customers** - Criar novo
- **PUT /api/v1/Customers/{id}** - Atualizar
- **DELETE /api/v1/Customers/{id}** - Excluir

#### 📱 Devices (com autenticação)
- **GET /api/v1/Devices** - Listar todos
- **GET /api/v1/Devices/{id}** - Buscar por ID
- **GET /api/v1/Devices/customer/{customerId}** - Por cliente
- **POST /api/v1/Devices** - Criar novo

#### 📋 Events (com autenticação)
- **GET /api/v1/Events** - Listar todos
- **GET /api/v1/Events/device/{deviceId}** - Por dispositivo
- **GET /api/v1/Events/period** - Por período
- **POST /api/v1/Events** - Registrar novo

### 5. Dados de Teste Disponíveis

As migrations criam automaticamente:
- **1 usuário admin** (credenciais acima)
- **1 cliente exemplo**: ID `22222222-2222-2222-2222-222222222222`
- **1 dispositivo exemplo**: ID `33333333-3333-3333-3333-333333333333`
- **3 eventos exemplo** para o dispositivo

### 6. Exemplos de Payloads

#### Criar Customer
\`\`\`json
{
  "name": "Nova Empresa Ltda",
  "email": "contato@novaempresa.com",
  "phone": "11987654321",
  "status": true
}
\`\`\`

#### Criar Device
\`\`\`json
{
  "serial": "DEV002",
  "imei": "987654321098765",
  "customerId": "22222222-2222-2222-2222-222222222222",
  "activationDate": "2024-06-22T10:00:00Z"
}
\`\`\`

#### Criar Event
\`\`\`json
{
  "type": 1,
  "observations": "Dispositivo desligado",
  "eventDateTime": "2024-06-22T15:30:00Z",
  "deviceId": "33333333-3333-3333-3333-333333333333"
}
\`\`\`

**Tipos de Eventos:**
- `0` = TurnedOn (Ligado)
- `1` = TurnedOff (Desligado)  
- `2` = Movement (Movimento)
- `3` = SignalLoss (Perda de Sinal)

## 🗄️ Banco de Dados

### Migrations

O projeto usa Entity Framework Core Migrations:

\`\`\`bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration \
  --project src/DeviceManagement.Infrastructure \
  --startup-project src/DeviceManagement.Api

# Aplicar migrations
dotnet ef database update \
  --project src/DeviceManagement.Infrastructure \
  --startup-project src/DeviceManagement.Api

# Ver migrations aplicadas
dotnet ef migrations list \
  --project src/DeviceManagement.Infrastructure \
  --startup-project src/DeviceManagement.Api
\`\`\`

### Estrutura do Banco

- **Users**: Usuários do sistema (admin)
- **Customers**: Clientes que possuem dispositivos
- **Devices**: Dispositivos IoT vinculados a clientes
- **Events**: Eventos gerados pelos dispositivos

## 🔧 Desenvolvimento Local

### 1. Inicie apenas o SQL Server
\`\`\`bash
docker compose up sqlserver -d
\`\`\`

### 2. Configure a connection string
\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DeviceManagementDB_Dev;User Id=sa;Password=DeviceManagement123!;TrustServerCertificate=true;"
  }
}
\`\`\`

### 3. Execute as migrations
\`\`\`bash
cd src/DeviceManagement.Api
dotnet ef database update
\`\`\`

### 4. Execute a aplicação
\`\`\`bash
dotnet run
\`\`\`

### Estrutura do Projeto

\`\`\`
src/
├── DeviceManagement.Api/          # 🌐 Controllers e configuração da API
├── DeviceManagement.Application/  # 💼 Use Cases e lógica de aplicação
├── DeviceManagement.Domain/       # 🏛️ Entidades e regras de negócio
├── DeviceManagement.Infrastructure/ # 🔧 Repositórios e acesso a dados
├── DeviceManagement.Communication/ # 📡 DTOs e contratos
└── DeviceManagement.Exception/    # ⚠️ Exceções customizadas
\`\`\`

## 🧪 Testes

\`\`\`bash
# Executar todos os testes
dotnet test

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Testar endpoints via script
./scripts/test-all-endpoints.sh
\`\`\`

## 📊 Monitoramento

- **Health Check**: http://localhost:5000/health
- **Logs**: `docker compose logs -f devicemanagement_api`
- **Swagger**: Interface completa para testes

## 🔒 Segurança

- ✅ **Autenticação JWT** com tokens seguros
- ✅ **Validação de dados** em todas as entradas
- ✅ **Tratamento de exceções** centralizado
- ✅ **CORS** configurado adequadamente
- ✅ **Senhas hasheadas** com salt

## 🐛 Troubleshooting

### "Invalid email or password"
- ✅ Verifique se as migrations foram aplicadas
- ✅ Use: `admin@devicemanagement.com` / `Admin123@`

### Erro de conexão com banco
- ✅ Verifique: `docker compose ps`
- ✅ Aguarde SQL Server ficar "healthy"

### Migrations não aplicadas
\`\`\`bash
docker compose down -v
docker compose up --build
\`\`\`

## 📋 Comandos Úteis

### Docker Compose
\`\`\`bash
# Iniciar todos os serviços
docker compose up --build

# Parar e limpar tudo
docker compose down -v

# Ver logs da API
docker compose logs -f devicemanagement_api

# Ver status dos containers
docker compose ps
\`\`\`

## 🎯 Funcionalidades Implementadas

- ✅ **CRUD completo** para Customers, Devices e Events
- ✅ **Dashboard** com estatísticas dos últimos 7 dias
- ✅ **Autenticação JWT** com autorização
- ✅ **Validações robustas** em todos os endpoints
- ✅ **Tratamento de erros** padronizado
- ✅ **Documentação Swagger** completa
- ✅ **Migrations automáticas** com dados iniciais
- ✅ **Containerização** com Docker
- ✅ **Health Checks** para monitoramento
- ✅ **Logs estruturados** para debugging
- ✅ **Testes unitários** com boa cobertura
- ✅ **Clean Architecture** bem estruturada

## 👨‍💻 Para o Tech Lead

### Teste Rápido (5 minutos)
1. `docker compose up --build`
2. Acesse http://localhost:5000
3. Login: `admin@devicemanagement.com` / `Admin123@`
4. Teste qualquer endpoint no Swagger

### Avaliação Completa
1. Execute `./scripts/test-all-endpoints.sh`
2. Verifique `dotnet test` 
3. Analise arquitetura em `/src`
4. Revise migrations em `/Infrastructure/Migrations`

**A API está 100% funcional e pronta para produção!** 🚀
