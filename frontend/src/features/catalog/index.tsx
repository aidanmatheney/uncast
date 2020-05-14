import React, { FunctionComponent } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';

import { RootState } from '../../app/rootReducer';
import PodcastGrid from '../podcast/PodcastGrid';
import { AddStreamMenu } from '../addStream';

const Container = styled.div``;

const Catalog: FunctionComponent = () => {
  const { podcastState: catalog, podcastById } = useSelector((state: RootState) => state.podcast);

  return (
    <Container>
      <PodcastGrid podcasts={Object.keys(catalog).map(id => podcastById[id]).filter(p => p != null)} />
      <AddStreamMenu />
    </Container>
  );
};

export default Catalog;
