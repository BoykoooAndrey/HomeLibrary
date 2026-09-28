export function clearEditor(editor) {
    editor.innerHTML = "";
}

export function setEditorContent(editor, xml) {
    editor.innerHTML = "";

    if (!xml) {
        return;
    }

    const parser = new DOMParser();
    const xmlDocument = parser.parseFromString(xml, "application/xml");

    if (xmlDocument.querySelector("parsererror")) {
        throw new Error("Не удалось прочитать XML оглавления");
    }

    const content = xmlDocument.documentElement;

    if (content.nodeName !== "content") {
        throw new Error("Корневой элемент оглавления должен быть content");
    }

    const serializer = new XMLSerializer();

    editor.innerHTML = Array.from(content.childNodes)
        .map(node => serializer.serializeToString(node))
        .join("");
}

export function getEditorContent(editor) {
    const xmlDocument = document.implementation.createDocument(
        null,
        "content"
    );

    const content = xmlDocument.documentElement;

    for (const node of editor.childNodes) {
        content.appendChild(
            xmlDocument.importNode(node, true)
        );
    }

    return new XMLSerializer().serializeToString(xmlDocument);
}

export function formatEditor(command) {
    document.execCommand(command, false, null);
}