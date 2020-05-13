import { combineReducers } from '@reduxjs/toolkit';

import userReducer from '../features/user/userSlice';
import podcastReducer from '../features/podcast/podcastSlice';

const rootReducer = combineReducers({
  user: userReducer,
  podcast: podcastReducer
});
export default rootReducer;

export type RootState = ReturnType<typeof rootReducer>;
