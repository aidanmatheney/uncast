import React, { FunctionComponent } from 'react';
import styled from 'styled-components/macro';
import Popup from 'reactjs-popup';

import { Podcast, Episode }  from '../../common/entities';
import PodcastPopupBody from './PodcastPopupBody';

const Container = styled.div``;

const CardImg = styled.img`
  max-width: 100%;
  max-height: 100%;

  border-radius: 4px;
  cursor: pointer;
`;

const PodcastCard: FunctionComponent<{
  podcast: Podcast;
  onPlaybackRequested?(episode: Episode): void;
}> = ({
  podcast,
  onPlaybackRequested
}) => {
  return (
    <Container>
      <Popup trigger={<CardImg src={podcast.thumbnailUrl} title={podcast.name} alt={podcast.name} />} modal closeOnDocumentClick>
        <PodcastPopupBody podcast={podcast} onPlaybackRequested={onPlaybackRequested} />
      </Popup>
    </Container>
  );
};

export default PodcastCard;
