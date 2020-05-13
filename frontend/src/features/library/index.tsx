import React, { FunctionComponent } from 'react';
import { useSelector } from 'react-redux';

import { RootState } from '../../app/rootReducer';
import PodcastGrid from '../podcast/PodcastGrid';


const Library: FunctionComponent = () => {
  const subscriptions = useSelector((state: RootState) => state.user.subscriptions);
  console.log('Library subscriptions', subscriptions);

  return (<PodcastGrid podcasts={Object.values(subscriptions)} />);
};

export default Library;
