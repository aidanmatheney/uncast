import React, { FunctionComponent } from 'react';
import styled from 'styled-components';

import { Episode } from '../../common/entities';

const Container = styled.div``;

const EpisodeCard: FunctionComponent<{
  episode: Episode;
}> = ({
  episode
}) => {

  return (
    <Container>
      <pre>EpisodeCard: {JSON.stringify(episode)}</pre>
    </Container>
  );
};
export default EpisodeCard;
