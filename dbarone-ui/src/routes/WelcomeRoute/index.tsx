import React, { FunctionComponent } from 'react';
import style from './style.css';

const WelcomeRoute: FunctionComponent = () => {
    return (
        <>
            <section>
                <div className={style.blockquoteContainer}>
                    <blockquote cite="https://www.dbarone.com/">
                        <p>Developer and business intelligence lead, living on the Australian NSW Central Coast.</p>
                        <cite>
                            <a href="https://www.dbarone.com">David Barone</a>
                        </cite>
                    </blockquote>
                </div>

                <div className={ style.links }>
                    <a title="Linkedin" href="https://www.linkedin.com/in/david-barone-083aa05b/">LinkedIn</a>
                    <a title="Github" href="https://github.com/davidbarone">Github</a>
                    <a title="Mail" href="mailto:davidbarone@live.com">Email</a>
                </div>
            </section>
        </>
    );
};

export default WelcomeRoute;