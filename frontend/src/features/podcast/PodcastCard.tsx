import React, { FunctionComponent } from 'react';
import styled from 'styled-components/macro';
import Popup from 'reactjs-popup';

import { Podcast, Episode }  from '../../common/entities';
import PodcastPopupBody from './PodcastPopupBody';

import './audio.jpg';

const Container = styled.div``;

const CardImg = styled.img`
  max-width: 100%;
  max-height: 100%;

  border-radius: 4px;
  cursor: pointer;
`;

const CardImgContainer = styled.div`
  max-width: 100%;
  max-height: 100%;

  border-radius: 4px;
  cursor: pointer;
  color: ${props => props.theme.color};
  background: ${props => props.theme.pageBackground};
`;

const PodcastCard: FunctionComponent<{
  podcast: Podcast;
  onPlaybackRequested?(episode: Episode): void;
}> = ({
  podcast,
  onPlaybackRequested
}) => {

  function defaultCard () {
    return (
      <Popup trigger={
        <CardImgContainer>
          <CardImg src="https://i.imgur.com/WwIPeBK.jpg" title={podcast.name} alt={podcast.name} />
          {podcast.name}
        </CardImgContainer>
        } modal closeOnDocumentClick>
        <PodcastPopupBody podcast={podcast} onPlaybackRequested={onPlaybackRequested} />
      </Popup>
    )
  }
  
  function imageCard () {
    return (
      <Popup trigger={
        <CardImgContainer>
          <CardImg src={podcast.thumbnailUrl} title={podcast.name} alt={podcast.name} />
          {podcast.name}
        </CardImgContainer>  
        } modal closeOnDocumentClick>
        <PodcastPopupBody podcast={podcast} onPlaybackRequested={onPlaybackRequested} />
      </Popup>
    )
  }

  return (
    <Container>
      {
        podcast.thumbnailUrl == null && defaultCard()
      }
      {
        podcast.thumbnailUrl != null && imageCard()
      }
    </Container>
  );
};

export default PodcastCard;
