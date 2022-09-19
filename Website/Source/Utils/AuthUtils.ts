export const getSession = (): string | null => {
  let sessionId = localStorage.getItem('CSGOTunesAuthToken');

  if (sessionId === null || sessionId === undefined) {
    return null;
  }

  sessionId = sessionId.trim();

  if (sessionId === '') {
    return null;
  }

  return sessionId;
};

export const setSession = (sessionId: string): void => {
  localStorage.setItem('CSGOTunesAuthToken', sessionId);
};

export const clearSession = (): void => {
  localStorage.removeItem('CSGOTunesAuthToken');
};
