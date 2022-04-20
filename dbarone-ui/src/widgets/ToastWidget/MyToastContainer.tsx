import React, { useEffect, useReducer, useRef, FunctionComponent } from 'react';
import { createPortal } from 'react-dom';
import { toastManager } from './toast';
import style from './style.css';
import { actionTypes, optionTypes, contentTypes } from './myToastTypes';

const ADD = 'ADD';
const REMOVE = 'REMOVE';
interface stateTypes extends optionTypes {
  content?: contentTypes;
}

interface MapperValuesInterface extends optionTypes{
  content: contentTypes
}
interface ActionsInterface {
  type: actionTypes;
  data: stateTypes;
}

const reducer = (state: stateTypes[], action: ActionsInterface) => {
    const { type, data } = action;
    if(type === ADD) {
        if(state.filter(i => i.uniqueCode && i.uniqueCode === data.uniqueCode).length) {
            return state;
        }
        return [...state, data];
    } else if(type === REMOVE) {
        return state.filter(i => i.id !== data.id);
    }
    return state;
};

const ToastContainer: FunctionComponent = () => {
    const toastRootElementId = 'myToastMainContainer';
    const [data, dispatch] = useReducer(reducer, []);
    const toastRef = useRef<HTMLDivElement | null>(null);
    toastRef.current;

    const callback = (actionType: actionTypes, content: contentTypes, options: optionTypes) => {
        if (actionType === ADD) {
            dispatch({ type: ADD, data: { content, ...options, key: `${options.id}` } });
        }
        if (options.pause && actionType === REMOVE) {
            dispatch({ type: REMOVE, data: { id: options.id } });
        } else if (!options.pause) {
            window.setTimeout(() => {
                dispatch({ type: REMOVE, data: { id: options.id } });
            }, options.timeout);
        }
    };

    useEffect(() => {
        toastManager.subscribe(callback);
    }, []);

    useEffect((): any => {
        const node = document.createElement('div');
        node.setAttribute('id', toastRootElementId);
        node.className = style.myToastMainContainer;
        document.body.appendChild(node);
        toastRef.current = node;
        return () => document.body.removeChild(node);
    }, []);

    const positionMaintainer = (): any => {
        const mapper: any = {};
        data.map(({ position, ...rest }: optionTypes) => {
            if(position) {
                if(!mapper[position]) mapper[position] = [];
                mapper[position].push(rest);
            }
        });
        return mapper;
    };

    const markup = () => {
        const mapper = positionMaintainer();
        return Object.keys(mapper).map((position, index) => {
            const content: any = mapper[position].map(({ key, content, variant, className }: MapperValuesInterface) => {
                let animationCssClass = 'toast-item-animation-top';
                if (position.indexOf('bottom')) animationCssClass = 'toast-item-animation-bottom';
                return (
                    <div key={key} className={
                        `${style['toast-item']}
                        ${style[(`toast-item-${variant}`)]}
                        ${style[(`${animationCssClass}`)]}
                        ${className ? `${style[(`${className}`)]}` : ''}`}>{content}</div>
                );
            });
            return (
                <div key={index} className={`${style['toast-container']} ${style[(`${position}`)]}`}>
                    {content}
                </div>
            );
        });
    };


    if(!toastRef.current) return null;
    return createPortal(
        markup(),
        toastRef.current
    );
};

export default ToastContainer;