import { combineReducers } from '@reduxjs/toolkit';
import { connectRouter } from 'connected-react-router';
import { History } from 'history';
import authenticationReducer from '../features/authentication/authenticationSlice';

const createRootReducer = (history: History<unknown>) => combineReducers({
  router: connectRouter(history),
  authentication: authenticationReducer
});

export type RootState = ReturnType<ReturnType<typeof createRootReducer>>;

export default createRootReducer;
