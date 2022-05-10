import React, { FunctionComponent, ChangeEvent, useRef, useState } from 'react';
import style from './style.css';

interface FileUploadProps {
    name?: string;
    label: string;
    action: (e: ChangeEvent<HTMLInputElement>, file: File) => void;
    visible?: boolean;
}

type UploadType = 'INITIAL' | 'SAVING' | 'SUCCESS' | 'FAILURE';

const FileUploaderWidget: FunctionComponent<FileUploadProps> = ({ name, label, action, visible = true }) => {
    const [uploadState, setUploadState] = useState<UploadType>('INITIAL');

    const uploadFile = (e: ChangeEvent<HTMLInputElement>) => {
        setUploadState('SAVING');
        if (e.target.files === null) {
            return;
        }
        const file = e.target.files[0];
        setTimeout(() => {
            action(e, file);
            setUploadState('INITIAL');
            e.preventDefault();
        }, 0);
    };

    return (
        <label className={style.dropbox} title="Drag file here to upload">
            <input
                type="file"
                name={name}
                disabled={uploadState === 'SAVING'}
                onChange={uploadFile}
                accept="*"
                className={style.inputFile}
            />
            <p style={{ display: uploadState === 'INITIAL' ? 'inline' : 'none' }}>
                {label}
            </p>
            <p style={{ display: uploadState === 'SAVING' ? 'inline' : 'none' }}>
                Uploading file...
            </p>
        </label>
    );
};

export default FileUploaderWidget;