import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RssPodcast, Podcast } from '../../../common/entities';
import { standardTheme, Theme } from '../../theme';

interface UserState {
  theme: Theme;
}

const initialState: UserState = {
  theme: standardTheme
};

export const authenticationSlice = createSlice({
  name: 'authentication',
  initialState,
  reducers: {
    setTheme(state, { payload: theme }: PayloadAction<UserState['theme']>) {
      state.theme = theme;
    }
  }
});

export const {
  setTheme
} = authenticationSlice.actions;

export default authenticationSlice.reducer;
