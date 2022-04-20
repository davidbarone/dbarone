import React, { FunctionComponent } from 'react';
import style from './style.css';

const { useState, useEffect } = React;

interface CounterProps {
  startNumber: number;
}

const Counter: FunctionComponent<CounterProps> = ({ startNumber }) => {
    const [value, setValue] = useState(0);

    useEffect(() => { setValue(startNumber); }, []);

    return (
        <div className={style.counter}>
            <div>Current value of counter is: {value}</div>
            <button onClick={() => setValue((value) => value + 1)}>Increment</button>
            <button onClick={() => setValue((value) => value - 1)}>Decrement</button>
        </div>
    );
};

export default Counter;