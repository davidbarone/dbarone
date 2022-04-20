const formToJson = (form: HTMLFormElement): { [k: string]: FormDataEntryValue } => {
    const data = new FormData(form);
    const json = Object.fromEntries(data.entries());
    return json;
};

export {
    formToJson
};