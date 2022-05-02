import { LinkType } from './LinkType';
import { ResponseMessageType } from './ResponseMessageType';
import { ResponseStatusType } from './ResponseStatusType';

export type ResponseEnvelopeType = {
    status: ResponseStatusType,
    data: any,
    links: Array<LinkType>,
    messages: Array<ResponseMessageType>
}