USE DeviceManagementDB;
GO

PRINT 'Iniciando criação das tabelas...';

-- Tabela de Usuários
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuarios' AND xtype='U')
BEGIN
    CREATE TABLE Usuarios (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Nome NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        SenhaHash NVARCHAR(500) NOT NULL,
        Salt NVARCHAR(500) NOT NULL,
        Perfil NVARCHAR(50) NOT NULL DEFAULT 'User',
        Ativo BIT NOT NULL DEFAULT 1,
        DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        DataAtualizacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    PRINT 'Tabela Usuarios criada!';
END
ELSE
BEGIN
    PRINT 'Tabela Usuarios já existe.';
END
GO

-- Tabela de Clientes
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Clientes' AND xtype='U')
BEGIN
    CREATE TABLE Clientes (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Nome NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        Telefone NVARCHAR(20) NULL,
        Status BIT NOT NULL DEFAULT 1,
        DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        DataAtualizacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    PRINT 'Tabela Clientes criada!';
END
ELSE
BEGIN
    PRINT 'Tabela Clientes já existe.';
END
GO

-- Tabela de Dispositivos
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Dispositivos' AND xtype='U')
BEGIN
    CREATE TABLE Dispositivos (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Serial NVARCHAR(100) NOT NULL UNIQUE,
        IMEI NVARCHAR(15) NOT NULL UNIQUE,
        DataAtivacao DATETIME2 NULL,
        ClienteId UNIQUEIDENTIFIER NOT NULL,
        DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        DataAtualizacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Dispositivos_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
    );
    PRINT 'Tabela Dispositivos criada!';
END
ELSE
BEGIN
    PRINT 'Tabela Dispositivos já existe.';
END
GO

-- Tabela de Eventos
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Eventos' AND xtype='U')
BEGIN
    CREATE TABLE Eventos (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Tipo INT NOT NULL,
        Observacoes NVARCHAR(500) NULL,
        DataHora DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        DispositivoId UNIQUEIDENTIFIER NOT NULL,
        DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Eventos_Dispositivos FOREIGN KEY (DispositivoId) REFERENCES Dispositivos(Id)
    );
    PRINT 'Tabela Eventos criada!';
END
ELSE
BEGIN
    PRINT 'Tabela Eventos já existe.';
END
GO

-- Criar índices para performance
PRINT 'Criando índices...';

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Usuarios_Email')
    CREATE NONCLUSTERED INDEX IX_Usuarios_Email ON Usuarios(Email);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Clientes_Email')
    CREATE NONCLUSTERED INDEX IX_Clientes_Email ON Clientes(Email);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Dispositivos_Serial')
    CREATE NONCLUSTERED INDEX IX_Dispositivos_Serial ON Dispositivos(Serial);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Dispositivos_IMEI')
    CREATE NONCLUSTERED INDEX IX_Dispositivos_IMEI ON Dispositivos(IMEI);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Dispositivos_ClienteId')
    CREATE NONCLUSTERED INDEX IX_Dispositivos_ClienteId ON Dispositivos(ClienteId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Eventos_DataHora')
    CREATE NONCLUSTERED INDEX IX_Eventos_DataHora ON Eventos(DataHora);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Eventos_DispositivoId')
    CREATE NONCLUSTERED INDEX IX_Eventos_DispositivoId ON Eventos(DispositivoId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Eventos_Tipo')
    CREATE NONCLUSTERED INDEX IX_Eventos_Tipo ON Eventos(Tipo);

PRINT 'Todas as tabelas e índices foram criados com sucesso!';