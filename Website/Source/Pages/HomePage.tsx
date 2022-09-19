import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { clearSession, getSession } from '../Utils/AuthUtils';

export const HomePage: React.FunctionComponent<any> = () => {
  const history = useHistory();

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
      <p>Loading...</p>
    </div>
  );
};
