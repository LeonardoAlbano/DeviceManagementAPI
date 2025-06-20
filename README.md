# Device Management API

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
git clone <seu-repositorio>
cd DeviceManagement
\`\`\`

2. **Execute com Docker Compose**
\`\`\`bash
docker compose up --build
\`\`\`

3. **Acesse a aplicação**
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Credenciais Padrão

O sistema cria automaticamente via migrations:

- **Email**: `admin@devicemanagement.com`
- **Senha**: `Admin123!`

## 🗄️ Banco de Dados

### Migrations

O projeto usa Entity Framework Core Migrations para versionamento do banco:

\`\`\`bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project src/DeviceManagement.Infrastructure --startup-project src/DeviceManagement.Api

# Aplicar migrations
dotnet ef database update --project src/DeviceManagement.Infrastructure --startup-project src/DeviceManagement.Api

# Ver migrations aplicadas
dotnet ef migrations list --project src/DeviceManagement.Infrastructure --startup-project src/DeviceManagement.Api
\`\`\`

### Estrutura do Banco

- **Users**: Usuários do sistema
- **Customers**: Clientes
- **Devices**: Dispositivos IoT
- **Events**: Eventos dos dispositivos

## 📋 Funcionalidades

### Endpoints Principais

#### 🔐 Autenticação
- `POST /api/v1/Auth/login` - Login do usuário

#### 👥 Clientes
- `GET /api/v1/Customers` - Listar clientes
- `GET /api/v1/Customers/{id}` - Obter cliente por ID
- `POST /api/v1/Customers` - Criar cliente
- `PUT /api/v1/Customers/{id}` - Atualizar cliente
- `DELETE /api/v1/Customers/{id}` - Excluir cliente

#### 📱 Dispositivos
- `GET /api/v1/Devices` - Listar dispositivos
- `GET /api/v1/Devices/{id}` - Obter dispositivo por ID
- `GET /api/v1/Devices/customer/{customerId}` - Dispositivos por cliente
- `POST /api/v1/Devices` - Criar dispositivo

#### 📊 Eventos
- `GET /api/v1/Events` - Listar eventos
- `GET /api/v1/Events/device/{deviceId}` - Eventos por dispositivo
- `GET /api/v1/Events/period` - Eventos por período
- `POST /api/v1/Events` - Registrar evento

#### 📈 Dashboard
- `GET /api/v1/Dashboard` - Estatísticas dos últimos 7 dias

## 🔧 Desenvolvimento Local

### Configuração do Ambiente

1. **Inicie apenas o SQL Server**
\`\`\`bash
docker compose up sqlserver -d
\`\`\`

2. **Configure a connection string**
\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DeviceManagementDB_Dev;User Id=sa;Password=DeviceManagement123!;TrustServerCertificate=true;"
  }
}
\`\`\`

3. **Execute as migrations**
\`\`\`bash
cd src/DeviceManagement.Api
dotnet ef database update
\`\`\`

4. **Execute a aplicação**
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
\`\`\`

## 📊 Monitoramento

- **Health Check**: http://localhost:5000/health
- **Logs**: `docker compose logs -f devicemanagement_api`
- **Métricas**: Logs estruturados com informações de performance

## 🔒 Segurança

- ✅ **Autenticação JWT** com tokens seguros
- ✅ **Validação de dados** em todas as entradas
- ✅ **Tratamento de exceções** centralizado
- ✅ **CORS** configurado adequadamente
- ✅ **Senhas hasheadas** com salt

## 📝 Dados de Exemplo

As migrations criam automaticamente:
- 1 usuário administrador
- 1 cliente de exemplo
- 1 dispositivo de exemplo
- 3 eventos de exemplo

## 🐛 Troubleshooting

### Problema: "Invalid email or password"
- ✅ Verifique se as migrations foram aplicadas
- ✅ Use as credenciais: `admin@devicemanagement.com` / `Admin123!`

### Problema: Erro de conexão com banco
- ✅ Verifique se o SQL Server está rodando: `docker compose ps`
- ✅ Aguarde o health check ficar "healthy"

### Problema: Migrations não aplicadas
- ✅ Execute: `docker compose down -v`
- ✅ Execute: `docker compose up --build`

## 📋 Comandos Úteis

### Docker Compose
\`\`\`bash
# Iniciar todos os serviços
docker compose up --build

# Parar e limpar tudo
docker compose down -v

# Ver logs da API
docker compose logs -f devicemanagement_api

# Ver status
docker compose ps
\`\`\`

### Entity Framework
\`\`\`bash
# Criar migration
dotnet ef migrations add NomeDaMigration --project src/DeviceManagement.Infrastructure --startup-project src/DeviceManagement.Api

# Aplicar migrations
dotnet ef database update --project src/DeviceManagement.Infrastructure --startup-project src/DeviceManagement.Api

# Reverter migration
dotnet ef database update PreviousMigrationName --project src/DeviceManagement.Infrastructure --startup-project src/DeviceManagement.Api
\`\`\`

## 🎯 Boas Práticas Implementadas

- ✅ **Clean Architecture** com separação de camadas
- ✅ **SOLID Principles** aplicados
- ✅ **Repository Pattern** para acesso a dados
- ✅ **Unit of Work** para transações
- ✅ **Dependency Injection** nativo do .NET
- ✅ **Migrations** para versionamento do banco
- ✅ **Testes Unitários** com cobertura
- ✅ **Documentação** completa com Swagger
- ✅ **Containerização** com Docker
- ✅ **Logging** estruturado
- ✅ **Health Checks** para monitoramento
