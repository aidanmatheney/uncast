import { combineReducers } from '@reduxjs/toolkit';
import { connectRouter } from 'connected-react-router';
import { History } from 'history';

import userReducer from '../features/user/userSlice';
import podcastReducer from '../features/podcast/podcastSlice';

const createRootReducer = (history: History<unknown>) => combineReducers({
  router: connectRouter(history),
  user: userReducer,
  podcast: podcastReducer
});

export type RootState = ReturnType<ReturnType<typeof createRootReducer>>;

export default createRootReducer;
