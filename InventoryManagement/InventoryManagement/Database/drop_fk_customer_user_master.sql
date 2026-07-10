-- Drop Foreign Key Constraint: customer_master references user_master
-- This script removes the FK integrity check between customer_master.CreatedBy 
-- (or created_by) and user_master.user_id, allowing customer records to be saved 
-- without requiring a valid user_master entry.

USE `inventory_management`;

-- Step 1: Find and drop the foreign key constraint
-- The constraint name is typically customer_master_ibfk_2 or similar
-- This query identifies the exact constraint name
SET @constraint_name = (
    SELECT CONSTRAINT_NAME
    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'customer_master'
      AND REFERENCED_TABLE_NAME = 'user_master'
    LIMIT 1
);

-- Drop the constraint if it exists
IF @constraint_name IS NOT NULL THEN
    SET @sql = CONCAT('ALTER TABLE `customer_master` DROP FOREIGN KEY `', @constraint_name, '`');
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    SELECT CONCAT('Foreign key constraint "', @constraint_name, '" dropped successfully.') AS result;
ELSE
    SELECT 'No foreign key constraint found referencing user_master.' AS result;
END IF;

-- Step 2: Verify removal
-- This query confirms the FK has been dropped
SELECT 
    CONSTRAINT_NAME,
    TABLE_NAME,
    REFERENCED_TABLE_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE CONSTRAINT_SCHEMA = DATABASE()
  AND TABLE_NAME = 'customer_master';

-- Result: Should return an empty result set if the FK was successfully removed
