import React, { FunctionComponent, useMemo } from 'react';
import styled from 'styled-components/macro';
import { xml2js } from 'xml-js';
import { useTextFetch } from '../../common/hooks'
import { RssPodcast, PodcastType }  from '../../common/entities';
import Popup from 'reactjs-popup'
import './podcastCard.css'

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

const PodcastCard: FunctionComponent<{
  podcast: PodcastType;
}> = ({
  podcast
}) => {
  const feedUrl = (podcast as RssPodcast).feedUrl || 'https://podcasts.files.bbci.co.uk/p02nq0gn.rss'; // TODO
  const { text: rss, isLoading } = useTextFetch(feedUrl, { });

  const info = useMemo(() => {
    if (rss == null) {
      return null;
    }

    const doc = xml2js(rss);
    const entries = doc
      .elements[0]
      .elements.filter(({ type }: { type: string; }) => type === 'element')[0]
      .elements;

    try {
      const title = entries
      .find((el: any) => el.name === 'title')
      .elements[0].text;
    const url = entries
      .find((el: any) => el.name === 'link')
      .elements[0].text;
    const imageUrl = entries
      .find((el: any) => el.name === 'image')
      .elements
      .find((el: any) => el.name === 'url')
      .elements[0].text;

      return {
        title,
        url,
        imageUrl
      };
    } catch (err) {
      console.error('Error parsing feed', { err, rss, doc, entries });
      return {
        title: 'TITLE',
        url: 'http://example.com'
      }
    }


  }, [rss]);

  return (
    <Container>
      {isLoading && '?'}
      {info && (
        <Popup className="modal" modal trigger={<Img src={info.imageUrl} alt={info.title} title={info.title} />} closeOnDocumentClick>
            <div>
              <ImgSmall src={info.imageUrl} alt={info.title} title={info.title} />
              <PodcastMenu>
                {info.title}
                <br />
                <a href={info.url} target="_blank" rel="noopener noreferrer"> View more info </a>
                <br />
                description description description description description description description description description description description description description description description description
              </PodcastMenu>
            </div>
        </Popup>
      )}
    </Container>
  );
};

export default PodcastCard;
