USE DeviceManagementDB;
GO

PRINT 'Inserindo dados iniciais...';

IF NOT EXISTS (SELECT * FROM Usuarios WHERE Email = 'admin@devicemanagement.com')
BEGIN
    INSERT INTO Usuarios (Id, Nome, Email, SenhaHash, Salt, Perfil, Ativo) VALUES
    (NEWID(), 'Administrador', 'admin@devicemanagement.com', 
     'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 
     'V8F4DjY9ZkYvTlz9BQzbeA==', 
     'Admin', 1);
    PRINT 'Usuário administrador criado!';
END
ELSE
BEGIN
    PRINT 'Usuário administrador já existe.';
END
GO

IF NOT EXISTS (SELECT * FROM Clientes)
BEGIN
    INSERT INTO Clientes (Id, Nome, Email, Telefone, Status) VALUES
    (NEWID(), 'João Silva', 'joao.silva@email.com', '11999999999', 1),
    (NEWID(), 'Maria Santos', 'maria.santos@email.com', '11888888888', 1),
    (NEWID(), 'Pedro Oliveira', 'pedro.oliveira@email.com', '11777777777', 1);
    PRINT 'Clientes de exemplo criados!';
END
ELSE
BEGIN
    PRINT 'Clientes já existem.';
END
GO

DECLARE @ClienteId1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Email = 'joao.silva@email.com');
DECLARE @ClienteId2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Clientes WHERE Email = 'maria.santos@email.com');

IF NOT EXISTS (SELECT * FROM Dispositivos) AND @ClienteId1 IS NOT NULL AND @ClienteId2 IS NOT NULL
BEGIN
    INSERT INTO Dispositivos (Id, Serial, IMEI, DataAtivacao, ClienteId) VALUES
    (NEWID(), 'DEV001', '123456789012345', GETUTCDATE(), @ClienteId1),
    (NEWID(), 'DEV002', '123456789012346', GETUTCDATE(), @ClienteId2),
    (NEWID(), 'DEV003', '123456789012347', NULL, @ClienteId1);
    PRINT 'Dispositivos de exemplo criados!';
END
ELSE
BEGIN
    PRINT 'Dispositivos já existem ou clientes não encontrados.';
END
GO

DECLARE @DispositivoId1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Dispositivos WHERE Serial = 'DEV001');
DECLARE @DispositivoId2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Dispositivos WHERE Serial = 'DEV002');

IF NOT EXISTS (SELECT * FROM Eventos) AND @DispositivoId1 IS NOT NULL AND @DispositivoId2 IS NOT NULL
BEGIN
    INSERT INTO Eventos (Id, Tipo, Observacoes, DataHora, DispositivoId) VALUES
    (NEWID(), 0, 'Dispositivo ligado', DATEADD(HOUR, -2, GETUTCDATE()), @DispositivoId1),
    (NEWID(), 2, 'Movimento detectado', DATEADD(HOUR, -1, GETUTCDATE()), @DispositivoId1),
    (NEWID(), 0, 'Dispositivo ligado', DATEADD(HOUR, -3, GETUTCDATE()), @DispositivoId2),
    (NEWID(), 3, 'Queda de sinal', DATEADD(MINUTE, -30, GETUTCDATE()), @DispositivoId2);
    PRINT 'Eventos de exemplo criados!';
END
ELSE
BEGIN
    PRINT 'Eventos já existem ou dispositivos não encontrados.';
END
GO

PRINT 'Dados iniciais inseridos com sucesso!';

SELECT 'Usuários' as Tabela, COUNT(*) as Total FROM Usuarios
UNION ALL
SELECT 'Clientes', COUNT(*) FROM Clientes
UNION ALL
SELECT 'Dispositivos', COUNT(*) FROM Dispositivos
UNION ALL
SELECT 'Eventos', COUNT(*) FROM Eventos;