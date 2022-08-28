import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';

export const DashboardPage: React.FunctionComponent<any> = () => {
  const history = useHistory();

  useEffect(() => {
    const sessionId = localStorage.getItem('CSGOTunesAuthToken');

    if (sessionId === null || sessionId.trim() === '') {
      localStorage.removeItem('CSGOTunesAuthToken');
      history.push('/login');
    }
  }, []);

  return (
    <div>
      <p>Welcome to the dashboard.</p>
    </div>
  );
};
