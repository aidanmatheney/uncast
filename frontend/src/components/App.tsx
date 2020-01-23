import React, { FunctionComponent, useState } from 'react';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import styled from 'styled-components/macro';

import { RestfulProvider } from 'restful-react';

import { useGetLibraryRssPodcasts } from '../api';

import Library from './Library';
import Catalog from './Catalog';
import Profile from './Profile';
import NavBar from './NavBar';

import TabId from '../TabId';

const Container = styled.div`
  height: 100vh;
  display: flex;
  flex-direction: column;
`;

const ActivityPane = styled.div`
  flex-grow: 1;
`;

const NavBarPane = styled.div``;


const App: FunctionComponent = () => {
  const [activeTab, setActiveTab] = useState<TabId>(TabId.Library);

  const { data: podcasts, loading, error } = useGetLibraryRssPodcasts({ });

  return (
    <Container>
      <ActivityPane>
        <BrowserRouter>
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
          </Switch>
        </BrowserRouter>
      </ActivityPane>

      <NavBarPane>
        <NavBar activeTab={activeTab} onTabClick={setActiveTab} />
      </NavBarPane>
    </Container>
  );
}

const WrappedApp: FunctionComponent = props => (
  <RestfulProvider base="https://localhost:5001">
    <App {...props} />
  </RestfulProvider>
);

export default WrappedApp;
