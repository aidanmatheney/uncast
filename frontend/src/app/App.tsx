import React, { FunctionComponent, useState, useEffect, useRef } from 'react';
import AudioPlayer from 'react-h5-audio-player';
import 'react-h5-audio-player/lib/styles.css';

import { Provider as ReduxProvider, useDispatch } from 'react-redux';
import { BrowserRouter, Switch, Route, HashRouter } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import styled, { ThemeProvider } from 'styled-components';
import createStore, { history } from './createStore';
import { loadFromStore, saveToStore } from '../features/user/userSlice';

import { AddStreamMenu } from '../features/addstream';
import Library from '../features/library';
import Catalog from '../features/catalog';
import Profile from '../features/profile';
import NavBar from '../features/navbar';
import TabId from '../common/TabId';

import { standardTheme, lightTheme, darkTheme } from "../features/theme";
import { useInterval } from '../common/hooks';

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
const Player: FunctionComponent<{
  audioUrl: string;

  startTime?: number;
  onTimeChanged?(time: number): void;
}> = ({
  audioUrl,

  startTime,
  onTimeChanged
}) => {
  const playerRef = useRef<AudioPlayer>(null);
  const player = playerRef.current;
  const audio = player?.audio.current;

  const [loadedStartTime, setLoadedStartTime] = useState<boolean>(false);

  return (
    <div>
      <AudioPlayer
        ref={playerRef}
        src={audioUrl}

        onListen={() => onTimeChanged?.(audio!.currentTime)}

        onCanPlay={() => {
          if (startTime != null && !loadedStartTime) {
            audio!.currentTime = startTime;
            setLoadedStartTime(true);
          }
        }}
      />
    </div>
  )
};

// const Button = styled.button`
//   text-align: center;
//   margin: 0.25rem;
//   cursor: pointer;
//   background: ${props => props.theme.background};
//   color: ${props => props.theme.color};
//   border: 2px solid ${props => props.theme.borderColor};
//   border-radius: 3px;
// `;

const ThemeButton = styled.button`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;
  background: ${props => props.theme.background};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
`;

const ThemeButtonMenu = styled.button`
  text-align: center;
  margin: 0.25rem;
  background: ${props => props.theme.pageBackground};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
`;

const NavBarPane = styled.div``;

export const store = createStore();

const App: FunctionComponent = () => {
  const dispatch = useDispatch();

  const [activeTab, setActiveTab] = useState<TabId>(TabId.Library);

  const [theme, setTheme] = useState(standardTheme);

  const themeMenu = (
    <ThemeButtonMenu>
      <ThemeButton onClick={() => setTheme(standardTheme)}>
        Standard Theme
      </ThemeButton>
      <ThemeButton onClick={() => setTheme(lightTheme)}>
        Light Theme
      </ThemeButton>
      <ThemeButton onClick={() => setTheme(darkTheme)}>
        Dark Theme
      </ThemeButton>
    </ThemeButtonMenu>
  );

  useEffect(() => {
    // Try to load the user from storage when the app is mounted
    dispatch(loadFromStore());
  }, [dispatch]);

  useInterval(() => {
    dispatch(saveToStore());
  }, 5000);

  return (
    <Wrapper>
      <ThemeProvider theme={theme}> {/*Theme, TODO: switch between*/}
        <Container>
          <ActivityPane>
            <HashRouter>
              <Switch>
                <Route exact path="/" component={Library} />
                <Route exact path="/Library" component={Library} />
                <Route exact path="/catalog" component={Catalog} />
                <Route exact path="/profile" component={Profile} />

                <Route render={props => {
                  return (
                    <div>
                      <div>Unmatched route:</div>
                      <pre>{JSON.stringify(props, null, 2)}</pre>
                    </div>
                  );
                }} />
              </Switch>
            </HashRouter>

          </ActivityPane>

          <NavBarPane>
            {themeMenu}
            <Player audioUrl="http://traffic.libsyn.com/joeroganexp/p1472.mp3" />
            <AddStreamMenu />
            <NavBar activeTab={activeTab} onTabClick={setActiveTab} />
          </NavBarPane>
        </Container>
      </ThemeProvider> {/*Theme, TODO: switch between*/}
    </Wrapper>
  );
};

const WrappedApp: typeof App = props => (
  <ReduxProvider store={store}>
    <ConnectedRouter history={history}>
      <BrowserRouter>
        <App {...props} />
      </BrowserRouter>
    </ConnectedRouter>
  </ReduxProvider>
);

export default WrappedApp;
