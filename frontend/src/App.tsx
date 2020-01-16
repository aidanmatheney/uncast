import React, { FunctionComponent } from 'react';
import logo from './logo.svg';
import './App.css';

import { RestfulProvider } from 'restful-react';

import { useGetLibraryRssPodcasts } from './api';
import PodcastGrid from './PodcastGrid';

const App: FunctionComponent = () => {
  const { data: podcasts, loading, error } = useGetLibraryRssPodcasts({ });

  return (
    <div className="App">
      {podcasts && <PodcastGrid podcasts={podcasts} />}
      {loading && 'Podcast grid loading'}
      {error && `Error fetching podcasts: ${JSON.stringify(error)}`}

      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
};

const WrappedApp: FunctionComponent = props => (
  <RestfulProvider base="https://localhost:5001">
    <App {...props} />
  </RestfulProvider>
);

export default WrappedApp;
