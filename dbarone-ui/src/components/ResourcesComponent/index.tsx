import React, { useState, useEffect, FunctionComponent } from 'react';
import { httpGet, httpDelete, httpPost } from '../../utils/ApiFacade';
import TableWidget from '../../widgets/TableWidget';
import ButtonWidget from '../../widgets/ButtonWidget';
import SliderWidget from '../../widgets/SliderWidget';
import EditPostComponent from '../../components/EditPostComponent';
import { LinkType } from '../../types/LinkType';
import FileUploaderWidget from '../../widgets/FileUploadWidget';

type ResourceType = {
    id: number,
    filename: string,
    contentType: string,
    status: string,
    createdDt: Date,
    createdBy: string,
    size: number
}

const ResourcesComponent: FunctionComponent = () => {
    const [resources, setResources] = useState<Array<ResourceType>>([]);
    const [links, setLinks] = useState<Array<LinkType>>([]);
    const [api, setApi] = useState<string>('/resources');

    const getResources = () => {
        httpGet(api, 'Loaded resources successfully.').then((result) => {
            setResources(result.envelope.data);
            setLinks(result.envelope.links);
        });
    };

    const deleteResource = (id: number) => {
        httpDelete(`/resources/${id}`, `Deleted resource ${id} successfully.`).then(r => getResources());
    };

    const addResource = (e: React.ChangeEvent<HTMLInputElement>, file: File) => {
        const fd = new FormData();
        fd.append('file', file);
        httpPost('/resources', fd, 'Resource uploaded successfully', true).then(r => {
            getResources();
        });
    };
    
    useEffect(() => {
        getResources();
    }, [api]);

    return (
        <>
            <TableWidget<ResourceType>
                data={resources}
                visible={true}
                mapping={{
                    'Id': (row) => (<>{row.id}</>),
                    'Filename': (row) => (<>{row.filename}</>),
                    'Content Type': (row) => (<>{row.contentType}</>),
                    'Status': (row) => (<>{row.status}</>),
                    'Created Date': (row) => (<>{row.createdDt}</>),
                    'Content By': (row) => (<>{row.createdBy}</>),
                    'Size': (row) => (<>{row.size}</>),
                    'Delete': (row) => (
                        <>
                            <ButtonWidget
                                title="Delete"
                                label="Delete"
                                visible={true}
                                click={(e) => {
                                    deleteResource(row.id);
                                    e.preventDefault();
                                }} />
                        </>
                    )

                }}
            />

            {/* Pagination */}
            {links.map((l, i) => (
                <ButtonWidget key={i} label={l.rel} click={() => setApi(l.uri) }></ButtonWidget>
            ))}

            {/* Upload Resource */}
            <FileUploaderWidget label='Upload' name='Upload' action={addResource}></FileUploaderWidget>
        </>
    );
};

export default ResourcesComponent;