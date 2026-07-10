-- Migration: Add missing columns to gst_master table
-- Issue: "Error loading dropdowns: Unknown column 'effective_from' in 'field list'"
-- Location: PricingMaster.aspx → LoadDropdowns() → GetGSTMaster()
-- Description: Adds effective_from and effective_to columns to gst_master for tax rate validity periods

ALTER TABLE gst_master 
ADD COLUMN IF NOT EXISTS effective_from DATETIME NULL AFTER description,
ADD COLUMN IF NOT EXISTS effective_to DATETIME NULL AFTER effective_from;

-- Verify the migration
SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'gst_master'
ORDER BY ORDINAL_POSITION;
