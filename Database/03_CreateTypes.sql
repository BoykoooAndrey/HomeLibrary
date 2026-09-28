USE HomeLibrary;
GO

IF TYPE_ID(N'dbo.IdList') IS NULL
BEGIN
    CREATE TYPE dbo.IdList AS TABLE
    (
        Id INT NOT NULL PRIMARY KEY
    );
END;
GO