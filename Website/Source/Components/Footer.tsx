import React from 'react';
import { useTranslation } from 'react-i18next';

export const Footer: React.FunctionComponent<any> = () => {
  const { t } = useTranslation();

  return (
    <div id="footer">
      <p>{t('disclaimer')}</p>
    </div>
  );
};
