CREATE TABLE IF NOT EXISTS Users (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  ProfilePicture VARCHAR(255) NULL,
  UserName VARCHAR(255) NULL,
  Email VARCHAR(255) NOT NULL,
  PasswordHash VARCHAR(255) NOT NULL,
  EmailVerified BOOLEAN DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS Packages (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Name VARCHAR(255) NOT NULL,
  Country VARCHAR(255) NOT NULL,
  Credits INT NOT NULL,
  Price DECIMAL(10,2) NOT NULL,
  ExpireAt DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS ClassSchedules (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Title VARCHAR(255) NOT NULL,
  Country VARCHAR(255) NOT NULL,
  RequiredCredits INT NOT NULL,
  StartTime DATETIME NOT NULL,
  Capacity INT NOT NULL
);

CREATE TABLE IF NOT EXISTS UserPackages (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  UserId INT NOT NULL,
  PackageId INT NOT NULL,
  RemainingCredits INT NOT NULL,
  PurchasedAt DATETIME NOT NULL,
  FOREIGN KEY (UserId) REFERENCES Users(Id),
  FOREIGN KEY (PackageId) REFERENCES Packages(Id)
);

CREATE TABLE IF NOT EXISTS Bookings (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  UserId INT NOT NULL,
  ClassScheduleId INT NOT NULL,
  Canceled BOOLEAN DEFAULT FALSE,
  BookedAt DATETIME NOT NULL,
  FOREIGN KEY (UserId) REFERENCES Users(Id),
  FOREIGN KEY (ClassScheduleId) REFERENCES ClassSchedules(Id)
);

CREATE TABLE IF NOT EXISTS Waitlists (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  UserId INT NOT NULL,
  ClassScheduleId INT NOT NULL,
  AddedAt DATETIME NOT NULL,
  FOREIGN KEY (UserId) REFERENCES Users(Id),
  FOREIGN KEY (ClassScheduleId) REFERENCES ClassSchedules(Id)
);

-- sample data
INSERT INTO Users (Email, PasswordHash, EmailVerified, UserName)
VALUES
  ('alice@example.com', 'pass', TRUE, 'Alice'),
  ('bob@example.com', 'pass', FALSE, 'Bob');

INSERT INTO Packages (Name, Country, Credits, Price, ExpireAt)
VALUES
  ('Starter', 'US', 5, 9.99, DATE_ADD(NOW(), INTERVAL 30 DAY)),
  ('Pro', 'US', 10, 19.99, DATE_ADD(NOW(), INTERVAL 60 DAY));

INSERT INTO ClassSchedules (Title, Country, RequiredCredits, StartTime, Capacity)
VALUES
  ('Yoga Basics', 'US', 1, DATE_ADD(NOW(), INTERVAL 1 DAY), 20),
  ('Advanced Pilates', 'US', 2, DATE_ADD(NOW(), INTERVAL 2 DAY), 15);

INSERT INTO UserPackages (UserId, PackageId, RemainingCredits, PurchasedAt)
VALUES
  (1, 1, 5, NOW());

INSERT INTO Bookings (UserId, ClassScheduleId, Canceled, BookedAt)
VALUES
  (1, 1, FALSE, NOW());

INSERT INTO Waitlists (UserId, ClassScheduleId, AddedAt)
VALUES
  (1, 2, NOW());
