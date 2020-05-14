import React, { FunctionComponent, useMemo } from 'react';
import styled from 'styled-components/macro';
import { xml2js } from 'xml-js';
import { useTextFetch } from '../../common/hooks'
import { RssPodcast, PodcastType }  from '../../common/entities';
import Popup from 'reactjs-popup';
import './podcastCard.css'
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

  float: left;
  border-radius: 4px;
  column-span: 1;
  display: grid;
`;

const PodcastMenu = styled.div`
  text-align: center;
  margin: 0.25rem;
  background: ${props => props.theme.pageBackground};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};

  max-width: 45%;
  max-height: 45%;
  float: right;
  word-wrap: normal;
  display: grid;
`;

const SubscribeButton = styled.button`
  background: orange;
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
      <Popup
        modal
        className="modal"
        trigger={<Img src={podcast.thumbnailUrl} title={podcast.name} alt={podcast.name} />}
        closeOnDocumentClick
      >
        <div>
          <ImgSmall src={podcast.thumbnailUrl} alt={podcast.name} title={podcast.name} />
          <PodcastMenu>
            {podcast.name}
            <br />
            <a href={(podcast as RssPodcast).feedUrl} target="_blank" rel="noopener noreferrer"> View more info </a>
            <br />
            description description description description description description description description description description description description description description description description

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
          </PodcastMenu>
        </div>
      </Popup>
    </Container>
  );
};

export default PodcastCard;
