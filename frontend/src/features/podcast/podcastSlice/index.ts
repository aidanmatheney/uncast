import { createSlice } from '@reduxjs/toolkit';
import { PodcastBase } from '../../../common/entities';

interface PodcastSliceState {
  podcastById: Record<string, PodcastBase>;
}

const initialState: PodcastSliceState = {
  podcastById: { }
};

export const podcastSlice = createSlice({
  name: 'podcast',
  initialState,
  reducers: {

  },
  extraReducers: map => {

  }
});

// export const {

// } = podcastSlice.actions;

export default podcastSlice.reducer;
