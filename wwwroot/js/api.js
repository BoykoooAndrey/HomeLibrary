async function getError(response) {
    try {
        const problem = await response.json();

        if (problem.errors) {
            const messages = Object.values(problem.errors).flat();

            return new Error(messages.join("\n"));
        }

        if (problem.detail) {
            return new Error(problem.detail);
        }

        if (problem.title) {
            return new Error(problem.title);
        }

        return new Error(`Ошибка выполнения запроса: ${response.status}`);
    }
    catch {
        return new Error(`Ошибка выполнения запроса: ${response.status}`);
    }
}


export async function getBooks(search = "") {
    const url = search ? `/api/Books?search=${encodeURIComponent(search)}` : "/api/Books";

    const response = await fetch(url);

    if (!response.ok) {
        throw await getError(response);
    }

    return await response.json();
}

export async function getBook(id) {
    const response = await fetch(`/api/Books/${id}`);

    if (!response.ok) {
        throw await getError(response);
    }

    return await response.json();
}

export async function updateBook(id, request) {
    const response = await fetch(`/api/Books/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(request)
    });

    if (!response.ok) {
        throw await getError(response);
    }
}

export async function deleteBook(id) {
    const response = await fetch(`/api/Books/${id}`, {
        method: "DELETE"
    });

    if (!response.ok) {
        throw await getError(response);
    }
}

export async function createBook(request) {
    const response = await fetch("/api/Books", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(request)
    });

    if (!response.ok) {
        throw await getError(response);
    }

    return await response.json();
}

export async function getAuthors(search = "") {
    const url = search
        ? `/api/Authors?search=${encodeURIComponent(search)}`
        : "/api/Authors";

    const response = await fetch(url);

    if (!response.ok) {
        throw await getError(response);
    }

    return await response.json();
}

export async function createAuthor(request) {
    const response = await fetch("/api/Authors", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(request)
    });

    if (!response.ok) {
        throw await getError(response);
    }

    return await response.json();
}