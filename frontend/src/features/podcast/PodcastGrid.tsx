import React, { FunctionComponent } from 'react';
import styled from 'styled-components';

import { Podcast } from '../../common/entities';
import PodcastCard from './PodcastCard';

const Container = styled.div`
  background: ${props => props.theme.pgBgColor};
  color: ${props => props.theme.color};
  height: 100%;

  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(8rem, 1fr));
  align-content: start;
`;

const CardContainer = styled.div`
  margin: 0.5rem;
`;

const PodcastGrid: FunctionComponent<{
  podcasts: Podcast[];
}> = ({
  podcasts
}) => {
  return (
    <Container>
      {podcasts.map(p => (
        <CardContainer key={p.name}>
          <PodcastCard podcast={p} />
        </CardContainer>
      ))}
    </Container>
  );
};
export default PodcastGrid;
