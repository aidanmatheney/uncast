DROP TABLE libraryrsspodcast;
DROP TABLE librarypodcast;
DROP TABLE librarypodcasttype;



CREATE TABLE File(
    Id char(36) NOT NULL,
    
    Path varchar(256) NOT NULL,
    OriginalName varchar(256) NULL,
    
    PRIMARY KEY(Id),
    
    UNIQUE(Path)
);



CREATE TABLE PodcastType(
    Name varchar(64) NOT NULL,
    PRIMARY KEY(Name)
);

INSERT INTO PodcastType(Name) VALUES
    ('Library'),
    ('Custom')
;

CREATE TABLE Podcast(
    Id char(36) NOT NULL,
    Type varchar(32) NOT NULL,
    
    Name varchar(256) CHARACTER SET UTF8MB4 NOT NULL,
    Author varchar(256) CHARACTER SET UTF8MB4 NOT NULL,
    Description varchar(4096) CHARACTER SET UTF8MB4 NOT NULL,
    ThumbnailFileId char(36) NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Type)
        REFERENCES PodcastType(Name)
        ON DELETE CASCADE,
    
    FOREIGN KEY (ThumbnailFileId)
        REFERENCES File(Id)
        ON DELETE RESTRICT
);

CREATE TABLE LibraryPodcastType(
    Name varchar(64) NOT NULL,
    PRIMARY KEY(Name)
);

INSERT INTO LibraryPodcastType(Name) VALUES
    ('Rss'),
    ('YouTube')
;

CREATE TABLE LibraryPodcast(
    Id char(36) NOT NULL,
    Type varchar(32) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES Podcast(Id)
        ON DELETE CASCADE,
    FOREIGN KEY(Type)
        REFERENCES LibraryPodcastType(Name)
        ON DELETE CASCADE
);

CREATE TABLE LibraryRssPodcast(
    Id char(36) NOT NULL,
    
    FeedUrl varchar(4096) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES LibraryPodcast(Id)
        ON DELETE CASCADE
);

CREATE TABLE LibraryYouTubePodcast(
    Id char(36) NOT NULL,
    
    ChannelId varchar(256) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES LibraryPodcast(Id)
        ON DELETE CASCADE
);

CREATE TABLE CustomPodcastType(
    Name varchar(64) NOT NULL,
    PRIMARY KEY(Name)
);

INSERT INTO CustomPodcastType(Name) VALUES
    ('Rss'),
    ('YouTube'),
    ('File')
;

CREATE TABLE CustomPodcast(
    Id char(36) NOT NULL,
    Type varchar(32) NOT NULL,
    
    UserId char(36) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES Podcast(Id)
        ON DELETE CASCADE,
    FOREIGN KEY(Type)
        REFERENCES CustomPodcastType(Name)
        ON DELETE CASCADE,
    
    FOREIGN KEY (UserId)
        REFERENCES Podcast(Id)
        ON DELETE CASCADE
);

CREATE TABLE CustomRssPodcast(
    Id char(36) NOT NULL,
    
    FeedUrl varchar(4096) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES CustomPodcast(Id)
        ON DELETE CASCADE
);

CREATE TABLE CustomYouTubePodcast(
    Id char(36) NOT NULL,
    
    ChannelId varchar(256) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES CustomPodcast(Id)
        ON DELETE CASCADE
);

CREATE TABLE CustomFilePodcast(
    Id char(36) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES CustomPodcast(Id)
        ON DELETE CASCADE
);



INSERT INTO Podcast(
    Id,
    Type,
    Name,
    Author,
    Description,
    ThumbnailFileId
) VALUES
    (
        '7c676fe4-c7a2-4bbd-bbd9-86fd4537b2b2',
        'Library',
        'The Joe Rogan Experience',
        'Joe Rogan',
        'The Joe Rogan Experience is a free audio and video podcast hosted by American comedian, actor, sports commentator, martial artist, and television host, Joe Rogan.',
        NULL
    ),
    (
        '8974c643-4ce4-4236-a770-e294158fc732',
        'Library',
        'Reply All',
        'Gimlet Media',
        'Reply All is an American podcast from Gimlet Media, hosted by PJ Vogt and Alex Goldman. The show features stories about how people shape the internet, and how the internet shapes people.',
        NULL
    )
;

INSERT INTO LibraryPodcast(
    Id,
    Type
) VALUES
    ('7c676fe4-c7a2-4bbd-bbd9-86fd4537b2b2', 'Rss'),
    ('8974c643-4ce4-4236-a770-e294158fc732', 'Rss')
;

INSERT INTO LibraryRssPodcast(
    Id,
    FeedUrl
) VALUES
    ('7c676fe4-c7a2-4bbd-bbd9-86fd4537b2b2', 'http://joeroganexp.joerogan.libsynpro.com/rss'),
    ('8974c643-4ce4-4236-a770-e294158fc732', 'https://feeds.megaphone.fm/replyall')
;



CREATE TABLE PodcastEpisode(
    Id char(36) NOT NULL,
    PodcastId char(36) NOT NULL,
    
    Name varchar(256) CHARACTER SET UTF8MB4 NOT NULL,
    Description varchar(4096) CHARACTER SET UTF8MB4 NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(PodcastId)
        REFERENCES CustomFilePodcast(Id)
        ON DELETE CASCADE
);

CREATE TABLE LibraryRssPodcastEpisode(
    Id char(36) NOT NULL,

    Url varchar(4096) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES PodcastEpisode(Id)
        ON DELETE CASCADE
);

CREATE TABLE LibraryYouTubePodcastEpisode(
    Id char(36) NOT NULL,
    
    Url varchar(4096) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES PodcastEpisode(Id)
        ON DELETE CASCADE
);

CREATE TABLE CustomRssPodcastEpisode(
    Id char(36) NOT NULL,
    
    Url varchar(4096) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES PodcastEpisode(Id)
        ON DELETE CASCADE
);

CREATE TABLE CustomYouTubePodcastEpisode(
    Id char(36) NOT NULL,
    
    Url varchar(4096) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES PodcastEpisode(Id)
        ON DELETE CASCADE
);

CREATE TABLE CustomFilePodcastEpisode(
    Id char(36) NOT NULL,
    
    FileId char(36) NOT NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(Id)
        REFERENCES PodcastEpisode(Id)
        ON DELETE CASCADE,
    
    FOREIGN KEY (FileId)
        REFERENCES File(Id)
        ON DELETE CASCADE
);