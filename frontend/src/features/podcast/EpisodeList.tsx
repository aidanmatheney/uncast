import React, { FunctionComponent } from 'react';
import styled from 'styled-components';

import { Episode } from '../../common/entities';
import EpisodeCard from './EpisodeCard';

const Container = styled.div`
  overflow-y: auto;
`;

const CardContainer = styled.div``;

const EpisodeList: FunctionComponent<{
  episodes: Episode[];
  onPlaybackRequested?(episode: Episode): void;
}> = ({
  episodes,
  onPlaybackRequested
}) => {
  return (
    <Container>
      {episodes.map(episode => (
        <CardContainer key={episode.id}>
          <EpisodeCard episode={episode} onPlaybackRequested={onPlaybackRequested} />
        </CardContainer>
      ))}
    </Container>
  );
};
export default EpisodeList;
