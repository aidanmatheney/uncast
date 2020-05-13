import { PodcastType } from '../../common/entities';

const podcasts: PodcastType[] = [
  {
    name: 'The Joe Rogan Experience',
    author: 'Joe Rogan',
    description: 'The Joe Rogan Experience is a free audio and video podcast hosted by American comedian, actor, sports commentator, martial artist, and television host, Joe Rogan.',
    feedUrl: 'http://joeroganexp.joerogan.libsynpro.com/rss'
  },
  {
    name: 'Reply All',
    author: 'Gimlet Media',
    description: 'Reply All is an American podcast from Gimlet Media, hosted by PJ Vogt and Alex Goldman. The show features stories about how people shape the internet, and how the internet shapes people.',
    feedUrl: 'https://feeds.megaphone.fm/replyall'
  }
];

export default podcasts;
