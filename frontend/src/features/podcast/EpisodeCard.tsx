import React, { FunctionComponent } from 'react';
import styled from 'styled-components';
import { FaPlus } from 'react-icons/fa';
import { IconType } from 'react-icons/lib';

import { Episode } from '../../common/entities';
import { formatSeconds, formatUnixDate } from '../../common/utils';

const Container = styled.div`
  margin: 0.25rem;
  color: ${props => props.theme.color};
  font-size: 1em;
  text-align: left;
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
  background: ${props => props.theme.background};
`;

const EpisodeActivityContainer = styled.div`
  margin: 0.25rem;
  color: ${props => props.theme.color};
  font-size: 1em;
  text-align: right;
  background: transparent;
`;

const EpisodeAddButton = styled.button`
  margin: 0.25rem;
  color: ${props => props.theme.color};
  cursor: pointer;
  font-size: 1em;
  text-align: center;
  border-radius: 3px;
  background: ${props => props.theme.pageBackground};
`;

const tabs: {
  id: string;
  name: string;
  Icon: IconType;
}[] = [
  {
    id: 'Play Episode',
    name: 'Play Episode',
    Icon: FaPlus
  }
];

const IconContainer = styled.div``;
const TextContainer = styled.div``;

const EpisodeCard: FunctionComponent<{
  episode: Episode;
  onPlaybackRequested?(episode: Episode): void;
}> = ({
  episode,
  onPlaybackRequested
}) => {
  const duration = formatSeconds(episode.durationS);
  const date = episode.dateUnix ? formatUnixDate(episode.dateUnix) : null;

  return (
    <Container>
      <div>{episode.name}</div>
      <div><i>{duration}</i></div>
      {date && (<div>Released: {date}</div>)}
      <EpisodeActivityContainer>
        {tabs.map(({ id: tabId, name: tabName, Icon }) => (
          <div key={tabId}>
            <EpisodeAddButton key={tabId} onClick={() => onPlaybackRequested?.(episode)}>
              <IconContainer><Icon /></IconContainer>
              <TextContainer>{tabName}</TextContainer>
            </EpisodeAddButton>
          </div>
        ))}
      </EpisodeActivityContainer>
    </Container>
  );
};
export default EpisodeCard;
