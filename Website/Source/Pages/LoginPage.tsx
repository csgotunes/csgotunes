import React, { MouseEvent, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { InitAuthResponse } from '../Models/InitAuthResponse';
import { getApiBaseUrl } from '../Utils';
import { Header } from '../Components/Header';
import { Footer } from '../Components/Footer';
import { getSession } from '../Utils/AuthUtils';
import { useTranslation } from 'react-i18next';

export const LoginPage: React.FunctionComponent<any> = () => {
  const history = useHistory();
  const [loggingIn, setLoggingIn] = useState<boolean>(false);
  const { t } = useTranslation();

  useEffect(() => {
    const sessionId = getSession();

    if (sessionId !== null) {
      history.push('/dashboard');
    }
  }, []);

  const onLoginButtonClick = (e: MouseEvent<HTMLButtonElement>): void => {
    void (async function () {
      setLoggingIn(true);

      try {
        const requestOptions = {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' }
        };

        setLoggingIn(true);
        const response = await fetch(getApiBaseUrl() + '/init-auth', requestOptions);

        if (response.status >= 400) {
          console.log('Oh, no!');
        }

        const initAuthResponse: InitAuthResponse = await response.json();

        window.location.href = initAuthResponse.loginUrl;
      } finally {
        setLoggingIn(false);
      }
    })();
  };

  return (
    <div id="loginPage" className="page">
      <Header />
      <div id="welcome" className="section">
        <div className="content">
          <p>{t('welcome_paragraph')}</p>
          <button id="loginButton" disabled={loggingIn} onClick={onLoginButtonClick}>{t('login_button')}</button>
        </div>
      </div>
      <Footer />
    </div>
  );
};
