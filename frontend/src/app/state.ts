import { RootState } from './rootReducer';

export const loadState: () => RootState | undefined = () => {
  const stateString = localStorage.getItem('state');
  if (stateString == null) {
    return undefined;
  }

  try {
    return JSON.parse(stateString);
  } catch (err) {
    return undefined;
  }
};

export const saveState = (state: RootState) => {
  const stateString = JSON.stringify(state);
  localStorage.setItem('state', stateString);
};
