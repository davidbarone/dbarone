/**
 * Custom event type information for TypeScript
 */
interface CustomEventMap {
    'sessionStorageUpdated': CustomEvent;
}

declare global {
    interface Document {
        addEventListener<K extends keyof CustomEventMap>(type: K,
           listener: (this: Document, ev: CustomEventMap[K]) => void): void;
    }
}

/**
 * Sets session storage, and handles the serialisation.
 * @param key 
 * @param obj 
 */
const setSessionStorage = <T>(key: string, obj: T | null) => {
    let str = '';
    if (obj) {
        str = JSON.stringify(obj);
    }
    sessionStorage.setItem(key, str);

    // Force event to be raised - same page does not receive event normally.
    document.dispatchEvent(new Event('sessionStorageUpdated'));
};

/**
 * Gets session storage, and handles the serialisation.
 * @param key 
 * @returns 
 */
const getSessionStorage = <T>(key: string): T | null => {
    const str = sessionStorage.getItem(key);
    if (str) {
        return JSON.parse(str) as T;
    } else {
        return null;
    }
};

const formToJson = (form: HTMLFormElement): { [k: string]: FormDataEntryValue } => {
    const data = new FormData(form);
    const json = Object.fromEntries(data.entries());
    return json;
};

export {
    formToJson,
    setSessionStorage,
    getSessionStorage
};

