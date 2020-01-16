CREATE TABLE LibraryPodcastType(
	Id int AUTO_INCREMENT PRIMARY KEY,
    Name varchar(32) NOT NULL UNIQUE
);

INSERT INTO LibraryPodcastType(Name) VALUES ("RSS");
SET @rssTypeId = (SELECT Id FROM LibraryPodcastType WHERE Name = "RSS");


CREATE TABLE LibraryPodcast(
	Id int AUTO_INCREMENT PRIMARY KEY,
    TypeId int NOT NULL,
    FOREIGN KEY (TypeId)
		REFERENCES LibraryPodcastType(Id)
		ON DELETE CASCADE
);

CREATE TABLE LibraryRssPodcast(
	Id int PRIMARY KEY,
    FOREIGN KEY (Id)
		REFERENCES LibraryPodcast(Id)
        ON DELETE CASCADE,
	
    Url varchar(256) NOT NULL UNIQUE
);


INSERT INTO LibraryPodcast(TypeId) VALUES (@rssTypeId);
INSERT INTO LibraryRssPodcast(Id, Url) VALUES (LAST_INSERT_ID(), "http://joeroganexp.joerogan.libsynpro.com/rss");

INSERT INTO LibraryPodcast(TypeId) VALUES (@rssTypeId);
INSERT INTO LibraryRssPodcast(Id, Url) VALUES (LAST_INSERT_ID(), "https://feeds.megaphone.fm/replyall");
