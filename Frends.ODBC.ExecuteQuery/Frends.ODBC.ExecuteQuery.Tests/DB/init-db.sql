ALTER LOGIN sa ENABLE;
GO
ALTER LOGIN sa WITH PASSWORD = 'yourStrong!Password';
GO
CREATE DATABASE UnitTests;
GO
USE UnitTests;
GO
CREATE TABLE AnimalTypes (Id INT, Animal nvarchar(255), PRIMARY KEY(Id));
GO
INSERT INTO AnimalTypes (Id, Animal)
VALUES (1, 'Mammal'), (2, 'Bird');
GO