import { toast } from '../widgets/ToastWidget';

type SettingsType = {
    API_DOMAIN: string
}

// Do something with environment settings
const settings = process.env.APP_SETTINGS as unknown as SettingsType;
const getFullUrl = (urlPath: string): string => settings.API_DOMAIN + urlPath;

function handleResponse(response: FlattenedResponse, successMessage: string) {
    if (!response.ok) {
        throw Error(response.statusText);
    }
    toastSuccess(successMessage);
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
    headers: Headers,
    body: any,
    ok: boolean,
    link: object | null,
    status: number
    statusText: string
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
    const linkHeadersMap = linkHeadersArray.map( header => {
        const thisHeaderRel = header[1].replace( /"/g, '' ).replace( 'rel=', '' );
        const thisHeaderUrl = header[0].slice( 1, -1 );
        return [ thisHeaderRel, thisHeaderUrl ];
    } );
    return Object.fromEntries(linkHeadersMap);
}

async function parseResponse(response: Response): Promise<FlattenedResponse> {
    const body = await response.json();
    const headers = response.headers;
    return {
        ok: response.ok,
        body: body,
        link: parseLinkHeader(headers.get('link') || ''),
        headers: headers,
        statusText: response.statusText,
        status: response.status
    };
}

/**
 * Wrapper for fetch
 * @param url 
 * @param options 
 * @returns 
 */
async function fetchWrapper(url: string, options: object): Promise<FlattenedResponse> { 
    return await new Promise((resolve, reject)=>{
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
    url = getFullUrl(url);
    return fetchWrapper(url, {
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then((response) => handleResponse(response, successMessage))
        .then((response) => response)
        .catch((error) => {
            toastFailure(error);
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
    return fetchWrapper(url, {
        method: 'DELETE',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
        },
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
async function httpPost(url: string, body: object, successMessage: string): Promise<FlattenedResponse> {
    url = getFullUrl(url);
    return fetchWrapper(url, {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body)
    })
        .then((response) => handleResponse(response, successMessage))
        .then((response) => response)
        .catch((error) => {
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
    url = getFullUrl(url);
    return fetchWrapper(url, {
        method: 'PUT',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
        },
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
