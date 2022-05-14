import React, { FunctionComponent } from 'react';
import ListPostsComponent from '../../components/ListPostsComponent';
import ResourcesComponent from '../../components/ResourcesComponent';
import { TabsWidget } from '../../widgets/TabsWidget';
import { Tab } from '../../widgets/TabsWidget/Tab';

const AdminRoute: FunctionComponent = () => {
    return (
        <>
            <h1>Admin</h1>
            <TabsWidget activeTabId={0}>
                <Tab title='Posts'>
                    <ListPostsComponent allowEdit={true}></ListPostsComponent>
                </Tab>
                <Tab title='Resources'>
                    <ResourcesComponent></ResourcesComponent>
                </Tab>
                <Tab title='Users'>
                    <div>This is a users tab</div>
                </Tab>
                <Tab title='Groups'>
                    <div>This is a groups tab</div>
                </Tab>
            </TabsWidget>
        </>
    );
};

export default AdminRoute;