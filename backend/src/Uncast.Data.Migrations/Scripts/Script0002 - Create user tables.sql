CREATE TABLE User(
    Id char(36) PRIMARY KEY,
    UserName nvarchar(256) NULL,
	NormalizedUserName nvarchar(256) NULL,
	Email nvarchar(256) NULL,
	NormalizedEmail nvarchar(256) NULL,
	EmailConfirmed bit NOT NULL,
	PasswordHash nvarchar(256) NULL,
	SecurityStamp nvarchar(256) NULL,
	ConcurrencyStamp nvarchar(256) NULL,
	PhoneNumber nvarchar(256) NULL,
	PhoneNumberConfirmed bit NOT NULL,
	TwoFactorEnabled bit NOT NULL,
	LockoutEnd datetime NULL,
	LockoutEnabled bit NOT NULL,
	AccessFailedCount int NOT NULL,
	AuthenticatorKey nvarchar(256) NULL
);

CREATE TABLE Role(
	Id char(36) PRIMARY KEY,
	Name varchar(256) NOT NULL,
	NormalizedName varchar(256) NOT NULL,
	ConcurrencyStamp nvarchar(256) NULL
);

CREATE TABLE UserRole(
	UserId char(36),
	FOREIGN KEY (UserId)
		REFERENCES User(Id)
		ON DELETE CASCADE,

	RoleId char(36),
	FOREIGN KEY (RoleId)
		REFERENCES Role(Id)
		ON DELETE CASCADE,

	PRIMARY KEY (UserId, RoleId)
);

CREATE TABLE UserClaim(
	UserId char(36),
	FOREIGN KEY (UserId)
		REFERENCES User(Id)
		ON DELETE CASCADE,

	ClaimType varchar(256) NOT NULL,
	ClaimValue nvarchar(2048) NOT NULL,

	PRIMARY KEY (UserId, ClaimType)
);

CREATE TABLE UserLogin(
	UserId char(36),
	FOREIGN KEY (UserId)
		REFERENCES User(Id)
		ON DELETE CASCADE,

	LoginProvider nvarchar(256),
	ProviderKey varchar(256),
	ProviderDisplayName nvarchar(256) NOT NULL,

	PRIMARY KEY (UserId, LoginProvider, ProviderKey)
);

CREATE TABLE UserRecoveryCode(
	UserId char(36),
	FOREIGN KEY (UserId)
		REFERENCES User(Id)
		ON DELETE CASCADE,

	RecoveryCode varchar(256) NOT NULL,

	PRIMARY KEY (UserId, RecoveryCode)
);

CREATE TABLE PersistedGrant(
	`Key` nvarchar(2048) NOT NULL,
	Type nvarchar(2048) NOT NULL,
	SubjectId nvarchar(2048) NOT NULL,
	ClientId nvarchar(2048) NOT NULL,
	CreationTime datetime NOT NULL,
	Expiration datetime NULL,
	Data nvarchar(2048) NOT NULL
);
