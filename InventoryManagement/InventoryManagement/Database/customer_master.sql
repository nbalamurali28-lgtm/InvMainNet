CREATE TABLE IF NOT EXISTS customer_master (
  customer_id VARCHAR(20) NOT NULL,
  customer_name VARCHAR(150) NOT NULL,
  contact_person VARCHAR(150) DEFAULT NULL,
  email VARCHAR(150) DEFAULT NULL,
  phone VARCHAR(20) DEFAULT NULL,
  address TEXT DEFAULT NULL,
  gst_number VARCHAR(50) DEFAULT NULL,
  area_id VARCHAR(20) DEFAULT NULL,
  is_active TINYINT(1) NOT NULL DEFAULT 1,
  created_at DATETIME DEFAULT NULL,
  updated_at DATETIME DEFAULT NULL,
  created_by VARCHAR(50) DEFAULT NULL,
  updated_by VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (customer_id),
  KEY idx_customer_area (area_id),
  CONSTRAINT fk_customer_area FOREIGN KEY (area_id) REFERENCES area_master (area_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
