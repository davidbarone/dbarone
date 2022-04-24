import React, { FunctionComponent, ChangeEvent, useRef } from 'react';
import style from './style.css';

interface DropdownProps {
  name?: string;
  label: string;
  values: string[];
  texts?: string[];
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
    texts = undefined,
    multiple = false,
    size = 4,
    selectedValue = undefined,
    onInputHook = undefined,
    nullText = undefined }) => {

    const [target, setTarget] = state ? state : [undefined, undefined];
    const selectNode = useRef<HTMLSelectElement>(null);

    const onChange = (e: ChangeEvent<HTMLSelectElement>) => {
        if (selectNode.current !== null) {
            if (!multiple) {
                const val = e.target.value;
                if (setTarget != null) {
                    setTarget({ ...target, [e.target.name]: val });
                }
            } else {
                // If multiple select, we must get the value a slightly longer way.
                const val = Array.from(selectNode.current.options)
                    .filter((o) => o.selected)
                    .map((o) => o.value);
                if (setTarget != null) {
                    setTarget({ ...target, [e.target.name]: val });
                }
            }

            if (onInputHook) {
                onInputHook(e);
            }
        }
    };

    values = values || [];
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
                {(nullText !== undefined ? ['', ...values] : values).map((v, i) => (
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
