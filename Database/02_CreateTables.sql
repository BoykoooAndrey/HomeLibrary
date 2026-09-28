USE HomeLibrary;
GO

IF OBJECT_ID(N'dbo.Authors', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Authors
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        MiddleName NVARCHAR(100) NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Books', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Books
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(300) NOT NULL,
        PublicationYear INT NOT NULL,
        Content XML NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.BookAuthors', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BookAuthors
    (
        BookId INT NOT NULL,
        AuthorId INT NOT NULL,

        CONSTRAINT PK_BookAuthors
            PRIMARY KEY (BookId, AuthorId),

        CONSTRAINT FK_BookAuthors_Books
            FOREIGN KEY (BookId)
            REFERENCES dbo.Books(Id)
            ON DELETE CASCADE,

        CONSTRAINT FK_BookAuthors_Authors
            FOREIGN KEY (AuthorId)
            REFERENCES dbo.Authors(Id)
    );
END;
GO