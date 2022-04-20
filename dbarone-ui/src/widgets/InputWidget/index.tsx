import React, { FunctionComponent, MouseEventHandler } from 'react';
import style from './style.css';

type indexableObject = {
  [key: string]: any
}

interface InputProps {
  name?: string;
  type: 'input' | 'checkbox' | 'button' | 'color' | 'date' | 'datetime-local' | 'email' | 'file' | 'hidden' | 'image' | 'month' | 'number' | 'password' | 'radio' | 'range' | 'reset' | 'search' | 'submit' | 'tel' | 'text' | 'time' | 'url' | 'week';
  label?: string;
  disabled?: boolean;
  rows?: number;
  onInputHook?: any;
  state?: [any, React.Dispatch<React.SetStateAction<any>>]
}


const InputWidget: FunctionComponent<InputProps> = ({ name = undefined, type, label = undefined, state = undefined, disabled = false, rows = 1, onInputHook = undefined }) => {
    const [obj, setObj] = state ? state : [undefined, undefined];
  
    const onInput = (e: any ) => {
        const val = e.target.type === 'checkbox' ? e.target.checked : e.target.value;
        if (setObj) {
            setObj({ ...obj, [e.target.name]: val });
        }

        if (onInputHook) {
            onInputHook(e);
        }
    };

    const input = () => {
        if (type === 'text' && rows > 0) {
            {/* Textarea */ }
            return (
                <textarea
                    rows={rows}
                    disabled={disabled}
                    className={disabled ? style.readonly : style.writeable}
                    name={name}
                    value={name ? obj[name] : ''}
                    onInput={onInput}
                />
            );
        } else {
            return (
                <input
                    disabled={disabled}
                    className={disabled ? style.readonly : style.writeable}
                    type={type}
                    name={name}
                    value={name ? obj[name] : (type==='submit' ? 'Submit': '')}
                    onInput={onInput}
                />
            );
        }
    };

    return (
        <div className={style.field}>
            <label>{label}{label ? ':' : ''}</label>
            {input()}
        </div>
    );
};

export default InputWidget;
