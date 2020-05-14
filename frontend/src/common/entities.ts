interface PodcastBase {
  id: string;
  name: string;
  author?: string;
  description?: string;
  url?: string;
  thumbnailUrl?: string;
}

export type RssPodcast = PodcastBase & {
  type: 'rss';
  feedUrl: string;
};

export type FilePodcast = PodcastBase & {
  type: 'file';
  file: any;
};

export type Podcast = RssPodcast | FilePodcast;

interface EpisodeBase {
  id: string;
  podcastId: string;
  name: string;
  description?: string;
  dateUnix?: number;
  durationS: number;
}

export type RssEpisode = EpisodeBase & {
  type: 'rss';
  fileUrl: string;
};

export type FileEpisode = EpisodeBase & {
  type: 'file';
  file: any;
};

export type Episode = RssEpisode | FileEpisode;
