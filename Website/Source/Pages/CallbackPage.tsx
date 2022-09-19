import React, { useEffect } from 'react';
import { getApiBaseUrl, isNullOrWhitespace } from '../Utils';
import { CompleteAuthResponse } from '../Models/CompleteAuthResponse';
import { useHistory } from 'react-router-dom';
import { setSession } from '../Utils/AuthUtils';
import { useTranslation } from 'react-i18next';

export const CallbackPage: React.FunctionComponent<any> = () => {
  const history = useHistory();
  const { t } = useTranslation();

  useEffect(() => {
    void (async function () {
      const params = new URLSearchParams(window.location.search);
      // Get the value of "some_key" in eg "https://example.com/?some_key=some_value"
      const state = params.get('state');
      const code = params.get('code');

      if (isNullOrWhitespace(state) || isNullOrWhitespace(code)) {
        console.log('Oh, no!');
        return;
      }

      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          code,
          state
        })
      };

      const response = await fetch(getApiBaseUrl() + '/complete-auth', requestOptions);

      if (response.status >= 400) {
        console.log('Oh, no!');
      }

      const completeAuthResponse: CompleteAuthResponse = await response.json();
      setSession(completeAuthResponse.sessionID);

      // FIXME: This is a hack to remove the query string parameters.
      // For some reason, when you try to use `history.replace` it does not work properly.
      window.history.replaceState({}, document.title, '/#');

      history.push('/dashboard');
    })();
  }, []);

  return (
    <div>
      <p>{t('loading')}</p>
    </div>
  );
};
