import React, { FunctionComponent } from 'react';
import { useSelector } from 'react-redux';

import { RootState } from '../../app/rootReducer';
import PodcastGrid from '../podcast/PodcastGrid';


const Library: FunctionComponent = () => {
  const { subscriptions, rssPodcastById, filePodcastById } = useSelector((state: RootState) => state.podcast);
  console.log('Library subscriptions', subscriptions);

  return (<PodcastGrid podcasts={Object.keys(subscriptions).map(id => {
    return rssPodcastById[id] || filePodcastById[id] || null;
  }).filter(p => p != null)} />);
};

export default Library;
