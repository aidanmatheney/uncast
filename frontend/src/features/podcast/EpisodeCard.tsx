import React, { FunctionComponent, useState } from 'react';
import styled from 'styled-components';
import { FaPlay, FaStar } from 'react-icons/fa';
import { IconType } from 'react-icons/lib';

import Truncate from 'react-truncate';

import { Episode } from '../../common/entities';
import { formatSeconds, formatUnixDate } from '../../common/utils';
import { useDispatch, useSelector } from 'react-redux';
import { setEpisodeFavorite } from './podcastSlice';
import { RootState } from '../../app/rootReducer';

const Container = styled.div`
  margin: .25rem;
  padding: .25rem;

  color: ${props => props.theme.color};
  font-size: 1em;
  text-align: left;
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
  background: ${props => props.theme.background};
`;

const HeaderContainer = styled.div`
  display: flex;
  flex-direction: column;
`;

const HeaderTitleContainer = styled.div`
  font-weight: bold;
`;

const HeaderInfoContainer = styled.div`
  display: flex;
  flex-direction: row;

  justify-content: space-between;

  font-weight: lighter;
  font-size: .8em;
`;

const DetailsContainer = styled.div`
  font-size: .8em;

  margin-top: .25rem;
  margin-bottom: .25rem;
`;

const ActionsContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
`;

const ActionButton = styled.button`
  margin: 0.25rem;
  color: ${props => props.theme.color};
  cursor: pointer;
  font-size: 1em;
  text-align: center;
  border-radius: 3px;
  background: ${props => props.theme.pageBackground};
`;

const EpisodeCard: FunctionComponent<{
  episode: Episode;
  onPlaybackRequested?(episode: Episode): void;
}> = ({
  episode,
  onPlaybackRequested
}) => {
  const dispatch = useDispatch();

  const isFavorite = useSelector((state: RootState) => state.podcast.userEpisodeStateById[episode.id]?.favorite ?? false);

  const duration = formatSeconds(episode.durationS ?? 0);
  const date = episode.dateUnix ? formatUnixDate(episode.dateUnix) : null;

  const [truncateDescription, setTruncateDescription] = useState(true);

  const actions: {
    id: string;
    title: string;
    invoke(): void;
    Icon: IconType;
  }[] = [
    {
      id: 'favorite',
      title: isFavorite ? 'Unfavorite' : 'Favorite',
      invoke: () => dispatch(setEpisodeFavorite({ id: episode.id, favorite: !isFavorite })),
      Icon: FaStar
    },
    {
      id: 'play',
      title: 'Play',
      invoke: () => onPlaybackRequested?.(episode),
      Icon: FaPlay
    }
  ];

  return (
    <Container>
      <HeaderContainer>
        <HeaderTitleContainer>{episode.name}</HeaderTitleContainer>
        <HeaderInfoContainer>
          <div>{date}</div>
          <div>{duration}</div>
        </HeaderInfoContainer>
      </HeaderContainer>
      <DetailsContainer title={episode.description}>
        {episode.description
          ? (
            <Truncate
              lines={truncateDescription ? 3 : false}
              ellipsis={<span title="Show more" onClick={() => setTruncateDescription(false)} style={{ cursor: 'pointer' }}>…</span>}
            >
              <span style={{ whiteSpace: 'pre-wrap' }}>{episode.description}</span>
            </Truncate>
          ) : (
            <div style={{ fontStyle: 'italic' }}>
              No episode notes.
            </div>
          )}
      </DetailsContainer>
      <ActionsContainer>
        {actions.map(({ id, title, invoke, Icon }) => (
          <ActionButton key={id} title={title} onClick={invoke}><Icon /></ActionButton>
        ))}
      </ActionsContainer>
    </Container>
  );
};
export default EpisodeCard;
