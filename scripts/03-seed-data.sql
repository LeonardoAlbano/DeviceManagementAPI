USE DeviceManagementDB;
GO

PRINT 'Inserting initial data...';

IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'admin@devicemanagement.com')
BEGIN
INSERT INTO Users (Id, Name, Email, PasswordHash, Salt, Role, IsActive) VALUES
    (NEWID(), 'Administrator', 'admin@devicemanagement.com',
     'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=',
     'V8F4DjY9ZkYvTlz9BQzbeA==',
     'Admin', 1);
PRINT 'Admin user created!';
END

IF NOT EXISTS (SELECT * FROM Customers)
BEGIN
INSERT INTO Customers (Id, Name, Email, Phone, Status) VALUES
                                                           (NEWID(), 'John Silva', 'john.silva@email.com', '11999999999', 1),
                                                           (NEWID(), 'Maria Santos', 'maria.santos@email.com', '11888888888', 1),
                                                           (NEWID(), 'Pedro Oliveira', 'pedro.oliveira@email.com', '11777777777', 1);
PRINT 'Sample customers created!';
END

DECLARE @CustomerId1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Customers WHERE Email = 'john.silva@email.com');
DECLARE @CustomerId2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Customers WHERE Email = 'maria.santos@email.com');

IF NOT EXISTS (SELECT * FROM Devices) AND @CustomerId1 IS NOT NULL AND @CustomerId2 IS NOT NULL
BEGIN
INSERT INTO Devices (Id, Serial, IMEI, ActivationDate, CustomerId) VALUES
                                                                       (NEWID(), 'DEV001', '123456789012345', GETUTCDATE(), @CustomerId1),
                                                                       (NEWID(), 'DEV002', '123456789012346', GETUTCDATE(), @CustomerId2),
                                                                       (NEWID(), 'DEV003', '123456789012347', NULL, @CustomerId1);
PRINT 'Sample devices created!';
END

DECLARE @DeviceId1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Devices WHERE Serial = 'DEV001');
DECLARE @DeviceId2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Devices WHERE Serial = 'DEV002');

IF NOT EXISTS (SELECT * FROM Events) AND @DeviceId1 IS NOT NULL AND @DeviceId2 IS NOT NULL
BEGIN
INSERT INTO Events (Id, Type, Observations, EventDateTime, DeviceId) VALUES
                                                                         (NEWID(), 0, 'Device turned on', DATEADD(HOUR, -2, GETUTCDATE()), @DeviceId1),
                                                                         (NEWID(), 2, 'Movement detected', DATEADD(HOUR, -1, GETUTCDATE()), @DeviceId1),
                                                                         (NEWID(), 0, 'Device turned on', DATEADD(HOUR, -3, GETUTCDATE()), @DeviceId2),
                                                                         (NEWID(), 3, 'Signal loss', DATEADD(MINUTE, -30, GETUTCDATE()), @DeviceId2);
PRINT 'Sample events created!';
END

PRINT 'Initial data inserted successfully!';
