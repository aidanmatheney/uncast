import React, { FunctionComponent } from 'react';
import { useSelector } from 'react-redux';

import { RootState } from '../../app/rootReducer';
import PodcastGrid from '../podcast/PodcastGrid';
import { Episode } from '../../common/entities';


const Library: FunctionComponent<{
  onPlaybackRequested(episode: Episode): void;
}> = ({
  onPlaybackRequested
}) => {
  const podcastById = useSelector((state: RootState) => state.podcast.podcastById);
  const userPodcastStateById = useSelector((state: RootState) => state.podcast.userPodcastStateById);
  const subscribedPodcasts = Object.entries(userPodcastStateById).filter(([_, state]) => state.subscribed).map(([id]) => podcastById[id]);

  return (<PodcastGrid podcasts={subscribedPodcasts} onPlaybackRequested={onPlaybackRequested} />);
};
export default Library;
