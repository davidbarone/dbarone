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

/**
 * Converts a form to an object.
 * @param form 
 * @returns 
 */
const formToObject = (form: HTMLFormElement): { [k: string]: unknown } => {
    const data = new FormData(form);
    const json: {[k: string]: unknown} = Object.fromEntries(data.entries());
    for (const k in json) {
        if (!json[k]) {
            json[k] = null;
        }
    }
    return json;
};

export {
    formToObject,
    setSessionStorage,
    getSessionStorage
};

