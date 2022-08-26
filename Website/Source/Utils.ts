export const isNullOrWhitespace = (input: string | null | undefined): boolean => {
  return input === undefined || input === null || input.match(/^ *$/) !== null;
};

export const getApiBaseUrl = (): string => {
  const envValue = process.env.API_BASE_URL;

  if (envValue !== undefined && !isNullOrWhitespace(envValue)) {
    return envValue;
  }

  return 'http://localhost:7071/api';
};
