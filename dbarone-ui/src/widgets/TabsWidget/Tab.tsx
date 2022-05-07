import React, { FunctionComponent, useRef, useEffect } from 'react';

export interface TabProps {
    title: string
    children?: React.ReactNode
}

export const Tab: FunctionComponent<TabProps> = ({ title, children }) => {
    return (
        <div role="tabpanel">
            {children}
        </div>
    );
};
