export function renderBooks(books, booksContainer) {
    booksContainer.innerHTML = "";

    if (books.length === 0) {
        booksContainer.innerHTML = "<p>Книги не найдены</p>";
        return;
    }

    for (const book of books) {
        const bookElement = document.createElement("article");
        bookElement.className = "book-card";

        const authors = book.authors
            .map(author =>
                `${author.lastName} ${author.firstName} ${author.middleName ?? ""}`.trim())
            .join(", ");

        bookElement.innerHTML = `
            <h2>${book.title}</h2>

            <p>
                <strong>Авторы:</strong>
                ${authors}
            </p>

            <p>
                <strong>Год издания:</strong>
                ${book.publicationYear}
            </p>

            <div class="book-actions">
                <button data-action="edit" data-id="${book.id}">
                    Редактировать
                </button>

                <button data-action="delete" data-id="${book.id}">
                    Удалить
                </button>
            </div>
        `;

        booksContainer.appendChild(bookElement);
    }
}