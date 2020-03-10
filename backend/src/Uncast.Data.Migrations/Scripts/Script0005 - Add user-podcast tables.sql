CREATE TABLE AppColorScheme(
    Name varchar(36) NOT NULL,
    PRIMARY KEY(Name)
);

INSERT INTO AppColorScheme(Name) VALUES
    ('System'),
    ('Light'),
    ('Dark')
;

CREATE TABLE PodcastEpisodePlaybackStyle(
    Name varchar(36) NOT NULL,
    PRIMARY KEY(Name)
);

INSERT INTO PodcastEpisodePlaybackStyle(Name) VALUES
    ('Stream'),
    ('Download')
;

CREATE TABLE UserAppState(
    UserId char(36) NOT NULL,
    
    ColorScheme varchar(32) NOT NULL,
    DefaultPlaybackStyle varchar(32) NOT NULL,
    DefaultPlaybackSpeed decimal(5,2) NOT NULL,
    
    PRIMARY KEY(UserId),
    FOREIGN KEY(UserId)
        REFERENCES User(Id)
        ON DELETE CASCADE,
        
    FOREIGN KEY(ColorScheme)
        REFERENCES AppColorScheme(Name)
        ON DELETE RESTRICT,
    FOREIGN KEY(DefaultPlaybackStyle)
        REFERENCES PodcastEpisodePlaybackStyle(Name)
        ON DELETE RESTRICT
);

CREATE TABLE UserPodcastState(
    UserId char(36) NOT NULL,
    PodcastId char(36) NOT NULL,
    
    IsSubscription bit NOT NULL,
    IsFavorite bit NOT NULL,
    IsAutoDownload bit NOT NULL,
    PlaybackSpeed decimal(5,2) NULL,
    
    PRIMARY KEY(UserId, PodcastId),
    FOREIGN KEY(UserId)
        REFERENCES User(Id)
        ON DELETE CASCADE,
    FOREIGN KEY(PodcastId)
        REFERENCES Podcast(Id)
        ON DELETE CASCADE
);

CREATE TABLE PodcastEpisodePlaybackStatus(
    Name varchar(36) NOT NULL,
    PRIMARY KEY(Name)
);

INSERT INTO PodcastEpisodePlaybackStatus(Name) VALUES
    ('Unplayed'),
    ('InProgress'),
    ('Played')
;

CREATE TABLE UserPodcastEpisodeState(
    UserId char(36) NOT NULL,
    EpisodeId char(36) NOT NULL,
    
    PlaybackStatus varchar(32) NOT NULL,
    ProgressMs int NULL,
    
    PRIMARY KEY(UserId, EpisodeId),
    FOREIGN KEY(UserId)
        REFERENCES User(Id)
        ON DELETE CASCADE,
    FOREIGN KEY(EpisodeId)
        REFERENCES PodcastEpisode(Id)
        ON DELETE CASCADE,
        
    FOREIGN KEY(PlaybackStatus)
        REFERENCES PodcastEpisodePlaybackStatus(Name)
        ON DELETE RESTRICT
);

CREATE TABLE UserPodcastEpisodeQueueItem(
    UserId char(36) NOT NULL,
    EpisodeId char(36) NOT NULL,
    
    Ordinal int NOT NULL,
    
    PRIMARY KEY(UserId, EpisodeId),
    FOREIGN KEY(UserId)
        REFERENCES User(Id)
        ON DELETE CASCADE,
    FOREIGN KEY(EpisodeId)
        REFERENCES PodcastEpisode(Id)
        ON DELETE CASCADE,
        
    UNIQUE(UserId, Ordinal)
);
