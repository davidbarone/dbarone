import React, { FunctionComponent } from 'react';

const WelcomeRoute: FunctionComponent = () => {
    return (
        <>
            <h1>Welcome</h1>
            <p>Welome to the Web UI Reference Application. This application is a demonstration web UI application. You can get the source from:</p>
            <a href='https://github.com/davidbarone/webapp'>https://github.com/davidbarone/webapp</a>
            <p>This application is your standard blog site, featuring posts and comments. The API is faked using Json-Server, and is reset when you restart the application. Note that Json-Server supports updates and deletes.</p>
        </>
    );
};

export default WelcomeRoute;