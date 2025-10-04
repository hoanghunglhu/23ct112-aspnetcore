-- Create a new database called 'LearnAspNetCore'
-- Connect to the 'master' database to run this snippet
USE master
GO
-- Create the new database if it does not exist already
IF NOT EXISTS (
    SELECT [name]
        FROM sys.databases
        WHERE [name] = N'LearnAspNetCore'
)
CREATE DATABASE LearnAspNetCore
GO
--USE LearnAspNetCore

-- Create a new table called '[users]' in schema '[dbo]'
-- Drop the table if it already exists
IF OBJECT_ID('[dbo].[users]', 'U') IS NOT NULL
DROP TABLE [dbo].[users]
GO
-- Create the table in the specified schema
CREATE TABLE [dbo].[users]
(
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- Primary Key column
    [name] NVARCHAR(255) NOT NULL,
    [gender] BIT DEFAULT 1 NOT NULL,
    [birthday] DATE,
    [email] VARCHAR(255),
    [phone] VARCHAR(20),
    [address] NVARCHAR(500)
);
GO

-- Create a new table called '[user_tokens]' in schema '[dbo]'
-- Drop the table if it already exists
IF OBJECT_ID('[dbo].[user_tokens]', 'U') IS NOT NULL
DROP TABLE [dbo].[user_tokens]
GO
-- Create the table in the specified schema
CREATE TABLE [dbo].[user_tokens]
(
    [Id] INT NOT NULL PRIMARY KEY, -- Primary Key column
    [user_id] INT NOT NULL,
    [token] VARCHAR(255) NOT NULL
    CONSTRAINT FK_USER_TOKEN_USER_ID FOREIGN KEY ([user_id]) REFERENCES users([id])
);
GO