USE DeviceManagementDB;
GO

PRINT 'Starting table creation...';

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
CREATE TABLE Users (
                       Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                       Name NVARCHAR(255) NOT NULL,
                       Email NVARCHAR(255) NOT NULL UNIQUE,
                       PasswordHash NVARCHAR(500) NOT NULL,
                       Salt NVARCHAR(500) NOT NULL,
                       Role NVARCHAR(50) NOT NULL DEFAULT 'User',
                       IsActive BIT NOT NULL DEFAULT 1,
                       CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                       UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
PRINT 'Users table created!';
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Customers' AND xtype='U')
BEGIN
CREATE TABLE Customers (
                           Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                           Name NVARCHAR(255) NOT NULL,
                           Email NVARCHAR(255) NOT NULL UNIQUE,
                           Phone NVARCHAR(20) NULL,
                           Status BIT NOT NULL DEFAULT 1,
                           CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                           UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
PRINT 'Customers table created!';
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Devices' AND xtype='U')
BEGIN
CREATE TABLE Devices (
                         Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                         Serial NVARCHAR(100) NOT NULL UNIQUE,
                         IMEI NVARCHAR(15) NOT NULL UNIQUE,
                         ActivationDate DATETIME2 NULL,
                         CustomerId UNIQUEIDENTIFIER NOT NULL,
                         CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                         UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                         CONSTRAINT FK_Devices_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
PRINT 'Devices table created!';
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Events' AND xtype='U')
BEGIN
CREATE TABLE Events (
                        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                        Type INT NOT NULL,
                        Observations NVARCHAR(500) NULL,
                        EventDateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                        DeviceId UNIQUEIDENTIFIER NOT NULL,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                        CONSTRAINT FK_Events_Devices FOREIGN KEY (DeviceId) REFERENCES Devices(Id)
);
PRINT 'Events table created!';
END

PRINT 'Creating indexes for performance...';

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Email')
    CREATE NONCLUSTERED INDEX IX_Users_Email ON Users(Email);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Customers_Email')
    CREATE NONCLUSTERED INDEX IX_Customers_Email ON Customers(Email);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Devices_Serial')
    CREATE NONCLUSTERED INDEX IX_Devices_Serial ON Devices(Serial);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Devices_IMEI')
    CREATE NONCLUSTERED INDEX IX_Devices_IMEI ON Devices(IMEI);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Events_EventDateTime')
    CREATE NONCLUSTERED INDEX IX_Events_EventDateTime ON Events(EventDateTime);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Events_Type')
    CREATE NONCLUSTERED INDEX IX_Events_Type ON Events(Type);

PRINT 'All tables and indexes created successfully!';
