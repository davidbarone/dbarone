import React, { FunctionComponent, useRef, useEffect } from 'react';
import style from './style.css';

interface SliderProps {
    visibilityState: [boolean, React.Dispatch<React.SetStateAction<boolean>>];
    onClose?: () => void;
    children?: React.ReactNode
}

/**
 * A slider widget.
 * @param param0 
 * @returns 
 */
const SliderWidget: FunctionComponent<SliderProps> = ({ visibilityState, onClose = () => { return; }, children = []}) => {
    const contentDiv = useRef<HTMLDivElement>(null);
    const modalDiv = useRef<HTMLDivElement>(null);
    const [visible, setVisible] = visibilityState;

    useEffect(() => {
        if (visible) {
            if (contentDiv.current) {
                contentDiv.current.classList.add(style.slideIn);
                contentDiv.current.classList.remove(style.slideOut);
            }
            if (modalDiv.current) {
                modalDiv.current.classList.add(style.fadeIn);
                modalDiv.current.classList.remove(style.fadeOut);
            }
        } else {
            if (contentDiv.current) {
                contentDiv.current.classList.add(style.slideOut);
                contentDiv.current.classList.remove(style.slideIn);
            }
            if (modalDiv.current) {
                if (modalDiv.current.classList.contains(style.fadeIn)) {
                    modalDiv.current.classList.add(style.fadeOut);
                    modalDiv.current.classList.remove(style.fadeIn);
                }
            }
        }
    }, [visible]);

    const hideModal = () => {
        setVisible(false);
        if (onClose) {
            onClose();
        }
    };


    return (
        <div className={style.myModal} ref={modalDiv}>
            <div className={style.myModalContent} ref={contentDiv}>
                <div className={style.container}>
                    {children}

                    <button
                        className={style.closeButton}
                        onClick={() => {
                            hideModal();
                        }}
                    >
                    X
                    </button>
                </div>
            </div>
        </div>
    );
};

export default SliderWidget;
