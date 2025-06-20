IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagementDB')
BEGIN
    CREATE DATABASE DeviceManagementDB;
    PRINT 'Database DeviceManagementDB created successfully!';
END
ELSE
BEGIN
    PRINT 'Database DeviceManagementDB already exists.';
END
GO

USE DeviceManagementDB;
GO

PRINT 'Connected to DeviceManagementDB database';
