-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 24, 2019 at 04:42 AM
-- Server version: 10.1.36-MariaDB
-- PHP Version: 7.2.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `webdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `Username` varchar(20) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Password` varchar(10) NOT NULL,
  `Security_Question` varchar(100) NOT NULL,
  `Security_Answer` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`Username`, `FirstName`, `LastName`, `Email`, `Password`, `Security_Question`, `Security_Answer`) VALUES
('1:12 Baby!!!', 'Will', 'Fidelio', 'Mark@Rutzu.com', 'lookatmyin', 'I\'ve never seen a 1:13', 'And I never will'),
('Cutieeeee<3', 'James', 'Lee', 'Cutie69@hotmail.com', 'StopItHunt', 'Who do i love?', 'Sean'),
('Heart Breaker :(', 'Sean', 'Meath', 'Sean@gmail.com', 'hey', 'I', 'James'),
('ITouchKids', 'Hunter', 'Rogers', 'PrettyBoi@gmail.com', 'pedoisme', 'Who do i harass?', 'James');

-- --------------------------------------------------------

--
-- Table structure for table `log`
--

CREATE TABLE `log` (
  `Username` varchar(20) NOT NULL,
  `NumUnsuccessfulLogins` int(11) NOT NULL DEFAULT '0',
  `NumLogins` int(11) NOT NULL DEFAULT '0',
  `LastLogin` timestamp NULL DEFAULT NULL,
  `Blocked` tinyint(4) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `log`
--

INSERT INTO `log` (`Username`, `NumUnsuccessfulLogins`, `NumLogins`, `LastLogin`, `Blocked`) VALUES
('Heart Breaker :(', 0, 0, NULL, 0),
('Cutieeeee<3', 0, 0, NULL, 0),
('1:12 Baby!!!', 0, 0, NULL, 0),
('ITouchKids', 0, 0, NULL, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD UNIQUE KEY `Username` (`Username`,`Email`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
