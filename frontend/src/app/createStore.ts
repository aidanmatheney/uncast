import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import { routerMiddleware } from 'connected-react-router'
import { createBrowserHistory } from 'history';
import createRootReducer from './createRootReducer';

export const history = createBrowserHistory();

const loggerMiddleware = (store: any) => (next: any) => (action: { type: any, payload: any }) => {
  console.log("Action type:", action.type);
  console.log("Action payload:", action.payload);
  console.log("State before:", store.getState());
  next(action);
  console.log("State after:", store.getState());
};

const createStore = (preloadedState?: any) => {
  const store = configureStore({
    reducer: createRootReducer(history),
    middleware: [
      ...getDefaultMiddleware(),
      routerMiddleware(history),
      loggerMiddleware
    ],
    devTools: true,
    preloadedState
  });

  // Hot reloading
  if (module.hot) {
    // Enable Webpack hot module replacement for reducers
    module.hot.accept('./createRootReducer', async () => {
      const newCreateRootReducer = (await import('./createRootReducer')).default;
      store.replaceReducer(newCreateRootReducer(history));
    });
  }

  return store;
};

export type AppDispatch = ReturnType<typeof createStore>['dispatch'];

export default createStore;
