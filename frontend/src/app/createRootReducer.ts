import { combineReducers } from '@reduxjs/toolkit';
import { connectRouter } from 'connected-react-router';
import { History } from 'history';
import { adminReducer } from 'react-admin';

import authenticationReducer from '../features/authentication/authenticationSlice';
import podcastReducer from '../features/podcast/podcastSlice';

const createRootReducer = (history: History<unknown>) => combineReducers({
  router: connectRouter(history),
  authentication: authenticationReducer,
  admin: adminReducer,
  podcast: podcastReducer
});

export type RootState = ReturnType<ReturnType<typeof createRootReducer>>;

export default createRootReducer;
