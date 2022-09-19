import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { clearSession } from '../Utils/AuthUtils';
import { useTranslation } from 'react-i18next';

export const LogoutPage: React.FunctionComponent<any> = () => {
  const history = useHistory();
  const { t } = useTranslation();

  useEffect(() => {
    clearSession();
    history.push('/login');
  }, []);

  return (
    <div>
      <p>{t('logging_out')}</p>
    </div>
  );
};
