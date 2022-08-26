import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';

export const HomePage: React.FunctionComponent<any> = () => {
  const history = useHistory();

  useEffect(() => {
    const sessionId = localStorage.getItem('CSGOTunesAuthToken');

    if (sessionId === null || sessionId.trim() === '') {
      localStorage.removeItem('CSGOTunesAuthToken');
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
