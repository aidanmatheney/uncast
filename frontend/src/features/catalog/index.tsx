import React, { FunctionComponent, useEffect } from 'react';

import catalogPodcastFeeds from './catalogPodcastFeeds';
import PodcastGrid from '../podcast/PodcastGrid';
import styled from 'styled-components';
import { AddStreamMenu } from '../addStream';
import { useDispatch, useSelector } from 'react-redux';
import { catalogRssPodcast } from '../podcast/podcastSlice';
import { RootState } from '../../app/rootReducer';
import { createSelector } from '@reduxjs/toolkit';

const Container = styled.div``;

const Catalog: FunctionComponent = () => {
  const { catalog, rssPodcastById } = useSelector((state: RootState) => state.podcast);


  return (
    <Container>
      <PodcastGrid podcasts={Object.keys(catalog).map(id => rssPodcastById[id]).filter(p => p != null)} />
      <AddStreamMenu />
    </Container>
  );
};

export default Catalog;
