import React, { FunctionComponent } from 'react';
import { useSelector } from 'react-redux';

import { RootState } from '../../app/rootReducer';
import PodcastGrid from '../podcast/PodcastGrid';


const Library: FunctionComponent = () => {
  const { userPodcastStateById, podcastById } = useSelector((state: RootState) => state.podcast);

  return (<PodcastGrid podcasts={Object.entries(userPodcastStateById).filter(([id, state]) => state.subscribed).map(([id]) => {
    return podcastById[id] || null;
  }).filter(p => p != null)} />);
};

export default Library;
