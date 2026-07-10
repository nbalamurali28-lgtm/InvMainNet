-- Complete pricing_master table definition with all required columns
-- This script creates or replaces the pricing_master table with proper schema

CREATE TABLE IF NOT EXISTS pricing_master (
  pricing_id INT NOT NULL AUTO_INCREMENT,
  ProductID INT NOT NULL,
  base_price DECIMAL(12, 2) NOT NULL,
  gst_id INT NOT NULL,
  effective_from DATETIME NULL,
  effective_to DATETIME NULL,
  effectiveStatus VARCHAR(20) DEFAULT 'ACTIVE',
  created_by VARCHAR(50) DEFAULT NULL,
  created_at DATETIME DEFAULT NULL,
  updated_by VARCHAR(50) DEFAULT NULL,
  updated_at DATETIME DEFAULT NULL,
  PRIMARY KEY (pricing_id),
  KEY idx_pricing_product (ProductID),
  KEY idx_pricing_gst (gst_id),
  KEY idx_pricing_effective_dates (effective_from, effective_to),
  CONSTRAINT fk_pricing_product FOREIGN KEY (ProductID) REFERENCES Product (ProductID),
  CONSTRAINT fk_pricing_gst FOREIGN KEY (gst_id) REFERENCES gst_master (gst_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Add indexes for frequently queried conditions
CREATE INDEX IF NOT EXISTS idx_pricing_status ON pricing_master(effectiveStatus);
CREATE INDEX IF NOT EXISTS idx_pricing_date_range ON pricing_master(effective_from, effective_to);
