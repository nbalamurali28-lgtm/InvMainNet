-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: use inventory_management
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `todo_tasks`;
CREATE TABLE `todo_tasks` (   
`task_id` int NOT NULL AUTO_INCREMENT,  
 `title` varchar(255) NOT NULL,   
 `description` text,  
 `end_date` date DEFAULT NULL,   
 `completed` tinyint(1) DEFAULT '0',   
 `created_at` datetime DEFAULT CURRENT_TIMESTAMP,   
 `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,  
 PRIMARY KEY (`task_id`) 
 ) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
 
-- New column 'employee_id' added for user_master table -- Start 
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'user_master' 
    AND COLUMN_NAME = 'employee_id'
);

-- If the count is 0, prepare the ALTER statement; otherwise, run a dummy query
SET @sql = IF(@column_exists = 0, 
    'ALTER TABLE user_master ADD COLUMN employee_id INT', 
    'SELECT "Column already exists"'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
-- New column 'employee_id' added for user_master table -- End 


-- New column 'Remarks' added for role_master table -- Start 
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'role_master' 
    AND COLUMN_NAME = 'is_active'
);

-- If the count is 0, prepare the ALTER statement; otherwise, run a dummy query
SET @sql = IF(@column_exists = 0, 
    'ALTER TABLE role_master ADD COLUMN is_active TINYINT(1)', 
    'SELECT "Column already exists"'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
-- New column 'is_active' added for role_master table -- End 


-- New column 'Remarks' added for delivery_item_details table -- Start 
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'delivery_item_details' 
    AND COLUMN_NAME = 'Remarks'
);

-- If the count is 0, prepare the ALTER statement; otherwise, run a dummy query
SET @sql = IF(@column_exists = 0, 
    'ALTER TABLE delivery_item_details ADD COLUMN Remarks VARCHAR(1000)', 
    'SELECT "Column already exists"'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
-- New column 'Remarks' added for delivery_item_details table -- End 


-- New column 'OnlineQuantity' added for delivery_item_details table -- Start
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'delivery_item_details' 
    AND COLUMN_NAME = 'OnlineQuantity'
);

-- If the count is 0, prepare the ALTER statement; otherwise, run a dummy query
SET @sql = IF(@column_exists = 0, 
    'ALTER TABLE delivery_item_details ADD COLUMN OnlineQuantity decimal(10,2)', 
    'SELECT "Column already exists"'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
-- New column 'OnlineQuantity' added for delivery_item_details table -- End


-- New column 'Remarks' added for delivery_inf table -- Start
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'delivery_inf' 
    AND COLUMN_NAME = 'Remarks'
);

-- If the count is 0, prepare the ALTER statement; otherwise, run a dummy query
SET @sql = IF(@column_exists = 0, 
    'ALTER TABLE delivery_inf ADD COLUMN Remarks VARCHAR(1000)', 
    'SELECT "Column already exists"'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
-- New column 'Remarks' added for delivery_inf table -- End


--
-- Table structure for table `paymode_master`
--

DROP TABLE IF EXISTS `paymode_master`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paymode_master` (
  `PayModeId` varchar(15) NOT NULL,
  `PayModeName` varchar(25) NOT NULL,
  `CreatedBy` varchar(10) DEFAULT NULL,
  `UpdatedBy` varchar(10) DEFAULT NULL,
  `Created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `Updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`PayModeId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paymode_master`
--

LOCK TABLES `paymode_master` WRITE;
/*!40000 ALTER TABLE `paymode_master` DISABLE KEYS */;
INSERT INTO `paymode_master` VALUES ('BANK_TRANSFER','Online',NULL,NULL,'2026-05-09 23:06:33','2026-05-09 23:06:33'),('CARD','Card',NULL,NULL,'2026-05-09 23:06:33','2026-05-09 23:06:33'),('CASH','Cash',NULL,NULL,'2026-05-09 23:06:33','2026-05-09 23:06:33'),('CHEQUE','Cheque',NULL,NULL,'2026-05-09 23:06:33','2026-05-09 23:06:33'),('CREDIT','Credit',NULL,NULL,'2026-05-09 23:06:33','2026-05-09 23:06:33'),('PAYTM','Paytm',NULL,NULL,'2026-05-09 23:06:33','2026-05-09 23:06:33'),('UPI','UPI',NULL,NULL,'2026-05-09 23:06:33','2026-05-09 23:06:33');
/*!40000 ALTER TABLE `paymode_master` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'inventory_management'
--
/*!50003 DROP PROCEDURE IF EXISTS `DEL_INF_DELIVERY_INFO_PAYMENTS_REPORT` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DEL_INF_DELIVERY_INFO_PAYMENTS_REPORT`(param_FromDate datetime, param_ToDate datetime)
BEGIN
    CREATE TEMPORARY TABLE temp_results (
        Delivery_ID INT,
        DeliveryDate Varchar(25),
        EmployeeId Varchar(200), 
        Total_Quantity decimal(10,2), 
        Total_Amount decimal(15,2),
        BANK_TRANSFER decimal(10,2),         
        CARD decimal(10,2), 
        CASH decimal(10,2), 
        CHEQUE decimal(10,2), 
		CREDIT decimal(10,2), 
        PAYTM decimal(10,2), 
		UPI decimal(10,2)
    );

	INSERT INTO temp_results (Delivery_ID, DeliveryDate, EmployeeId, Total_Quantity, 
    Total_Amount, BANK_TRANSFER, CARD, CASH, CHEQUE, CREDIT, PAYTM, UPI) 
	(SELECT 
		MST.Delivery_ID, 
        DATE_FORMAT(MST.DeliveryDate, '%d-%b-%Y') AS 'DeliveryDate', 
        CONCAT(EMP.emp_code, ' - ', EMP.emp_name) AS 'EmployeeId', 
        MST.Total_Quantity, 
        MST.Total_Amount, 
        (SELECT IFNULL(SUM(PAY.Amount),0) FROM delivery_item_payment AS PAY WHERE PAY.Delivery_ID = MST.Delivery_ID AND PAY.PaymentMode = 'BANK_TRANSFER') AS BANK_TRANSFER, 
		(SELECT IFNULL(SUM(PAY.Amount),0) FROM delivery_item_payment AS PAY WHERE PAY.Delivery_ID = MST.Delivery_ID AND PAY.PaymentMode = 'CARD') AS CARD, 
        (SELECT IFNULL(SUM(PAY.Amount),0) FROM delivery_item_payment AS PAY WHERE PAY.Delivery_ID = MST.Delivery_ID AND PAY.PaymentMode = 'CASH') AS CASH, 
		(SELECT IFNULL(SUM(PAY.Amount),0) FROM delivery_item_payment AS PAY WHERE PAY.Delivery_ID = MST.Delivery_ID AND PAY.PaymentMode = 'CHEQUE') AS CHEQUE, 
        (SELECT IFNULL(SUM(PAY.Amount),0) FROM delivery_item_payment AS PAY WHERE PAY.Delivery_ID = MST.Delivery_ID AND PAY.PaymentMode = 'CREDIT') AS CREDIT, 
        (SELECT IFNULL(SUM(PAY.Amount),0) FROM delivery_item_payment AS PAY WHERE PAY.Delivery_ID = MST.Delivery_ID AND PAY.PaymentMode = 'PAYTM') AS PAYTM, 
        (SELECT IFNULL(SUM(PAY.Amount),0) FROM delivery_item_payment AS PAY WHERE PAY.Delivery_ID = MST.Delivery_ID AND PAY.PaymentMode = 'UPI') AS UPI
	FROM 
		delivery_inf AS MST 
        LEFT OUTER JOIN employee_master AS EMP ON EMP.emp_code = MST.EmployeeID 
	WHERE 
		MST.DeliveryDate BETWEEN param_FromDate AND param_ToDate 
	ORDER BY 
        MST.Delivery_ID, MST.DeliveryDate, MST.EmployeeID);
       
	SELECT 
		Delivery_ID AS 'Delivery Id', 
        DeliveryDate AS 'Delivery Date', 
        EmployeeId AS 'Employee Id', 
		Total_Quantity AS 'Total Quantity', 
        Total_Amount AS 'Total Amount', 
        BANK_TRANSFER AS 'Online', 
		CARD AS 'Card', 
        CASH AS 'Cash', 
        CHEQUE AS 'Cheque', 
        CREDIT AS 'Credit', 
        PAYTM AS 'Paytm', 
        UPI 
	FROM 
		temp_results;  
        
DROP TEMPORARY TABLE IF EXISTS temp_results; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DEL_INF_DELIVERY_INFO_PRODUCTS_REPORT` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DEL_INF_DELIVERY_INFO_PRODUCTS_REPORT`(param_FromDate datetime, param_ToDate datetime)
BEGIN
    SELECT 
		DTL.Delivery_ID AS 'Delivery Id', 
        DATE_FORMAT(DTL.DeliveryDate, '%d-%b-%Y') AS 'Delivery Date', 
        CONCAT(EMP.emp_code, ' - ', EMP.emp_name) AS 'Employee Id', 
        PRD.ProductName AS 'Product', 
        IFNULL(DTL.ActualDelivered,0) AS 'Quantity', 
        IFNULL(DTL.OnlineQuantity,0) AS 'OnlineQuantity', 
        IFNULL(DTL.Price,0) AS 'Price', 
        CONVERT((IFNULL(DTL.ActualDelivered,0) * IFNULL(DTL.Price,0)), decimal(15,2)) AS 'Amount', 
        DTL.Remarks 
	FROM 
		delivery_item_details AS DTL 
        LEFT OUTER JOIN product AS PRD ON PRD.ProductID = DTL.ProductID 
        LEFT OUTER JOIN employee_master AS EMP ON EMP.emp_code = DTL.EmployeeID 
	WHERE 
		DTL.DeliveryDate BETWEEN param_FromDate AND param_ToDate 
	ORDER BY 
        DTL.Delivery_ID, DTL.DeliveryDate, DTL.EmployeeID, PRD.ProductName;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DEL_INF_GET_DELIVERY_INFORMATION` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DEL_INF_GET_DELIVERY_INFORMATION`(param_DeliveryDate datetime, param_EmployeeId int)
BEGIN
	SELECT 
		Delivery_ID, 
        DeliveryDate, 
        EmployeeID, 
        Total_Amount, 
        Total_Quantity, 
        Remarks 
	FROM 
		`delivery_inf` 
	WHERE 
		DeliveryDate = param_DeliveryDate AND 
        EmployeeID = param_EmployeeId;
		
	SELECT 
		`DEL`.`Detail_ID`,
		`DEL`.`Delivery_ID`,
		`DEL`.`DeliveryDate`,
        `DEL`.`EmployeeID`,
		`DEL`.`ProductID`,
		`DEL`.`ActualDelivered` AS Quantity,
        `DEL`.`OnlineQuantity` AS OnlineQuantity,
		`DEL`.`Price`, 
        CONVERT((IFNULL(`DEL`.`ActualDelivered`,0.00) * IFNULL(`DEL`.`Price`,0.00)), DECIMAL(10,2)) AS `TotalAmount`, 
        `DEL`.`Remarks` 
	FROM 
		`delivery_item_details` AS `DEL` 
	WHERE 
		`DEL`.`DeliveryDate` = param_DeliveryDate AND 
        `DEL`.`EmployeeID` = param_EmployeeId
	ORDER BY 
		`DEL`.`Delivery_ID`,
		`DEL`.`ProductID`;
        
	SELECT 
		`delivery_item_payment`.`Payment_ID`,
		`delivery_item_payment`.`Delivery_ID`,
		`delivery_item_payment`.`DeliveryDate`,
		`delivery_item_payment`.`EmployeeID`,
		`delivery_item_payment`.`PaymentMode`,
		`delivery_item_payment`.`Amount`
	FROM 
		`inventory_management`.`delivery_item_payment` 
    WHERE 
		`delivery_item_payment`.`DeliveryDate` = param_DeliveryDate AND 
        `delivery_item_payment`.`EmployeeID` = param_EmployeeId
	ORDER BY 
		`delivery_item_payment`.`Delivery_ID`;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DEL_INF_GET_MASTERS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DEL_INF_GET_MASTERS`()
BEGIN
	SELECT `ProductID`, `ProductName` FROM `product` ORDER BY `product`.`ProductName`;

	SELECT `PayModeId`, `PayModeName` FROM `paymode_master` ORDER BY `PayModeName`;  
    
    SELECT `emp_code`, CONCAT(`emp_code`, ' - ',`emp_name`) AS emp_name FROM `employee_master` 
    WHERE `status` = 'ACTIVE' ORDER BY `emp_name`; 
    
	SELECT `productid`, `base_price` FROM `pricing_master` 
    WHERE 
		`effectiveStatus` = 'Active' 
		AND (`effective_to` IS NULL OR `effective_to` <= CURDATE()) 
    ORDER BY `productid`;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-21 21:30:51
