import React, { FunctionComponent } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';

import { RootState } from '../../app/rootReducer';
import PodcastGrid from '../podcast/PodcastGrid';
import { AddStreamMenu } from '../addStream';
import { Episode } from '../../common/entities';

const Container = styled.div``;

const Catalog: FunctionComponent<{
  onPlaybackRequested(episode: Episode): void;
}> = ({
  onPlaybackRequested
}) => {
  const podcastById = useSelector((state: RootState) => state.podcast.podcastById);
  const podcastState = useSelector((state: RootState) => state.podcast.podcastState);
  const catalogPodcasts = Object.entries(podcastState).filter(([_, state]) => state.catalog).map(([id]) => podcastById[id]);

  return (
    <Container>
      <PodcastGrid podcasts={catalogPodcasts} onPlaybackRequested={onPlaybackRequested} />
      <AddStreamMenu />
    </Container>
  );
};
export default Catalog;
