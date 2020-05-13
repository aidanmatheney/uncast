import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RssPodcast, PodcastType } from '../../../common/entities';
import { standardTheme, Theme } from '../../theme';

interface UserState {
  subscriptions: Record<string, PodcastType>;
  theme: Theme;
}

const initialState: UserState = {
  subscriptions: { },
  theme: standardTheme
};

export const authenticationSlice = createSlice({
  name: 'authentication',
  initialState,
  reducers: {
    subscribe(state, { payload: podcast }: PayloadAction<PodcastType>) {
      state.subscriptions[podcast.name] = podcast
    },
    unsubscribe(state, { payload: podcast }: PayloadAction<PodcastType>) {
      delete state.subscriptions[podcast.name];
    },

    setTheme(state, { payload: theme }: PayloadAction<UserState['theme']>) {
      state.theme = theme;
    },

    saveToStore(state) {
      const stateString = JSON.stringify(state);
      localStorage.setItem('userState', stateString);
    },
    loadFromStore(currentState) {
      const stateString = localStorage.getItem('userState');
      if (stateString == null) {
        const defaultPodcasts: RssPodcast[] = [
          {
            name: 'The Joe Rogan Experience',
            author: 'Joe Rogan',
            description: 'The Joe Rogan Experience is a free audio and video podcast hosted by American comedian, actor, sports commentator, martial artist, and television host, Joe Rogan.',
            feedUrl: 'http://joeroganexp.joerogan.libsynpro.com/rss'
          },
          {
            name: 'Reply All',
            author: 'Gimlet Media',
            description: 'Reply All is an American podcast from Gimlet Media, hosted by PJ Vogt and Alex Goldman. The show features stories about how people shape the internet, and how the internet shapes people.',
            feedUrl: 'https://feeds.megaphone.fm/replyall'
          }
        ]; // TODO: Select from redux store

        defaultPodcasts.forEach(podcast => currentState.subscriptions[podcast.name] = podcast);

        return currentState;
      }

      const state: UserState = JSON.parse(stateString);
      return state;
    }
  }
});

export const {
  subscribe,
  unsubscribe,

  setTheme,

  saveToStore,
  loadFromStore,
} = authenticationSlice.actions;

export default authenticationSlice.reducer;
