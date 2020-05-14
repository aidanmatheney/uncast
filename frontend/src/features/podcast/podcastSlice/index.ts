import { createSlice, PayloadAction, createSelector, createAsyncThunk } from '@reduxjs/toolkit';
import { RssPodcast, FilePodcast } from '../../../common/entities';
import { xml2js } from 'xml-js';
import { RootState } from '../../../app/rootReducer';

const sliceName = 'podcast';

interface PodcastSliceState {
  rssPodcastById: Record<string, RssPodcast>;
  filePodcastById: Record<string, FilePodcast>;

  catalog: Record<string, boolean>;
  subscriptions: Record<string, boolean>;
}

const initialState: PodcastSliceState = {
  rssPodcastById: { },
  filePodcastById: { },

  catalog: { },
  subscriptions: { }
};

interface ThunkApiConfig {
  state: RootState;
}

export const registerRssPodcast = createAsyncThunk<RssPodcast, { feedUrl: string; }, ThunkApiConfig>(`${sliceName}/registerRssPodcast`, async ({ feedUrl }, { getState }) => {
  const state = getState();

  const existingPodcast = state.podcast.rssPodcastById[feedUrl];
  if (existingPodcast != null) {
    return existingPodcast;
  }

  const response = await fetch(feedUrl);
  const rss = await response.text();

  const doc = xml2js(rss);
  const entries = doc
    .elements[0]
    .elements.filter(({ type }: { type: string; }) => type === 'element')[0]
    .elements;

  console.log('register RSS entries:', entries);

  const title: string = entries
    .find((el: any) => el.name === 'title')
    .elements[0].text;
  const url: string = entries
    .find((el: any) => el.name === 'link')
    .elements[0].text;
  const imageUrl: string = entries
    .find((el: any) => el.name === 'image')
    .elements
    .find((el: any) => el.name === 'url')
    .elements[0].text;

  const podcast: RssPodcast = {
    id: feedUrl,
    name: title,
    author: '',
    description: '',
    thumbnailUrl: imageUrl,
    feedUrl: feedUrl
  };

  return podcast;
});

export const catalogRssPodcast = createAsyncThunk<string, { feedUrl: string }, ThunkApiConfig>(`${sliceName}/catalogRssPodcast`, async ({ feedUrl }, { getState, dispatch }) => {
  await dispatch(registerRssPodcast({ feedUrl }));
  return feedUrl;
});

export const subscribeToPodcast = createAsyncThunk<string, { feedUrl: string } | { id: string }, ThunkApiConfig>(`${sliceName}/subscribeToRssPodcast`, async (payload, { getState, dispatch }) => {
  if ('feedUrl' in payload) {
    await dispatch(registerRssPodcast({ feedUrl: payload.feedUrl }));
    return payload.feedUrl;
  }

  return payload.id;
});

export const podcastSlice = createSlice({
  name: sliceName,
  initialState,
  reducers: {
    unsubscribeFromPodcast(state, { payload: { id } }: PayloadAction<{ id: string; }>) {
      delete state.subscriptions[id];
    }
  },
  extraReducers: map => {
    map.addCase(registerRssPodcast.fulfilled, (state, { payload: podcast }) => {
      state.rssPodcastById[podcast.id] = podcast;
    });
    map.addCase(catalogRssPodcast.fulfilled, (state, { payload: feedUrl }) => {
      state.catalog[feedUrl] = true;
    });
    map.addCase(subscribeToPodcast.fulfilled, (state, { payload: id }) => {
      state.subscriptions[id] = true;
    });
  }
});
export default podcastSlice.reducer;

export const {
  unsubscribeFromPodcast
} = podcastSlice.actions;
