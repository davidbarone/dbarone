import React, { FunctionComponent, MouseEventHandler } from 'react';
import style from './style.css';

interface ButtonPropsType {
    title?: string;
    label: string | undefined;
    visible?: boolean;
    click?: React.MouseEventHandler<HTMLButtonElement> | undefined;
    href?: string | undefined;
}

const ButtonWidget: FunctionComponent<ButtonPropsType> = ({ label = undefined, visible = true, click, title = '', href = undefined }) => {

    const doClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
        if (href) {
            window.location.href = href;
        } else if (click) {
            click(e);
        }
    };

    return (
        <button
            className={style.ButtonWidget}
            title={title}
            style={visible ? style['display:inline'] : style['display: none']}
            onClick={doClick}
        >
            {label}
        </button>
    );
};

export default ButtonWidget;
