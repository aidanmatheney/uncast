import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import rootReducer from './rootReducer';

const loggerMiddleware = (store: any) => (next: any) => (action: { type: string; payload: any; }) => {
  const beforeState = store.getState();
  next(action);
  const afterState = store.getState();

  console.log('Redux action:', {
    type: action.type,
    payload: action.payload,
    beforeState,
    afterState
  });
};

const createStore = (preloadedState?: any) => {
  const store = configureStore({
    reducer: rootReducer,
    middleware: [
      ...getDefaultMiddleware(),
      loggerMiddleware
    ],
    devTools: true,
    preloadedState
  });

  // Hot reloading
  if (module.hot) {
    // Enable Webpack hot module replacement for reducers
    module.hot.accept('./rootReducer', async () => {
      const newRootReducer = (await import('./rootReducer')).default;
      store.replaceReducer(newRootReducer);
    });
  }

  return store;
};

export type AppDispatch = ReturnType<typeof createStore>['dispatch'];

export default createStore;
