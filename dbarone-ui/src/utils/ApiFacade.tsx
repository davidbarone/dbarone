import { TokenModel } from '../models/TokenModel';
import { ResponseEnvelopeType } from '../types/ResponseEnvelopeType';
import { toast } from '../widgets/ToastWidget';
import { getSessionStorage } from '../utils/Utilities';

type SettingsType = {
    API_DOMAIN: string
}

// Do something with environment settings
const settings = process.env.APP_SETTINGS as unknown as SettingsType;
const getFullUrl = (urlPath: string): string => settings.API_DOMAIN + urlPath;

function handleResponse(response: FlattenedResponse, successMessage: string) {
    if (!response.ok) {
        toastFailure(response.envelope.status.message);
        throw Error(response.envelope.status.message);
    } else {
        toastSuccess(successMessage);
    }
    return response;
}

function toastSuccess(message: string) {
    toast.show(message, {
        timeout: 3000,
        position: 'bottom-right',
        variant: 'success',
    });
}

function toastFailure(message: string) {
    toast.show(message, {
        timeout: 3000,
        position: 'bottom-right',
        variant: 'danger',
    });
}

interface FlattenedResponse {

    /** The Http headers */
    headers: Headers,

    /** Response status */
    ok: boolean,

    /** Optional link header value */
    link: object | null,

    /** Http status code */
    status: number

    /** Http status text */
    statusText: string

    /** body (envelope) */
    envelope: ResponseEnvelopeType
}

/**
 * Link headers from Node-Server are in string, not json or xml format.
 * This function parses the string.
 * @param linkHeader 
 * @returns 
 */
function parseLinkHeader(linkHeader: string): object | null {
    if (!linkHeader) {
        return null;
    }
    const linkHeadersArray = linkHeader.split(', ').map(header => header.split('; '));
    const linkHeadersMap = linkHeadersArray.map(header => {
        const thisHeaderRel = header[1].replace(/"/g, '').replace('rel=', '');
        const thisHeaderUrl = header[0].slice(1, -1);
        return [thisHeaderRel, thisHeaderUrl];
    });
    return Object.fromEntries(linkHeadersMap);
}

async function parseResponse(response: Response): Promise<FlattenedResponse> {
    const body = await response.json();
    const headers = response.headers;
    return {
        ok: response.ok,
        status: response.status,
        statusText: response.statusText,
        headers: headers,
        link: parseLinkHeader(headers.get('link') || ''),
        envelope: body
    };
}

/**
 * Wrapper for fetch
 * @param url 
 * @param options 
 * @returns 
 */
async function fetchWrapper(url: string, options: object): Promise<FlattenedResponse> {
    return await new Promise((resolve, reject) => {
        fetch(url, options)
            .then(response => {
                return parseResponse(response);
            })
            .then((flattenedResponse) => resolve(flattenedResponse))
            .catch(error => {
                reject(error);
            });
    });
}

/**
 * Generic Http GET
 * @param url 
 * @returns 
 */
async function httpGet(url: string, successMessage: string): Promise<FlattenedResponse> {
    const user = getSessionStorage<TokenModel>('user');
    const headers: any = {};
    headers['Content-Type'] = 'application/json';
    if (user) {
        headers['authorization'] = `Bearer ${user.jwtToken}`;
    }
    url = getFullUrl(url);
    return fetchWrapper(url, {
        mode: 'cors',
        credentials: 'include', // for cookies
        withCredentials: true,
        headers: headers,
    })
        .then((response) => handleResponse(response, successMessage))
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
}

/**
 * Generic Http DELETE
 * @param url 
 * @returns 
 */
async function httpDelete(url: string, successMessage: string): Promise<FlattenedResponse> {
    url = getFullUrl(url);

    const user = getSessionStorage<TokenModel>('user');
    const headers: any = {};
    if (user) {
        headers['authorization'] = `Bearer ${user.jwtToken}`;
    }

    return fetchWrapper(url, {
        method: 'DELETE',
        credentials: 'include',  // for cookies
        withCredentials: true,
        mode: 'cors',
        headers: headers,
    })
        .then((response) => handleResponse(response, successMessage))
        .then((response) => response)
        .catch((error) => {
            toastFailure(error);
            throw error;
        });
}

/**
 * Generic Http POST
 * @param url 
 * @returns 
 */
async function httpPost(url: string, body: object, successMessage: string, isBodyRaw = false): Promise<FlattenedResponse> {
    const user = getSessionStorage<TokenModel>('user');
    const headers: any = {};
    if (!isBodyRaw) {
        headers['Content-Type'] = 'application/json';
    }
    if (user) {
        headers['authorization'] = `Bearer ${user.jwtToken}`;
    }

    url = getFullUrl(url);
    return fetchWrapper(url, {
        method: 'POST',
        credentials: 'include', // for cookies
        withCredentials: true,
        mode: 'cors',
        headers: headers,
        body: isBodyRaw ? body : JSON.stringify(body)       // For file uploads we don't stringify.
    })
        .then((response) => handleResponse(response, successMessage))
        .then((response) => response)
        .catch((error) => {
            alert(error);
            toastFailure(error);
            throw error;
        });
}

/**
 * Generic Http PUT
 * @param url 
 * @returns 
 */
async function httpPut(url: string, body: object, successMessage: string): Promise<FlattenedResponse> {
    const user = getSessionStorage<TokenModel>('user');
    url = getFullUrl(url);
    const headers: any = {};
    headers['Content-Type'] = 'application/json';
    if (user) {
        headers['authorization'] = `Bearer ${user.jwtToken}`;
    }

    return fetchWrapper(url, {
        method: 'PUT',
        credentials: 'include', // for cookies
        withCredentials: true,
        mode: 'cors',
        headers: headers,
        body: JSON.stringify(body)
    })
        .then((response) => handleResponse(response, successMessage))
        .then((response) => response)
        .catch((error) => {
            toastFailure(error);
            throw error;
        });
}

export {
    httpGet,
    httpDelete,
    httpPost,
    httpPut
};
