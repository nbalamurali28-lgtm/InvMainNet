-- Migration: Add missing columns to pricing_master table
-- Issue: "Error loading dropdowns: Unknown column 'effective_from' in 'field list'"
-- Description: Adds effective_from, effective_to, and effectiveStatus columns to support historical pricing

ALTER TABLE pricing_master 
ADD COLUMN IF NOT EXISTS effective_from DATETIME NULL AFTER gst_id,
ADD COLUMN IF NOT EXISTS effective_to DATETIME NULL AFTER effective_from,
ADD COLUMN IF NOT EXISTS effectiveStatus VARCHAR(20) DEFAULT 'ACTIVE' AFTER effective_to;

-- Verify the migration
SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'pricing_master'
ORDER BY ORDINAL_POSITION;
