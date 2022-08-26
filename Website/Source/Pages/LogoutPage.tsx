import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';

export const LogoutPage: React.FunctionComponent<any> = () => {
  const history = useHistory();

  useEffect(() => {
    localStorage.removeItem('CSGOTunesAuthToken');
    history.push('/login');
  }, []);

  return (
    <div>
      <p>Logging out...</p>
    </div>
  );
};
