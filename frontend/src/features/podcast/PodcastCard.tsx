import React, { FunctionComponent, useMemo } from 'react';
import styled from 'styled-components/macro';
import { xml2js } from 'xml-js';
import { useTextFetch } from '../../common/hooks'
import { RssPodcast, PodcastType }  from '../../common/entities';
import Popup from 'reactjs-popup';
import { useDispatch, useSelector } from 'react-redux';
import { subscribeToPodcast, unsubscribeFromPodcast } from './podcastSlice';
import { RootState } from '../../app/rootReducer';

const Container = styled.div``;

const Img = styled.img`
  max-width: 100%;
  max-height: 100%;

  border-radius: 4px;
  cursor: pointer;
`;

const ImgSmall = styled.img`
  max-width: 45%;
  max-height: 45%;
  margin: 0.25rem;

  float: left;
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 4px;
  display: grid;
`;

const PodcastDescription = styled.div`
  text-align: center;
  margin: 0.25rem;
  background: ${props => props.theme.background};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 4px;
  max-width: 45%;
  max-height: 45%;
  float: right;
  word-wrap: normal;
  display: grid;

  padding: 4px;
`;

const StyledPopupDescription = styled.div`
  text-align: center;
  color: ${props => props.theme.pageBackground};
  font-color: ${props => props.theme.color};
  font-size: 1em;
  width: 100%;
  height: 100%;
  background: ${props => props.theme.pageBackground};
  border-radius: 4px;
`;

const StyledPopup = styled(Popup)`
  color: ${props => props.theme.pageBackground};
  font-color: ${props => props.theme.color};
  width: 100%;
  height: 100%;
  background: ${props => props.theme.pageBackground};
  border-radius: 4px;
`;

const SubscribeButton = styled.button`
  background: orange;
  border-radius: 4px;

  margin-top: .5em;
`;

const PodcastCard: FunctionComponent<{
  podcast: PodcastType;
}> = ({
  podcast
}) => {
  const dispatch = useDispatch();

  const subscriptions = useSelector((state: RootState) => state.podcast.subscriptions);
  const subscribed = podcast.id in subscriptions;

  return (
    <Container>
      <StyledPopup
        modal
        className="modal"
        trigger={<Img src={podcast.thumbnailUrl} title={podcast.name} alt={podcast.name} />}
        closeOnDocumentClick
      >
        <StyledPopupDescription>
          <ImgSmall src={podcast.thumbnailUrl} alt={podcast.name} title={podcast.name} />
          <PodcastDescription>
            <div>{podcast.name}</div>
            <div style={{ fontSize: '0.8em' }}>by {podcast.author}</div>
            {podcast.description && <div>{podcast.description}</div>}
            {podcast.url && (<a href={podcast.url} target="_blank" rel="noopener noreferrer">View more info</a>)}
            <div>
              <SubscribeButton type="button" onClick={() => {
                if (subscribed) {
                  dispatch(unsubscribeFromPodcast({ id: podcast.id }))
                } else {
                  dispatch(subscribeToPodcast({ id: podcast.id }))
                }
              }}>
                {subscribed ? 'Unsubscribe' : 'Subscribe'}
              </SubscribeButton>
            </div>
          </PodcastDescription>
        </StyledPopupDescription>
      </StyledPopup>
    </Container>
  );
};

export default PodcastCard;
