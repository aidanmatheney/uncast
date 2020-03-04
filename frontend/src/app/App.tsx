import React, { FunctionComponent, useState, useEffect } from 'react';
import { Provider as ReduxProvider, useSelector, useDispatch } from 'react-redux';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import { RestfulProvider } from 'restful-react';
import styled from 'styled-components/macro';

import createStore, { history } from './createStore';
import { useGetLibraryRssPodcasts } from '../common/web-api';
import { useAuthenticatedRequestOptions } from '../common/hooks';
import { loadUser } from '../features/authentication/authenticationSlice';

import AuthenticationMenu from '../features/authentication/AuthenticationMenu';
import AuthenticationCallbacks from '../features/authentication/AuthenticationCallbacks';
import Library from '../features/library';
import Catalog from '../features/catalog';
import Profile from '../features/profile';
import NavBar from '../features/navbar';
import TabId from '../common/TabId';
import { RootState } from './createRootReducer';

const Container = styled.div`
  height: 100vh;
  display: flex;
  flex-direction: column;
`;

const ActivityPane = styled.div`
  flex-grow: 1;
`;

const NavBarPane = styled.div``;

const store = createStore();

const App: FunctionComponent = () => {
  const dispatch = useDispatch();

  const [activeTab, setActiveTab] = useState<TabId>(TabId.Library);

  const authentication = useSelector((state: RootState) => state.authentication);
  const requestOptions = useAuthenticatedRequestOptions();
  const { data: podcasts, loading, error } = useGetLibraryRssPodcasts({ requestOptions });

  useEffect(() => {
    // Try to load the user from storage when the app is mounted
    dispatch(loadUser());
  }, []);

  if (error) {
    console.error('useGetLibraryRssPodcasts error:', error);
  }

  return (
    <Container>
      <ActivityPane>
        <Switch>
          <Route exact path="/" render={() => (
            <Library podcasts={podcasts} />
            /*
              {loading && 'Podcast grid loading'}
              {error && `Error fetching podcasts: ${JSON.stringify(error)}`}
            */
          )} />
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
      </ActivityPane>

      <NavBarPane>
        <AuthenticationMenu />
        <NavBar activeTab={activeTab} onTabClick={setActiveTab} />
      </NavBarPane>
    </Container>
  );
};

const WrappedApp: typeof App = props => (
  <ReduxProvider store={store}>
    <ConnectedRouter history={history}>
      <BrowserRouter>
        <RestfulProvider
          base="https://localhost:5001"
          onError={(err, retry, response) => {
            debugger;
            console.error('REST error:', { err, retry, response });
          }}
        >
          <AuthenticationCallbacks>
            <App {...props} />
          </AuthenticationCallbacks>
        </RestfulProvider>
      </BrowserRouter>
    </ConnectedRouter>
  </ReduxProvider>
);

export default WrappedApp;
