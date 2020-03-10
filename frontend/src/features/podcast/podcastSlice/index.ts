import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { PodcastBase } from '../../../common/web-api';

const sliceName = 'podcast';

interface PodcastSliceState {
  podcastById: Record<string, PodcastBase>;
}

const initialState: PodcastSliceState = {
  podcastById: { }
};

export const podcastSlice = createSlice({
  name: sliceName,
  initialState,
  reducers: {

  },
  extraReducers: map => {

  }
});

export const {

} = podcastSlice.actions;

export default podcastSlice.reducer;
