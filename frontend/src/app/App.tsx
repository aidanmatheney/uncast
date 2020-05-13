import React, { Component, FunctionComponent, useState, useEffect } from 'react';
import AudioPlayer from 'react-h5-audio-player';
import ReactAudioPlayer from 'react-audio-player';
import 'react-h5-audio-player/lib/styles.css';
import cookie, { CookiesProvider, useCookies } from 'react-cookie';
import Cookies from 'js-cookie';

import { Provider as ReduxProvider, useSelector, useDispatch } from 'react-redux';
import { BrowserRouter, Switch, Route, HashRouter, NavLink, Link } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import styled, { css, ThemeProvider } from 'styled-components';
import createStore, { history } from './createStore';
import { loadFromStore, saveToStore } from '../features/user/userSlice';

import AddStreamMenu from '../features/addstream';
import Library from '../features/library';
import Catalog from '../features/catalog';
import Profile from '../features/profile';
import NavBar from '../features/navbar';
import TabId from '../common/TabId';
import { RootState } from './createRootReducer';

import { ThemeStandard, ThemeLight, ThemeDark } from "../features/theme/Theme";
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
const Player = (player: any) => {
  const [cookies, setCookie] = useCookies(['playback']);
  player = React.createRef();
  var loadFromCookie = false;
  return (
    <div>
      <AudioPlayer
        ref={player}
        src="http://traffic.libsyn.com/joeroganexp/p1472.mp3"  //just an example for testing, replace with any audio
        onListen={e => setCookie('playback', Math.floor(player.current.audio.current.currentTime), { path: '/' } )}
        onCanPlay={e => 
          {if (loadFromCookie === false) {
            player.current.audio.current.currentTime = Cookies.get('playback');
            loadFromCookie = true;
          }
        }}
      />
    </div>
  )
};

const Button = styled.button`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;
  background: ${props => props.theme.background};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
`;

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

  const [themeState, setTheme] = useState(ThemeStandard);

  const themeMenu = (
    <ThemeButtonMenu>
      <ThemeButton onClick={() => setTheme(ThemeStandard)}>
        Standard Theme
      </ThemeButton>
      <ThemeButton onClick={() => setTheme(ThemeLight)}>
        Light Theme
      </ThemeButton>
      <ThemeButton onClick={() => setTheme(ThemeDark)}>
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
      <ThemeProvider theme={themeState}> {/*Theme, TODO: switch between*/}
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
            <Player />
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
