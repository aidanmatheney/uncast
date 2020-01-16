import React, { FunctionComponent } from 'react';
import styled from 'styled-components/macro';

import { LibraryRssPodcast } from './api';
import PodcastCard from './PodcastCard';

const Pane = styled.div`
  display: flex;
  flex-direction: row;

  margin: -0.5rem;
`;

const CardContainer = styled.div`
  margin: 0.5rem;
`;

const PodcastGrid: FunctionComponent<{
  podcasts: LibraryRssPodcast[];
}> = ({
  podcasts
}) => {
  return (
    <Pane>
      {podcasts.map(p => (
        <CardContainer key={p.id}>
          <PodcastCard podcast={p} />
        </CardContainer>
      ))}
    </Pane>
  );
};

export default PodcastGrid
