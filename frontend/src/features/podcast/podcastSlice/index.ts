import { createSlice, PayloadAction, createAsyncThunk } from '@reduxjs/toolkit';
import { Podcast, Episode, RssPodcast, RssEpisode } from '../../../common/entities';
import { xml2js } from 'xml-js';
import { RootState } from '../../../app/rootReducer';

const sliceName = 'podcast';

interface PodcastSliceState {
  podcastById: Record<string, Podcast>;
  episodeById: Record<string, Episode>;
  episodesByPodcastId: Record<string, Episode[]>;
  podcastState: Record<string, {
    catalog: boolean;
    lastRefreshTime: number;
  }>;

  userPodcastStateById: Record<string, {
    subscribed: boolean;
  }>;
  userEpisodeStateById: Record<string, {
    favorite: boolean;
    playbackMs: number;
  }>;
  userPlaybackQueue: string[];
}

const initialState: PodcastSliceState = {
  podcastById: { },
  episodeById: { },
  episodesByPodcastId: { },
  podcastState: { },

  userPodcastStateById: { },
  userEpisodeStateById: { },
  userPlaybackQueue: []
};

const initialPodcastState = {
  catalog: false,
  lastRefreshTime: 0
};

const initialUserPodcastState = {
  subscribed: false
};

const initialUserEpisodeState = {
  favorite: false,
  playbackMs: 0
};

interface ThunkApiConfig {
  state: RootState;
}

export const registerPodcast = createAsyncThunk<Podcast, { feedUrl: string; }, ThunkApiConfig>(`${sliceName}/registerPodcast`, async (payload, { getState }) => {
  const state = getState();

  const existingPodcast = state.podcast.podcastById[payload.feedUrl];
  if (existingPodcast != null) {
    return existingPodcast;
  }

  const response = await fetch(payload.feedUrl);
  const rss = await response.text();

  const doc = xml2js(rss);
  const entries = doc
    .elements[0]
    .elements.filter(({ type }: { type: string; }) => type === 'element')[0]
    .elements;

  const title: string = entries
    .find((el: any) => el.name === 'title')
    .elements[0].text;
  const description: string = entries
    .find((el: any) => el.name === 'description')
    .elements[0].text;
  const author: string = entries
    .find((el: any) => el.name === 'itunes:author')
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
    type: 'rss',
    id: payload.feedUrl,
    name: title,
    author,
    description,
    url,
    thumbnailUrl: imageUrl,
    feedUrl: payload.feedUrl
  };

  return podcast;
});

export const catalogPodcast = createAsyncThunk<Podcast, { feedUrl: string }, ThunkApiConfig>(`${sliceName}/catalogPodcast`, async ({ feedUrl }, { getState, dispatch }) => {
  const { payload: podcast } = await dispatch(registerPodcast({ feedUrl }));
  return podcast!;
});

export const subscribeToPodcast = createAsyncThunk<string, { feedUrl: string } | { id: string }, ThunkApiConfig>(`${sliceName}/subscribeToPodcast`, async (payload, { getState, dispatch }) => {
  if ('feedUrl' in payload) {
    await dispatch(registerPodcast({ feedUrl: payload.feedUrl }));
    return payload.feedUrl;
  }

  return payload.id;
});

export const refreshEpisodes = createAsyncThunk<{ podcastId: string; episodes: Episode[]; } | null, { podcastId: string }, ThunkApiConfig>(`${sliceName}/refreshEpisodes`, async ({ podcastId }, { getState }) => {
  const state = getState();

  const podcast = state.podcast.podcastById[podcastId];
  if (podcast.type !== 'rss') {
    return null;
  }

  const podcastState = state.podcast.podcastState[podcastId];
  if (podcastState?.lastRefreshTime != null && (new Date().getTime() - podcastState.lastRefreshTime) < 5 * 60 * 1000) {
    return null;
  }

  const response = await fetch(podcast.feedUrl);
  const rss = await response.text();

  const doc = xml2js(rss);
  const entries = doc
    .elements[0]
    .elements.filter(({ type }: { type: string; }) => type === 'element')[0]
    .elements;



  const episodes: Episode[] = entries
    .filter(({ name }: { name: string; }) => name === 'item')
    .map(({ elements }: { elements: any[] }) => elements)
    .map((metas: { name: string; elements: [{ text?: string; cdata?: string; }]; attributes?: { url: string; }; }[]) => {
      const name = metas.find(({ name }) => name === 'title')!.elements[0].text!;
      const descriptionEl = metas.find(({ name }) => name === 'description')?.elements?.[0];
      const description = descriptionEl?.text ?? descriptionEl?.cdata;
      const dateString = metas.find(({ name }) => name === 'pubDate')?.elements?.[0].text;
      const dateUnix = dateString ? new Date(dateString).getTime() : undefined;

      const durationString = metas.find(({ name }) => name === 'itunes:duration')?.elements?.[0].text!;
      const durationS = durationString.split(':').map((part, index, arr) => Number(part) * Math.pow(60, arr.length - index - 1)).reduce((a, c) => a + c);

      const guid = metas.find(({ name }) => name === 'guid')!.elements[0].cdata;
      const url = metas.find(({ name }) => name === 'enclosure')!.attributes!.url;

      const episode: RssEpisode = {
        type: 'rss',
        id: guid || url,
        podcastId,
        name,
        description,
        fileUrl: url,
        dateUnix,
        durationS
      };
      return episode;
    });

    console.log('refreshEpisodes', {
      episodes,
      episodesRaw: entries
        .filter(({ name }: { name: string; }) => name === 'item')
        .map(({ elements }: { elements: any[] }) => elements)
    });

    return {
      podcastId,
      episodes
    };
});

export const podcastSlice = createSlice({
  name: sliceName,
  initialState,
  reducers: {
    unsubscribeFromPodcast(state, { payload: { id } }: PayloadAction<{ id: string; }>) {
      state.userPodcastStateById[id] = {
        ...initialUserPodcastState,
        ...state.userPodcastStateById[id],
        subscribed: false
      }
    },

    setEpisodePlaybackPosition(state, { payload: { id, playbackMs } }: PayloadAction<{ id: string; playbackMs: number; }>) {
      //console.log('setEpisodePlaybackPosition', { playbackMs });

      state.userEpisodeStateById[id] = {
        ...initialUserEpisodeState,
        ...state.userEpisodeStateById[id],
        playbackMs
      }
    },

    setEpisodeFavorite(state, { payload: { id, favorite } }: PayloadAction<{ id: string; favorite: boolean; }>) {
      state.userEpisodeStateById[id] = {
        ...initialUserEpisodeState,
        ...state.userEpisodeStateById[id],
        favorite
      }
    },
  },
  extraReducers: map => {
    map.addCase(registerPodcast.fulfilled, (state, { payload: podcast }) => {
      state.podcastById[podcast.id] = podcast;
    });
    map.addCase(catalogPodcast.fulfilled, (state, { payload: podcast }) => {
      state.podcastState[podcast.id] = {
        ...initialPodcastState,
        ...state.podcastState[podcast.id],
        catalog: true
      };
    });
    map.addCase(subscribeToPodcast.fulfilled, (state, { payload: id }) => {
      state.userPodcastStateById[id] = {
        ...initialUserPodcastState,
        ...state.userPodcastStateById[id],
        subscribed: true
      };
    });

    map.addCase(refreshEpisodes.fulfilled, (state, { payload }) => {
      if (payload == null) {
        return;
      }

      const { podcastId, episodes } = payload;

      state.episodesByPodcastId[podcastId] = episodes;
      for (const episode of episodes) {
        state.episodeById[episode.id] = episode;
      }

      state.podcastState[podcastId] = {
        ...initialPodcastState,
        ...state.podcastState[podcastId],
        lastRefreshTime: new Date().getTime()
      }
    });
  }
});
export default podcastSlice.reducer;

export const {
  unsubscribeFromPodcast,
  setEpisodePlaybackPosition,
  setEpisodeFavorite
} = podcastSlice.actions;
