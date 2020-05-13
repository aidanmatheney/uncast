import React, { FunctionComponent, useMemo } from 'react';
import styled from 'styled-components/macro';
import { xml2js } from 'xml-js';
import { useTextFetch } from '../../common/hooks'
import { RssPodcast, PodcastType }  from '../../common/entities';

const Container = styled.div``;

const Img = styled.img`
  max-width: 100%;
  max-height: 100%;

  border-radius: 4px;
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
    const entries = doc.elements[0].elements[0].elements;

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
        <a href={info.url} target="_blank" rel="noopener noreferrer">
          <Img src={info.imageUrl} alt={info.title} title={info.title} />
        </a>
      )}
    </Container>
  );
};

export default PodcastCard;
