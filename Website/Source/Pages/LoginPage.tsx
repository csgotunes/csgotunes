import React, { MouseEvent, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { InitAuthResponse } from '../Models/InitAuthResponse';
import { getApiBaseUrl } from '../Utils';
import { Header } from '../Components/Header';
import { Footer } from '../Components/Footer';

export const LoginPage: React.FunctionComponent<any> = () => {
  const history = useHistory();
  const [loggingIn, setLoggingIn] = useState<boolean>(false);

  useEffect(() => {
    const sessionId = localStorage.getItem('CSGOTunesAuthToken');

    if (sessionId !== null && sessionId.trim() !== '') {
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
          <p>Vibe out to music while playing Counter-Strike: Global Offensive without sacrificing awareness. CS:GO Tunes can automatically pause your music when you respawn, and resume the track once you are eliminated from the round. To get started, login with Spotify using the button below!</p>
          <button id="loginButton" disabled={loggingIn} onClick={onLoginButtonClick}>Login to Spotify</button>
        </div>
      </div>
      <Footer />
    </div>
  );
};
