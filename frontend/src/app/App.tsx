import React, { FunctionComponent, useState, useEffect } from 'react';
import { Provider as ReduxProvider, useDispatch, useSelector } from 'react-redux';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import styled, { ThemeProvider } from 'styled-components';

import createStore from './createStore';
import { RootState } from './rootReducer';

import Library from '../features/library';
import Catalog from '../features/catalog';
import Profile from '../features/profile';
import NavBar from '../features/navbar';
import Player from '../features/player';
import TabId from '../common/TabId';
import { loadState, saveState } from './state';
import catalogPodcastFeeds from '../features/catalog/catalogPodcastFeeds';
import { catalogPodcast, setEpisodePlaybackPosition } from '../features/podcast/podcastSlice';
import { Episode } from '../common/entities';

import debounce from 'lodash.debounce';

const Wrapper = styled.section`
  /* padding: 3em; */
  background: ${props => props.theme.pageBgColor};
`;

const Container = styled.div`
  height: 100vh;
  display: flex;
  flex-direction: column;
  background: ${props => props.theme.pageBgColor};
`;

const ActivityPane = styled.div`
  flex-grow: 1;

`;

const NavBarPane = styled.div``;

export const store = createStore(loadState());
const debouncedSaveState = debounce(
  () => {
    saveState(store.getState());
  },
  5000,
  {
    leading: true,
    trailing: false,
    maxWait: 5000
  }
);
store.subscribe(debouncedSaveState);

const App: FunctionComponent = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    for (const feedUrl of catalogPodcastFeeds) {
      dispatch(catalogPodcast({ feedUrl }));
    }
  }, [dispatch]);

  const [activeTab, setActiveTab] = useState<TabId>(TabId.Library);
  const theme = useSelector((state: RootState) => state.user.theme);

  const userEpisodeStateById = useSelector((state: RootState) => state.podcast.userEpisodeStateById);

  const [media, setMedia] = useState<{
    episode: Episode;
    url: string;
    startTimeS: number;
  } | undefined>(undefined);

  const handlePlaybackRequested = (episode: Episode) => {
    const startTimeS = (userEpisodeStateById[episode.id]?.playbackMs ?? 0) / 1000;

    let url: string;
    if (episode.type === 'rss') {
      url = episode.fileUrl;
    } else { // episode.type === 'file'
      console.error('File episode playback requested:', episode);
      url = 'http://example.com';
    }

    // console.log('handlePlaybackRequested', {
    //   episode,
    //   url,
    //   startTimeS
    // });

    setMedia({
      episode,
      url,
      startTimeS
    });
  };

  const handlePlaybackTimeChanged = (timeS: number) => {
    if (media == null) {
      return;
    }

    dispatch(setEpisodePlaybackPosition({ id: media.episode.id, playbackMs: timeS * 1000 }));
  };

  return (
    <Wrapper>
      <ThemeProvider theme={theme}>
        <Container>
          <ActivityPane>
            <Routes>
              <Route path="/" element={<Library onPlaybackRequested={handlePlaybackRequested} />} />
              <Route path="/Library" element={<Library onPlaybackRequested={handlePlaybackRequested} />} />
              <Route path="/Catalog" element={<Catalog onPlaybackRequested={handlePlaybackRequested} />} />
              <Route path="/Profile" element={<Profile />} />

              <Route element={<div>
                <div>Unmatched route:</div>
                <pre>{JSON.stringify(window.location, null, 2)}</pre>
              </div>} />
            </Routes>
          </ActivityPane>

          <NavBarPane>
            <Player media={media} onTimeChanged={handlePlaybackTimeChanged} />
            <NavBar activeTab={activeTab} onTabClick={setActiveTab} />
          </NavBarPane>
        </Container>
      </ThemeProvider>
    </Wrapper>
  );
};

const WrappedApp: typeof App = props => (
  <ReduxProvider store={store}>
    <BrowserRouter>
      <App {...props} />
    </BrowserRouter>
  </ReduxProvider>
);
export default WrappedApp;
