import React, { FunctionComponent, useState, useEffect } from 'react';
import { Provider as ReduxProvider, useDispatch, useSelector } from 'react-redux';
import { BrowserRouter, Routes, Route, useMatch } from 'react-router-dom';
import styled, { ThemeProvider } from 'styled-components';

import createStore from './createStore';
import { RootState } from './rootReducer';
import { useInterval } from '../common/hooks';

import Library from '../features/library';
import Catalog from '../features/catalog';
import Profile from '../features/profile';
import NavBar from '../features/navbar';
import Player from '../features/player';
import TabId from '../common/TabId';
import { loadState, saveState } from './state';
import catalogPodcastFeeds from '../features/catalog/catalogPodcastFeeds';
import { catalogRssPodcast } from '../features/podcast/podcastSlice';

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

//react-h5-player


const NavBarPane = styled.div``;

export const store = createStore(loadState());
store.subscribe(() => saveState(store.getState()));

const App: FunctionComponent = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    for (const feedUrl of catalogPodcastFeeds) {
      dispatch(catalogRssPodcast({ feedUrl }));
    }
  }, []);

  const [activeTab, setActiveTab] = useState<TabId>(TabId.Library);

  const theme = useSelector((state: RootState) => state.user.theme);

  return (
    <Wrapper>
      <ThemeProvider theme={theme}>
        <Container>
          <ActivityPane>
            <Routes>
              <Route path="/" element={<Library />} />
              <Route path="/Library" element={<Library />} />
              <Route path="/Catalog" element={<Catalog />} />
              <Route path="/Profile" element={<Profile />} />

              <Route element={<div>
                <div>Unmatched route:</div>
                <pre>{JSON.stringify(window.location, null, 2)}</pre>
              </div>} />
            </Routes>
          </ActivityPane>

          <NavBarPane>
            <Player audioUrl="http://traffic.libsyn.com/joeroganexp/p1472.mp3" />
            <NavBar activeTab={activeTab} onTabClick={setActiveTab} />
          </NavBarPane>
        </Container>
      </ThemeProvider> {/*Theme, TODO: switch between*/}
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
