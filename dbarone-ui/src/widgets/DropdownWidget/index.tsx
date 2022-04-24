import React, { FunctionComponent, MouseEventHandler, useRef } from 'react';
import style from './style.css';

interface DropdownProps {
  name?: string;
  label: string;
  values: string[];
  texts: string[];
  multiple: boolean;
  size: number;
  selectedValue: string;
  state?: [any, React.Dispatch<React.SetStateAction<any>>];
  disabled: boolean;
  onInputHook?: any;
  nullText?: string
}

const DropdownWidget: FunctionComponent<DropdownProps> = ({
    name = undefined,
    label = undefined,
    state = undefined,
    disabled = false,
    values = [],
    texts = [],
    multiple = false,
    size = 4,
    selectedValue = undefined,
    onInputHook = undefined,
    nullText=undefined}) => {

    const [target, setTarget] = state ? state : [undefined, undefined];
    const selectNode = useRef(null);

    const onChange = (e) => {
        if (selectNode.current!==null) {
            let val = e.target.value;

            // HTML <option> does not support null values. Must convert '' to null
            if (val === '') {
                val = null;
            }

            // If multiple select, we must get the value a slightly longer way.
            if (multiple) {
                val = Array.from(selectNode.current.options)
                    .filter((o) => o.selected)
                    .map((o) => o.value);
            }
            setTarget({ ...target, [e.target.name]: val });

            if (onInputHook) {
                onInputHook(e);
            }
        }
    };

    return (
        <div className={style.field}>
            <label>{label}:</label>
            <select
                ref={selectNode}
                disabled={disabled}
                multiple={multiple}
                size={size}
                name={name}
                onChange={onChange}
            >
                {(nullText !== undefined ? [undefined, ...values] : values).map((v, i) => (
                    <option
                        key={i}
                        label={
                            v === null
                                ? nullText
                                : (typeof texts != 'undefined' && texts !== null && values.filter((v) => v !== null).indexOf(v) < texts.length)
                                    ? texts[values.filter((v) => v !== null).indexOf(v)]
                                    : v
                        }
                        value={v}
                        selected={
                            Array.isArray(selectedValue)
                                ? selectedValue.indexOf(v) >= 0
                                : selectedValue === v
                        }
                    />
                ))}
            </select>
        </div>
    );
};

export default DropdownWidget;
