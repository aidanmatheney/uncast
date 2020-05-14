export interface PodcastBase {
  id: string;
  name: string;
  author: string;
  description: string;
  thumbnailUrl: string;
}

export interface RssPodcast extends PodcastBase {
  feedUrl: string;
}

export interface FilePodcast extends PodcastBase {
  file: any;
}

export type PodcastType = RssPodcast | FilePodcast;
