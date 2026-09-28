import {
    getBooks,
    getBook,
    createBook,
    updateBook,
    deleteBook,
    createAuthor
} from "./api.js";

import {
    renderBooks
} from "./books.js";

import {
    searchAuthors,
    getSelectedAuthorIds,
    clearSelectedAuthors,
    setSelectedAuthors,
    addSelectedAuthor
} from "./authors.js";

import {
    clearEditor,
    setEditorContent,
    getEditorContent,
    formatEditor
} from "./editor.js";

const booksContainer = document.getElementById("booksContainer");

const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");

const cardViewButton = document.getElementById("cardViewButton");
const listViewButton = document.getElementById("listViewButton");

const createBookButton = document.getElementById("createBookButton");

const bookDialog = document.getElementById("bookDialog");
const bookForm = document.getElementById("bookForm");
const cancelBookButton = document.getElementById("cancelBookButton");

const bookTitle = document.getElementById("bookTitle");
const bookYear = document.getElementById("bookYear");

const authorSearch = document.getElementById("authorSearch");
const authorResults = document.getElementById("authorResults");
const selectedAuthorsContainer = document.getElementById("selectedAuthors");

const contentEditor = document.getElementById("contentEditor");

let editingBookId = null;
const editorToolbar = document.querySelector(".editor-toolbar");

const createAuthorButton = document.getElementById("createAuthorButton");
const authorDialog = document.getElementById("authorDialog");
const authorForm = document.getElementById("authorForm");
const cancelAuthorButton = document.getElementById("cancelAuthorButton");

const authorFirstName = document.getElementById("authorFirstName");
const authorLastName = document.getElementById("authorLastName");
const authorMiddleName = document.getElementById("authorMiddleName");

const bookDialogTitle = document.getElementById("bookDialogTitle");

async function loadBooks(search = "") {
    try {
        const books = await getBooks(search);
        renderBooks(books, booksContainer);
    }
    catch (error) {
        console.error(error);
        alert(error.message);
    }
}

loadBooks();

searchButton.addEventListener("click", () => {
    loadBooks(searchInput.value);
});

searchInput.addEventListener("keydown", event => {
    if (event.key === "Enter") {
        loadBooks(searchInput.value);
    }
});


cardViewButton.addEventListener("click", () => {
    booksContainer.classList.remove("list");
    booksContainer.classList.add("cards");
});

listViewButton.addEventListener("click", () => {
    booksContainer.classList.remove("cards");
    booksContainer.classList.add("list");
});

cancelBookButton.addEventListener("click", () => {
    bookDialog.close();
});

authorSearch.addEventListener("input", async () => {
    try {
        await searchAuthors(
            authorSearch.value,
            authorResults,
            selectedAuthorsContainer);
    }
    catch (error) {
        console.error(error);
    }
});

bookForm.addEventListener("submit", async event => {
    event.preventDefault();

    try {
        const request = {
            title: bookTitle.value.trim(),
            publicationYear: Number(bookYear.value),
            content: getEditorContent(contentEditor),
            authorIds: getSelectedAuthorIds()
        };

        if (editingBookId === null) {
            await createBook(request);
        }
        else {
            await updateBook(editingBookId, request);
        }

        bookDialog.close();

        bookForm.reset();
        clearEditor(contentEditor);
        clearSelectedAuthors(selectedAuthorsContainer);

        authorResults.innerHTML = "";
        editingBookId = null;

        await loadBooks(searchInput.value);
    }
    catch (error) {
        console.error(error);
        alert(error.message);
    }
});

createBookButton.addEventListener("click", () => {
    editingBookId = null;

    bookDialogTitle.textContent = "Добавление книги";

    bookForm.reset();
    clearEditor(contentEditor);
    clearSelectedAuthors(selectedAuthorsContainer);
    authorResults.innerHTML = "";

    bookDialog.showModal();
});

booksContainer.addEventListener("click", async event => {
    const button = event.target.closest("button[data-action]");

    if (!button) {
        return;
    }

    const bookId = Number(button.dataset.id);
    const action = button.dataset.action;

    if (action === "edit") {
        try {
            const book = await getBook(bookId);

            editingBookId = book.id;

            bookTitle.value = book.title;
            bookYear.value = book.publicationYear;

            setEditorContent(contentEditor, book.content);
            setSelectedAuthors(book.authors, selectedAuthorsContainer);

            authorResults.innerHTML = "";
            authorSearch.value = "";

            bookDialogTitle.textContent = "Редактирование книги";

            bookDialog.showModal();
        }
        catch (error) {
            console.error(error);
            alert(error.message);
        }
    }

    if (action === "delete") {
        const confirmed = confirm("Вы действительно хотите удалить книгу?");

        if (!confirmed) {
            return;
        }

        try {
            await deleteBook(bookId);
            await loadBooks(searchInput.value);
        }
        catch (error) {
            console.error(error);
            alert(error.message);
        }
    }
});

createAuthorButton.addEventListener("click", () => {
    authorForm.reset();
    authorDialog.showModal();
});

cancelAuthorButton.addEventListener("click", () => {
    authorDialog.close();
});

authorForm.addEventListener("submit", async event => {
    event.preventDefault();

    try {
        const request = {
            firstName: authorFirstName.value.trim(),
            lastName: authorLastName.value.trim(),
            middleName: authorMiddleName.value.trim() || null
        };

        const result = await createAuthor(request);

        const author = {
            id: result.id,
            firstName: request.firstName,
            lastName: request.lastName,
            middleName: request.middleName
        };

        addSelectedAuthor(
            author,
            selectedAuthorsContainer
        );

        authorDialog.close();
        authorForm.reset();
        authorSearch.value = "";
        authorResults.innerHTML = "";
    }
    catch (error) {
        console.error(error);
        alert(error.message);
    }
});

cancelBookButton.addEventListener("click", () => {
    bookDialog.close();

    editingBookId = null;

    bookForm.reset();
    clearEditor(contentEditor);
    clearSelectedAuthors(selectedAuthorsContainer);

    authorSearch.value = "";
    authorResults.innerHTML = "";
});

searchInput.addEventListener("input", () => {
    if (!searchInput.value.trim()) {
        loadBooks();
    }
});

if (editorToolbar) {
    editorToolbar.addEventListener("click", event => {
        const button = event.target.closest("button[data-command]");

        if (!button) {
            return;
        }

        formatEditor(button.dataset.command);
        contentEditor.focus();
    });
}

