import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import { routerMiddleware } from 'connected-react-router'
import { createBrowserHistory } from 'history';
import createRootReducer from './createRootReducer';

export const history = createBrowserHistory();

const loggerMiddleware = (store: any) => (next: any) => (action: { type: any, payload: any }) => {
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
    reducer: createRootReducer(history),
    middleware: [
      // TODO: eliminate serializable values from all slice states and enable serializableCheck
      ...getDefaultMiddleware({ serializableCheck: false }),
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
