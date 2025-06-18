echo "🚀 Iniciando DeviceManagement API com .NET 9.0..."
echo "=================================================="

if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Por favor, inicie o Docker primeiro."
    exit 1
fi

echo "🛑 Parando containers existentes..."
docker-compose down

echo "🧹 Limpando imagens antigas..."
docker image prune -f

echo "🐳 Iniciando SQL Server..."
docker-compose up -d sqlserver

echo "⏳ Aguardando SQL Server inicializar..."
for i in {1..15}; do
    if docker exec sqlserver-devicemanagement /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "DeviceManagement123!" -C -Q "SELECT 1" > /dev/null 2>&1; then
        echo "✅ SQL Server está pronto!"
        break
    fi
    echo "   Tentativa $i/15 - aguardando..."
    sleep 3
done

echo "🗃️ Criando banco e tabelas..."
echo "   📄 Executando 01-create-database.sql..."
docker exec sqlserver-devicemanagement /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "DeviceManagement123!" -C -i /scripts/01-create-database.sql

echo "   📄 Executando 02-create-tables.sql..."
docker exec sqlserver-devicemanagement /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "DeviceManagement123!" -C -i /scripts/02-create-tables.sql

echo "   📄 Executando 03-seed-data.sql..."
docker exec sqlserver-devicemanagement /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "DeviceManagement123!" -C -i /scripts/03-seed-data.sql

echo "🏗️ Construindo e iniciando API com .NET 9.0..."
docker-compose up --build -d devicemanagement.api

echo "⏳ Aguardando API inicializar..."
for i in {1..10}; do
    if curl -f http://localhost:5000/health > /dev/null 2>&1; then
        echo "✅ API está pronta!"
        break
    fi
    echo "   Tentativa $i/10 - aguardando API..."
    sleep 3
done

echo "📊 Verificando status dos serviços..."
docker-compose ps

echo ""
echo "🎉 TUDO PRONTO COM .NET 9.0!"
echo "=================================================="
echo "🌐 API disponível em: http://localhost:5000"
echo "📚 Swagger disponível em: http://localhost:5000"
echo "🏥 Health Check: http://localhost:5000/health"
echo "🗄️ SQL Server disponível em: localhost:1433"
echo ""
echo "👤 Credenciais de teste:"
echo "   Email: admin@devicemanagement.com"
echo "   Senha: admin123"
echo "=================================================="