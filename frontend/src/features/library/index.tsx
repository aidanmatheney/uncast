import React, { FunctionComponent } from 'react';
import styled from 'styled-components';

import { LibraryRssPodcast } from '../../common/web-api';
import PodcastCard from './PodcastCard';

const Container = styled.div`
  background: #35363A;
  height: 100%;

  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(8rem, 1fr));
  align-content: start;
`;

const CardContainer = styled.div`
  margin: 0.5rem;
`;

const Library: FunctionComponent<{
  podcasts: LibraryRssPodcast[] | null;
}> = ({
  podcasts
}) => {
  return (
    <Container>
      {podcasts && podcasts.map(p => (
        <CardContainer key={p.id}>
          <PodcastCard podcast={p} />
        </CardContainer>
      ))}
    </Container>
  );
};

export default Library;
