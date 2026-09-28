USE master;
GO

IF DB_ID(N'HomeLibrary') IS NULL
BEGIN
    CREATE DATABASE HomeLibrary;
END;
GO