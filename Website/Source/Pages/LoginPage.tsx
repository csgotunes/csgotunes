import React, { MouseEvent, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { InitAuthResponse } from '../Models/InitAuthResponse';
import { getApiBaseUrl } from '../Utils';

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
    <div>
      <button disabled={loggingIn} onClick={onLoginButtonClick}>Login to Spotify</button>
    </div>
  );
};
