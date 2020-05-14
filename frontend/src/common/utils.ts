export const formatSeconds = (seconds: number) => {
  const date = new Date(0);
  date.setSeconds(seconds);
  return date.toISOString().substr(11, 8);
};

export const formatUnixDate = (unixDate: number) => {
  return new Date(unixDate).toLocaleDateString();
};
