-- Schema Verification Script
-- Run this to check all master tables have required columns

-- 1. Check pricing_master table
SELECT 'pricing_master' as table_name, 
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'pricing_master'
ORDER BY ORDINAL_POSITION;

-- 2. Check gst_master table  
SELECT 'gst_master' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'gst_master'
ORDER BY ORDINAL_POSITION;

-- 3. Check Product table
SELECT 'Product' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'Product'
ORDER BY ORDINAL_POSITION;

-- 4. Check product_category table
SELECT 'product_category' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'product_category'
ORDER BY ORDINAL_POSITION;

-- 5. Check employee_master table
SELECT 'employee_master' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'employee_master'
ORDER BY ORDINAL_POSITION;

-- 6. Check user_master table
SELECT 'user_master' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'user_master'
ORDER BY ORDINAL_POSITION;

-- 7. Check role_master table
SELECT 'role_master' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'role_master'
ORDER BY ORDINAL_POSITION;

-- 8. Check Product_GST_Mapping table
SELECT 'Product_GST_Mapping' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'Product_GST_Mapping'
ORDER BY ORDINAL_POSITION;

-- 9. Check area_master table
SELECT 'area_master' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'area_master'
ORDER BY ORDINAL_POSITION;

-- 10. Check customer_master table
SELECT 'customer_master' as table_name,
       COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'customer_master'
ORDER BY ORDINAL_POSITION;

-- Summary: Show all tables in the database
SELECT TABLE_NAME, TABLE_TYPE, TABLE_ROWS 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'inventory_management'
ORDER BY TABLE_NAME;
