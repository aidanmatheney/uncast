import React, { FunctionComponent } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';

import PodcastCard from './PodcastCard';
import { RootState } from '../../app/rootReducer';

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

const Library: FunctionComponent = () => {
  const subscriptions = useSelector((state: RootState) => state.user.subscriptions);

  console.log('Library subscriptions', subscriptions)

  return (
    <Container>
      {subscriptions && Object.values(subscriptions).map(p => (
        <CardContainer key={p.name}>
          <PodcastCard podcast={p} />
        </CardContainer>
      ))}
    </Container>
  );
};

export default Library;
