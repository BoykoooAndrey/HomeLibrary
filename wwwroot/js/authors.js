import { getAuthors } from "./api.js";

let selectedAuthors = [];

export function setSelectedAuthors(authors, selectedAuthorsContainer) {

    selectedAuthors = [...authors];

    renderSelectedAuthors(selectedAuthorsContainer);
}

export function addSelectedAuthor(author, selectedAuthorsContainer) {
    if (selectedAuthors.some(x => x.id === author.id)) {
        return;
    }

    selectedAuthors.push(author);
    renderSelectedAuthors(selectedAuthorsContainer);
}

export function getSelectedAuthorIds() {
    return selectedAuthors.map(author => author.id);
}

export function clearSelectedAuthors(selectedAuthorsContainer) {
    selectedAuthors = [];
    renderSelectedAuthors(selectedAuthorsContainer);
}

export async function searchAuthors(
    search,
    authorResults,
    selectedAuthorsContainer) {

    if (!search.trim()) {
        authorResults.innerHTML = "";
        return;
    }

    const authors = await getAuthors(search.trim());

    renderAuthorResults(
        authors,
        authorResults,
        selectedAuthorsContainer);
}

function renderAuthorResults(
    authors,
    authorResults,
    selectedAuthorsContainer) {

    authorResults.innerHTML = "";

    for (const author of authors) {
        if (selectedAuthors.some(x => x.id === author.id)) {
            continue;
        }

        const button = document.createElement("button");
        button.type = "button";

        button.textContent =
            `${author.lastName} ${author.firstName} ${author.middleName ?? ""}`.trim();

        button.addEventListener("click", () => {
            selectedAuthors.push(author);

            renderSelectedAuthors(selectedAuthorsContainer);

            authorResults.innerHTML = "";
        });

        authorResults.appendChild(button);
    }
}

function renderSelectedAuthors(container) {
    container.innerHTML = "";

    for (const author of selectedAuthors) {
        const element = document.createElement("span");
        element.className = "selected-author";

        const name =
            `${author.lastName} ${author.firstName} ${author.middleName ?? ""}`.trim();

        element.innerHTML = `
            ${name}
            <button type="button">×</button>
        `;

        element.querySelector("button").addEventListener("click", () => {
            selectedAuthors =
                selectedAuthors.filter(x => x.id !== author.id);

            renderSelectedAuthors(container);
        });

        container.appendChild(element);
    }
}