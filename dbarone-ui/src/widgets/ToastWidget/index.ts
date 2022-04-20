/* 
https://github.com/ashr81/react-tiny-toast
https://www.npmjs.com/package/react-tiny-toast
*/

import toast from './toast';
import ToastContainer from './MyToastContainer';

export const POSITIONS = {
    TOP_CENTER: 'top-center',
    TOP_LEFT: 'top-left',
    TOP_RIGHT: 'top-right',
    BOTTOM_LEFT: 'bottom-left',
    BOTTOM_RIGHT: 'bottom-right',
    BOTTOM_CENTER: 'bottom-center'
} as const;

export const VARIANTS = {
    SUCCESS: 'success',
    DANGER: 'danger',
    WARNING: 'warning',
    DEFAULT: 'default'
} as const;

export const ACTIONS = {
    ADD: 'ADD',
    REMOVE: 'REMOVE'
} as const;

export { toast, ToastContainer };