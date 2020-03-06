import React, { FunctionComponent, useMemo } from 'react';
import styled from 'styled-components/macro';
import { xml2js } from 'xml-js';
import { useTextFetch } from '../../common/hooks'
import { LibraryRssPodcast }  from '../../common/web-api';

const Container = styled.div``;

const Img = styled.img`
  max-width: 100%;
  max-height: 100%;

  border-radius: 4px;
`;

const PodcastCard: FunctionComponent<{
  podcast: LibraryRssPodcast;
}> = ({
  podcast
}) => {
  const { text: rss, isLoading } = useTextFetch(podcast.url!, { });

  const info = useMemo(() => {
    if (rss == null) {
      return null;
    }

    const doc = xml2js(rss);
    const entries = doc.elements[0].elements[0].elements;

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
