# Host: localhost  (Version 5.5.5-10.1.32-MariaDB)
# Date: 2021-12-06 06:43:23
# Generator: MySQL-Front 6.1  (Build 1.26)


#
# Structure for table "segmentos"
#

DROP TABLE IF EXISTS `segmentos`;
CREATE TABLE `segmentos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(255) DEFAULT NULL,
  `rColor` int(5) DEFAULT NULL,
  `gColor` int(5) DEFAULT NULL,
  `bColor` int(5) DEFAULT NULL,
  `color2` longblob,
  `r2` int(5) DEFAULT NULL,
  `g2` int(5) DEFAULT NULL,
  `b2` int(5) DEFAULT NULL,
  `color1` longblob,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=latin1;

#
# Data for table "segmentos"
#

