
import React, { FunctionComponent, useMemo, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';

import { Podcast, Episode } from '../../common/entities';
import { RootState } from '../../app/rootReducer';
import { unsubscribeFromPodcast, subscribeToPodcast, refreshEpisodes } from './podcastSlice';
import EpisodeList from './EpisodeList';

const Container = styled.div`
  display: flex;
  flex-direction: column;
  background: ${props => props.theme.pageBackground};
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

  const subscribed = useSelector((state: RootState) => state.podcast.userPodcastStateById[podcast.id]?.subscribed || false);
  const episodes: Episode[] | undefined = useSelector((state: RootState) => state.podcast.episodesByPodcastId[podcast.id]);

  useEffect(() => {
    dispatch(refreshEpisodes({ podcastId: podcast.id }));
  }, [podcast.id]);

  return (
    <Container>
      <HeaderContainer>
        <HeaderImg src={podcast.thumbnailUrl} alt={podcast.name} title={podcast.name} />
        <HeaderTextContainer>
          <div style={{ fontWeight: 'bold', fontSize: '1.2em' }}>{podcast.name}</div>
          <div style={{ fontSize: '0.8em' }}>by {podcast.author}</div>
          {podcast.url && (<div style={{ fontSize: '0.8em' }}><a href={podcast.url} target="_blank" rel="noopener noreferrer">Visit website</a></div>)}
        </HeaderTextContainer>
      </HeaderContainer>
      <TextContainer>
        {podcast.description && <div style={{ fontStyle: 'italic' }}>{podcast.description}</div>}
        <div>{<EpisodeList episodes={episodes || []} onPlaybackRequested={onPlaybackRequested} />}</div>
        <div>
          <SubscribeButton type="button" onClick={() => {
            if (subscribed) {
              dispatch(unsubscribeFromPodcast({ id: podcast.id }));
            } else {
              dispatch(subscribeToPodcast({ id: podcast.id }));
            }
          }}>
            {subscribed ? 'Unsubscribe' : 'Subscribe'}
          </SubscribeButton>
        </div>
      </TextContainer>
    </Container>
  );
};
export default PodcastPopupBody;
