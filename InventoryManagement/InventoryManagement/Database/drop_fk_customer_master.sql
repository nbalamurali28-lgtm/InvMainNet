-- Drop foreign key from customer_master that references user_master
-- IMPORTANT: Back up the table before running these commands.

-- 1) Backup (run locally):
-- mysqldump -u <user> -p --single-transaction inventory_management customer_master > customer_master_backup.sql

-- 2) Identify constraint name (run in MySQL client):
-- SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE
-- WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'customer_master'
--   AND REFERENCED_TABLE_NAME = 'user_master';

-- 3) Drop the foreign key (replace <fk_name> with the actual name, e.g. customer_master_ibfk_2):
-- ALTER TABLE customer_master DROP FOREIGN KEY `<fk_name>`;

-- 4) Optionally drop the index that backed the FK (if you want):
-- SHOW INDEX FROM customer_master WHERE Column_name IN ('CreatedBy','created_by');
-- DROP INDEX `<index_name>` ON customer_master;

-- Example (based on your error message):
-- ALTER TABLE customer_master DROP FOREIGN KEY `customer_master_ibfk_2`;

-- After running, verify:
-- SELECT * FROM information_schema.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = DATABASE() AND TABLE_NAME = 'customer_master';
