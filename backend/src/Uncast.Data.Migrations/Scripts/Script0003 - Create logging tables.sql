CREATE TABLE WebApiLogEntry(
    Id int AUTO_INCREMENT PRIMARY KEY,
    TimeWritten datetime NOT NULL,
    ServerName varchar(64) NOT NULL,
    Category varchar(256) NOT NULL,
    Scope longtext NULL,
    LogLevel varchar(11) NOT NULL,
    EventId int NOT NULL,
    EventName varchar(64) NULL,
    Message longtext NOT NULL,
    Exception longtext NULL
);

CREATE TABLE WebAppLogEntry(
    Id int AUTO_INCREMENT PRIMARY KEY,
    TimeWritten datetime NOT NULL
);
