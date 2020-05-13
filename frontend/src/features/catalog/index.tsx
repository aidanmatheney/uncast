import React, { FunctionComponent } from 'react';

import catalogPodcasts from './catalogPodcasts';
import PodcastGrid from '../podcast/PodcastGrid';
import styled from 'styled-components';
import { AddStreamMenu } from '../addStream';

const Container = styled.div``;

const Catalog: FunctionComponent = () => {
  return (
    <Container>
      <PodcastGrid podcasts={catalogPodcasts} />
      <AddStreamMenu />
    </Container>
  );
};

export default Catalog;
