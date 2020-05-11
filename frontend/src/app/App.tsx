import React, { Component, FunctionComponent, useState, useEffect } from 'react';
import AudioPlayer from 'react-h5-audio-player';
import 'react-h5-audio-player/lib/styles.css';

import { Provider as ReduxProvider, useSelector, useDispatch } from 'react-redux';
import { BrowserRouter, Switch, Route, HashRouter, NavLink, Link } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import styled, { css, ThemeProvider } from 'styled-components';
import createStore, { history } from './createStore';
import { loadUser } from '../features/authentication/authenticationSlice';

import AddStreamMenu from '../features/addstream';
import AuthenticationMenu from '../features/authentication/AuthenticationMenu';
import AuthenticationCallbacks from '../features/authentication/AuthenticationCallbacks';
import Library from '../features/library';
import Catalog from '../features/catalog';
import Profile from '../features/profile';
import NavBar from '../features/navbar';
import TabId from '../common/TabId';
import { RootState } from './createRootReducer';
import AdminDashboard from '../features/admin/AdminDashboard';

import { ThemeStandard, ThemeLight, ThemeDark } from "../features/theme/Theme";

const Wrapper = styled.section`
  padding: 3em,
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
const Player = () => (
  <AudioPlayer
    src="https://play.podtrac.com/npr-510306/edge1.pod.npr.org/anon.npr-mp3/npr/asc/2017/10/20171024_asc_japanesebreakfastpodcast.mp3"  //just an example for testing, replace with any audio
    onPlay={e => console.log("onPlay")}
  />
);

const Button = styled.button.attrs(props => ({
  
}))`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;  
  background: ${props => props.theme.background};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
`;

const ThemeButton = styled.button.attrs(props => ({
}))`
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

  const user = useSelector((state: RootState) => state.authentication.user);

  const [themeState, setTheme] = useState(ThemeStandard);

  const ThemeMenu = () => {
    return (
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
    )
  }
  
  useEffect(() => {
    // Try to load the user from storage when the app is mounted
    dispatch(loadUser());
  }, [dispatch]);

  return (
    <Wrapper>
    <ThemeProvider theme={themeState}> {/*Theme, TODO: switch between*/}
      <Container>
        <ActivityPane>
          <HashRouter>
          {user && (<Switch>
            <Route exact path="/" component={Library} />
            <Route exact path="/Library" component={Library} />
            <Route exact path="/catalog" component={Catalog} />
            <Route exact path="/profile" component={Profile} />

            <Route path="/admin" component={AdminDashboard} />

            <Route render={props => {
              return (
                <div>
                  <div>Unmatched route:</div>
                  <pre>{JSON.stringify(props, null, 2)}</pre>
                </div>
              );
            }} />
          </Switch>)}
          </HashRouter>
          
        </ActivityPane>

        <NavBarPane>
          <ThemeMenu />
          <Player />
          <AuthenticationMenu />
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
        <AuthenticationCallbacks>
          <App {...props} />
        </AuthenticationCallbacks>
      </BrowserRouter>
    </ConnectedRouter>
  </ReduxProvider>
);

export default WrappedApp;