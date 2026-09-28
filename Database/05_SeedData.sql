USE HomeLibrary;
GO

/* Авторы */

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Authors
    WHERE FirstName = N'Илья'
      AND LastName = N'Ильф'
)
BEGIN
    INSERT INTO dbo.Authors (FirstName, LastName, MiddleName)
    VALUES (N'Илья', N'Ильф', NULL);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Authors
    WHERE FirstName = N'Евгений'
      AND LastName = N'Петров'
)
BEGIN
    INSERT INTO dbo.Authors (FirstName, LastName, MiddleName)
    VALUES (N'Евгений', N'Петров', NULL);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Authors
    WHERE FirstName = N'Лев'
      AND LastName = N'Толстой'
      AND MiddleName = N'Николаевич'
)
BEGIN
    INSERT INTO dbo.Authors (FirstName, LastName, MiddleName)
    VALUES (N'Лев', N'Толстой', N'Николаевич');
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Authors
    WHERE FirstName = N'Аркадий'
      AND LastName = N'Стругацкий'
      AND MiddleName = N'Натанович'
)
BEGIN
    INSERT INTO dbo.Authors (FirstName, LastName, MiddleName)
    VALUES (N'Аркадий', N'Стругацкий', N'Натанович');
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Authors
    WHERE FirstName = N'Борис'
      AND LastName = N'Стругацкий'
      AND MiddleName = N'Натанович'
)
BEGIN
    INSERT INTO dbo.Authors (FirstName, LastName, MiddleName)
    VALUES (N'Борис', N'Стругацкий', N'Натанович');
END;

GO


/* Книги */

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Books
    WHERE Title = N'Двенадцать стульев'
)
BEGIN
    DECLARE @BookId1 INT;

    INSERT INTO dbo.Books (Title, PublicationYear, Content)
    VALUES
    (
        N'Двенадцать стульев',
        1928,
        N'<content>
            <div>Часть первая</div>
            <div>Часть вторая</div>
            <div>Часть третья</div>
        </content>'
    );

    SET @BookId1 = SCOPE_IDENTITY();

    INSERT INTO dbo.BookAuthors (BookId, AuthorId)
    SELECT @BookId1, Id
    FROM dbo.Authors
    WHERE (FirstName = N'Илья' AND LastName = N'Ильф')
       OR (FirstName = N'Евгений' AND LastName = N'Петров');
END;

GO


IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Books
    WHERE Title = N'Война и мир'
)
BEGIN
    DECLARE @BookId2 INT;

    INSERT INTO dbo.Books (Title, PublicationYear, Content)
    VALUES
    (
        N'Война и мир',
        1869,
        N'<content>
            <div>Том первый</div>
            <div>Том второй</div>
            <div>Том третий</div>
            <div>Том четвертый</div>
        </content>'
    );

    SET @BookId2 = SCOPE_IDENTITY();

    INSERT INTO dbo.BookAuthors (BookId, AuthorId)
    SELECT @BookId2, Id
    FROM dbo.Authors
    WHERE FirstName = N'Лев'
      AND LastName = N'Толстой'
      AND MiddleName = N'Николаевич';
END;

GO


IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Books
    WHERE Title = N'Пикник на обочине'
)
BEGIN
    DECLARE @BookId3 INT;

    INSERT INTO dbo.Books (Title, PublicationYear, Content)
    VALUES
    (
        N'Пикник на обочине',
        1972,
        N'<content>
            <div>Предисловие</div>
            <div>Рэдрик Шухарт, 23 года, холост, лаборант филиала</div>
            <div>Рэдрик Шухарт, 28 лет, женат, без определенных занятий</div>
        </content>'
    );

    SET @BookId3 = SCOPE_IDENTITY();

    INSERT INTO dbo.BookAuthors (BookId, AuthorId)
    SELECT @BookId3, Id
    FROM dbo.Authors
    WHERE (FirstName = N'Аркадий' AND LastName = N'Стругацкий')
       OR (FirstName = N'Борис' AND LastName = N'Стругацкий');
END;

GO