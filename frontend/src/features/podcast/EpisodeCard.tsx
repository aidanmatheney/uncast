import React, { FunctionComponent } from 'react';
import styled from 'styled-components';
import { FaPlus } from 'react-icons/fa';
import { IconType } from 'react-icons/lib';

import { Episode } from '../../common/entities';

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
  font-size: 1em;
  text-align: center;
  border-radius: 3px;
  background: ${props => props.theme.pageBackground};
`;

const EpisodeTab: {
  tab: string;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: 'Play Episode',
    name: 'Play Episode',
    Icon: FaPlus
  }
];

const IconContainer = styled.div``;
const TextContainer = styled.div``;

const EpisodeActivities: FunctionComponent = () => {
  return (
    <EpisodeActivityContainer>
      {EpisodeTab.map(({ tab, name, Icon }) => (
        <div>
          <EpisodeAddButton key={tab} >
            <IconContainer><Icon /></IconContainer>
            <TextContainer>{name}</TextContainer>
          </EpisodeAddButton>
        </div>
      ))}
    </EpisodeActivityContainer>
  )
}


const EpisodeCard: FunctionComponent<{
  episode: Episode;
}> = ({
  episode
}) => {

  return (
    <Container>
      <pre>
        {/*EpisodeCard: {JSON.stringify(episode)}*/}
        {episode.name}
        <br />
        <i>{episode.durationS}</i>
        <br />
        Released: {episode.date}
        <EpisodeActivities />
      </pre>
    </Container>
  );
};
export default EpisodeCard;
