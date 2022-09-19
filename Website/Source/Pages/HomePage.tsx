import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { clearSession, getSession } from '../Utils/AuthUtils';
import { useTranslation } from 'react-i18next';

export const HomePage: React.FunctionComponent<any> = () => {
  const history = useHistory();
  const { t } = useTranslation();

  useEffect(() => {
    const sessionId = getSession();

    if (sessionId === null) {
      clearSession();
      history.push('/login');
      return;
    }

    history.push('/dashboard');
  }, []);

  return (
    <div>
      <p>{t('loading')}</p>
    </div>
  );
};
