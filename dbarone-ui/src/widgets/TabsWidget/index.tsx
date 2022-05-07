import React, { FunctionComponent, useEffect, useState } from 'react';
import style from './style.css';

/**
 * Header item props
 */
interface TabHeaderItemProps {
    title: string
    active: boolean
    onSelected: () => void
}

/**
 * Tab Header - the button that allows current tabId to be changed
 * @param param0 
 * @returns 
 */
export const TabHeaderItem: FunctionComponent<TabHeaderItemProps> = ({ title, active, onSelected }) => {
    return (
        <button className={active ? style.active : {}} onClick={() => onSelected()}>
            {title}
        </button>
    );
};

type reduceType = {
    header: JSX.Element[],
    body: JSX.Element[]
}

interface TabWidgetProps {
    activeTabId: number
    children: JSX.Element[]
}

export const TabsWidget: FunctionComponent<TabWidgetProps> = ({
    activeTabId = 0,
    children
}) => {
    // stores the current tab index
    const [tabId, setTabId] = useState<number>(activeTabId);
    const [reduce, setReduce] = useState<reduceType>({header: [], body: []});

    useEffect(() => {
        // update tab state
        const state = tabsToRender(children, tabId, setTabId);
        setReduce(state);
    }, [tabId]);

    return (
        <div>
            <div className={style.tab}><>{reduce.header}</></div>
            <div className={style.tabcontent}>{reduce.body}</div>
        </div>
    );
};

function tabsToRender(children: JSX.Element[], activeTab: number, setTabIdState: React.Dispatch<React.SetStateAction<number>>): reduceType {
    const initialValue: { header: JSX.Element[], body: JSX.Element[] } = {
        header: [],
        body: [],
    };

    // acc, tab, idx
    return children.reduce<reduceType>((prev: reduceType, curr: JSX.Element, idx: number): reduceType => {
        const isActive = activeTab === idx;
        const { title } = curr.props;

        const HeaderItem = (
            <TabHeaderItem
                key={idx}
                title={title}
                active={isActive}
                onSelected={() => { setTabIdState(idx); } }
            ></TabHeaderItem>
        );

        const ContentItem = isActive ? curr : <></>;

        return {
            header: [...prev.header, HeaderItem],
            body: [...prev.body, ContentItem],
        };
    }, initialValue);
}
