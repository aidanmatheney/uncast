import React, { FunctionComponent } from 'react';
import styled from 'styled-components';

import { Episode } from '../../common/entities';
import EpisodeCard from './EpisodeCard';

const Container = styled.div`
  overflow: scroll;
  max-height: 50vh; /* TODO: more elegant solution */
`;

const CardContainer = styled.div``;

const EpisodeList: FunctionComponent<{
  episodes: Episode[];
}> = ({
  episodes
}) => {
  return (
    <Container>
      {episodes.map(episode => (
        <CardContainer key={episode.id}>
          <EpisodeCard episode={episode} />
        </CardContainer>
      ))}
    </Container>
  );
};
export default EpisodeList;
