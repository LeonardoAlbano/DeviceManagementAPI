IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagementDB')
BEGIN
    CREATE DATABASE DeviceManagementDB;
    PRINT 'Banco DeviceManagementDB criado com sucesso!';
END
ELSE
BEGIN
    PRINT 'Banco DeviceManagementDB já existe.';
END
GO

USE DeviceManagementDB;
GO

PRINT 'Conectado ao banco DeviceManagementDB';