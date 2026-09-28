USE HomeLibrary;
GO

-- ---------------------------------------------------------
-- CreateBook
-- Создание книги и привязка авторов.
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.CreateBook
    @Title NVARCHAR(300),
    @PublicationYear INT,
    @Content XML,
    @AuthorIds dbo.IdList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO dbo.Books (Title, PublicationYear, Content)
        VALUES (@Title, @PublicationYear, @Content);

        DECLARE @BookId INT = CONVERT(INT, SCOPE_IDENTITY());

        INSERT INTO dbo.BookAuthors (BookId, AuthorId)
        SELECT @BookId, Id
        FROM @AuthorIds;

        COMMIT;

        SELECT @BookId AS Id;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        THROW;
    END CATCH
END;
GO


-- ---------------------------------------------------------
-- UpdateBook
-- Обновление книги и замена списка связанных авторов.
-- Возвращает Updated = 0, если книга не существует.
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.UpdateBook
    @Id INT,
    @Title NVARCHAR(300),
    @PublicationYear INT,
    @Content XML,
    @AuthorIds dbo.IdList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.Books
        SET Title = @Title,
            PublicationYear = @PublicationYear,
            Content = @Content
        WHERE Id = @Id;

        IF @@ROWCOUNT = 0
        BEGIN
            ROLLBACK;

            SELECT CAST(0 AS BIT) AS Updated;
            RETURN;
        END;

        DELETE FROM dbo.BookAuthors
        WHERE BookId = @Id;

        INSERT INTO dbo.BookAuthors (BookId, AuthorId)
        SELECT @Id, Id
        FROM @AuthorIds;

        COMMIT;

        SELECT CAST(1 AS BIT) AS Updated;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        THROW;
    END CATCH
END;
GO

-- ---------------------------------------------------------
-- DeleteBook
-- Удаление книги.
-- Связи с авторами удаляются по ON DELETE CASCADE.
-- Возвращает Deleted = 0, если книга не существует.
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.DeleteBook
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Books
    WHERE Id = @Id;

    IF @@ROWCOUNT = 0
    BEGIN
        SELECT CAST(0 AS BIT) AS Deleted;
        RETURN;
    END;

    SELECT CAST(1 AS BIT) AS Deleted;
END;
GO