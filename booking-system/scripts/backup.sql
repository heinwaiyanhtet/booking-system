-- MySQL dump 10.13  Distrib 8.0.42, for Linux (aarch64)
--
-- Host: localhost    Database: booking
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Bookings`
--

DROP TABLE IF EXISTS `Bookings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Bookings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ClassScheduleId` int NOT NULL,
  `Canceled` tinyint(1) DEFAULT '0',
  `BookedAt` datetime NOT NULL,
  `CheckedIn` tinyint(1) DEFAULT NULL,
  `UserPackageId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  KEY `ClassScheduleId` (`ClassScheduleId`),
  KEY `FK_Bookings_UserPackageId` (`UserPackageId`),
  CONSTRAINT `Bookings_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`),
  CONSTRAINT `Bookings_ibfk_2` FOREIGN KEY (`ClassScheduleId`) REFERENCES `ClassSchedules` (`Id`),
  CONSTRAINT `FK_Bookings_UserPackageId` FOREIGN KEY (`UserPackageId`) REFERENCES `UserPackages` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Bookings`
--

LOCK TABLES `Bookings` WRITE;
/*!40000 ALTER TABLE `Bookings` DISABLE KEYS */;
INSERT INTO `Bookings` VALUES (1,1,1,1,'2025-06-21 07:14:04',1,NULL),(2,1,2,0,'2025-06-21 08:27:55',0,NULL),(3,1,3,1,'2025-06-21 08:28:29',0,1),(5,1,3,0,'2025-06-21 16:37:28',0,1),(7,1,3,0,'2025-06-21 16:42:25',0,1),(8,1,1,0,'2025-06-21 16:42:44',0,1),(9,1,3,0,'2025-06-21 16:44:00',0,1),(10,1,2,0,'2025-06-21 17:13:57',0,1);
/*!40000 ALTER TABLE `Bookings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ClassSchedules`
--

DROP TABLE IF EXISTS `ClassSchedules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ClassSchedules` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `Country` varchar(255) NOT NULL,
  `RequiredCredits` int NOT NULL,
  `StartTime` datetime NOT NULL,
  `Capacity` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ClassSchedules`
--

LOCK TABLES `ClassSchedules` WRITE;
/*!40000 ALTER TABLE `ClassSchedules` DISABLE KEYS */;
INSERT INTO `ClassSchedules` VALUES (1,'Yoga Basics','US',1,'2025-06-22 07:12:04',30),(2,'Advanced Pilates','US',2,'2025-06-23 07:12:04',15),(3,'Yoga Basics','China',1,'2025-06-22 07:14:04',20),(4,'Advanced Pilates','China',2,'2025-06-23 07:14:04',15);
/*!40000 ALTER TABLE `ClassSchedules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Packages`
--

DROP TABLE IF EXISTS `Packages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Packages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Country` varchar(255) NOT NULL,
  `Credits` int NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `ExpireAt` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Packages`
--

LOCK TABLES `Packages` WRITE;
/*!40000 ALTER TABLE `Packages` DISABLE KEYS */;
INSERT INTO `Packages` VALUES (1,'Starter','US',5,9.99,'2025-07-21 07:12:04'),(2,'Pro','US',10,19.99,'2025-08-20 07:12:04'),(3,'Starter','China',5,9.99,'2025-07-21 07:14:04'),(4,'Pro','China',10,19.99,'2025-08-20 07:14:04');
/*!40000 ALTER TABLE `Packages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserPackages`
--

DROP TABLE IF EXISTS `UserPackages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserPackages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `PackageId` int NOT NULL,
  `RemainingCredits` int NOT NULL,
  `PurchasedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  KEY `PackageId` (`PackageId`),
  CONSTRAINT `UserPackages_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`),
  CONSTRAINT `UserPackages_ibfk_2` FOREIGN KEY (`PackageId`) REFERENCES `Packages` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserPackages`
--

LOCK TABLES `UserPackages` WRITE;
/*!40000 ALTER TABLE `UserPackages` DISABLE KEYS */;
INSERT INTO `UserPackages` VALUES (1,1,1,93,'2025-06-21 07:14:04');
/*!40000 ALTER TABLE `UserPackages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProfilePicture` varchar(255) DEFAULT NULL,
  `UserName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `EmailVerified` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Users`
--

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
INSERT INTO `Users` VALUES (1,NULL,'Alice','alice@example.com','pass',1),(2,NULL,'Bob','bob@example.com','pass',0),(3,NULL,'Alice','alice@example.com','pass',1),(4,NULL,'Bob','bob@example.com','pass',0),(5,NULL,NULL,'hein@gmail.com','AQAAAAIAAYagAAAAEBMW9SABdKGvVcrIPC93insEtB1ZatODeu7HLXgLVEta5oHmb4cU7NlKQP4uZ/U/fQ==',0),(6,NULL,NULL,'hein@gmail.com','AQAAAAIAAYagAAAAEG1K4kMX7BTf8sTjp4ASZiQemOfEYeNfLHl9qXM2inGYa9gV3g6tmu7yN2ZY2gP//Q==',0),(7,NULL,NULL,'heinwaiyanhtet2020@gmail.com','AQAAAAIAAYagAAAAEPPLmKiNaiW0AB62qIA+IQ7Cgg96pdS+iE+us9Ox0jJkq6UyMsCLz3+n2U8/KZ1Zsg==',0);
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Waitlists`
--

DROP TABLE IF EXISTS `Waitlists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Waitlists` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ClassScheduleId` int NOT NULL,
  `AddedAt` datetime NOT NULL,
  `ReservedCredits` int NOT NULL,
  `UserPackageId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  KEY `ClassScheduleId` (`ClassScheduleId`),
  KEY `FK_Waitlists_UserPackageId` (`UserPackageId`),
  CONSTRAINT `FK_Waitlists_UserPackageId` FOREIGN KEY (`UserPackageId`) REFERENCES `UserPackages` (`Id`),
  CONSTRAINT `Waitlists_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`),
  CONSTRAINT `Waitlists_ibfk_2` FOREIGN KEY (`ClassScheduleId`) REFERENCES `ClassSchedules` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Waitlists`
--

LOCK TABLES `Waitlists` WRITE;
/*!40000 ALTER TABLE `Waitlists` DISABLE KEYS */;
INSERT INTO `Waitlists` VALUES (1,1,2,'2025-06-21 07:14:04',0,NULL),(2,1,1,'2025-06-21 16:46:39',1,1);
/*!40000 ALTER TABLE `Waitlists` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-21 21:23:31
