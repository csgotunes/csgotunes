import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { clearSession } from '../Utils/AuthUtils';

export const LogoutPage: React.FunctionComponent<any> = () => {
  const history = useHistory();

  useEffect(() => {
    clearSession();
    history.push('/login');
  }, []);

  return (
    <div>
      <p>Logging out...</p>
    </div>
  );
};
