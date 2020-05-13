import React, { FunctionComponent } from 'react';

import catalogPodcasts from './catalogPodcasts';
import PodcastGrid from '../podcast/PodcastGrid';

const Catalog: FunctionComponent = () => {
  return (<PodcastGrid podcasts={catalogPodcasts} />);
};

export default Catalog;
