import React, { FunctionComponent, useState, useEffect } from 'react';
import { Provider as ReduxProvider, useSelector, useDispatch } from 'react-redux';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import styled from 'styled-components/macro';

import createStore, { history } from './createStore';
import { loadUser } from '../features/authentication/authenticationSlice';

import AuthenticationMenu from '../features/authentication/AuthenticationMenu';
import AuthenticationCallbacks from '../features/authentication/AuthenticationCallbacks';
import Library from '../features/library';
import Catalog from '../features/catalog';
import Profile from '../features/profile';
import NavBar from '../features/navbar';
import TabId from '../common/TabId';
import AddStreamMenu from '../features/addstream';
import { RootState } from './createRootReducer';
import AdminDashboard from '../features/admin/AdminDashboard';

const Container = styled.div`
  height: 100vh;
  display: flex;
  flex-direction: column;
`;

const ActivityPane = styled.div`
  flex-grow: 1;
`;

const NavBarPane = styled.div``;

export const store = createStore();

const App: FunctionComponent = () => {
  const dispatch = useDispatch();

  const [activeTab, setActiveTab] = useState<TabId>(TabId.Library);

  const user = useSelector((state: RootState) => state.authentication.user);

  useEffect(() => {
    // Try to load the user from storage when the app is mounted
    dispatch(loadUser());
  }, [dispatch]);

  return (
    <Container>
      <ActivityPane>
        {user && (<Switch>
          <Route exact path="/" component={Library} />
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
      </ActivityPane>

      <NavBarPane>
        <AuthenticationMenu />
        <AddStreamMenu />
        <NavBar activeTab={activeTab} onTabClick={setActiveTab} />
      </NavBarPane>
    </Container>
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
