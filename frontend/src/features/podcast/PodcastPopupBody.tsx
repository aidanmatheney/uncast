
import React, { FunctionComponent, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';

import { Podcast, Episode } from '../../common/entities';
import { RootState } from '../../app/rootReducer';
import { unsubscribeFromPodcast, subscribeToPodcast, refreshEpisodes } from './podcastSlice';
import EpisodeList from './EpisodeList';

import './audio.jpg';

const Container = styled.div`
  display: flex;
  flex-direction: column;
  background: ${props => props.theme.pageBackground};
  max-height: 90vh;
`;

const HeaderContainer = styled.div`
  display: flex;
  flex-direction: row;
`;

const HeaderImg = styled.img`
  width: 40%;
`;

const HeaderTextContainer = styled.div`
  padding: .5em;
`;

const TextContainer = styled.div`
  padding: .5em;
  display: flex;
  flex-direction: column;
  min-height: 0; /* Prevent overflow */
`;

const SubscribeButton = styled.button`
  background: ${props => props.theme.background};
  border-radius: 4px;

  margin-top: .5em;
`;

const PodcastPopupBody: FunctionComponent<{
  podcast: Podcast;
  onPlaybackRequested?(episode: Episode): void;
}> = ({
  podcast,
  onPlaybackRequested
}) => {
  const dispatch = useDispatch();

  const subscribed = useSelector((state: RootState) => state.podcast.userPodcastStateById[podcast.id]?.subscribed ?? false);
  const episodes = useSelector((state: RootState) => state.podcast.episodesByPodcastId[podcast.id] ?? []);

  function defaultCard () {
    return (
      <HeaderImg src="https://i.imgur.com/WwIPeBK.jpg" alt={podcast.name} title={podcast.name} />
    )
  }

  function imageCard () {
    return (
      <HeaderImg src={podcast.thumbnailUrl} alt={podcast.name} title={podcast.name} />
    )
  }

  useEffect(() => {
    dispatch(refreshEpisodes({ podcastId: podcast.id }));
  }, [podcast.id, dispatch]);

  return (
    <Container>
      <HeaderContainer> 
        {
          podcast.thumbnailUrl == null && defaultCard()
        }
        {
          podcast.thumbnailUrl != null && imageCard()
        }
        <HeaderTextContainer>
          <div style={{ fontWeight: 'bold', fontSize: '1.2em' }}>{podcast.name}</div>
          <div style={{ fontSize: '.8em' }}>by {podcast.author}</div>
          {podcast.url && (<div style={{ fontSize: '.8em' }}><a href={podcast.url} target="_blank" rel="noopener noreferrer">Visit website</a></div>)}
        </HeaderTextContainer>
      </HeaderContainer>
      <TextContainer>
        {podcast.description && <div style={{ fontStyle: 'italic', fontSize: '.9em', marginBottom: '.5em' }}>{podcast.description}</div>}
        <EpisodeList episodes={episodes} onPlaybackRequested={onPlaybackRequested} />
        <div>
          <SubscribeButton type="button" onClick={() => dispatch((subscribed ? unsubscribeFromPodcast : subscribeToPodcast)({ id: podcast.id }))}>
            {subscribed ? 'Unsubscribe' : 'Subscribe'}
          </SubscribeButton>
        </div>
      </TextContainer>
    </Container>
  );
};
export default PodcastPopupBody;
