# Pricing Master Error Fix - Checklist

## Pre-Implementation
- [ ] Read `SOLUTION_SUMMARY.md` for complete context
- [ ] Backup database: `mysqldump -u root -p inventory_management > backup.sql`
- [ ] Note current database state with verification script
- [ ] Notify team members of maintenance window (if needed)

## Step 1: Database Migration
- [ ] Open MySQL Workbench / MySQL Command Line
- [ ] Connect to `inventory_management` database using: `root` / `root` (or your credentials)
- [ ] Copy SQL from `Database/migrate_gst_master.sql`
- [ ] Execute the migration script
- [ ] Check for any errors in output

## Step 2: Verification
- [ ] Execute verification query: `DESCRIBE gst_master;`
- [ ] Confirm all these columns exist:
  - [ ] gst_id (INT, AUTO_INCREMENT)
  - [ ] gst_percentage (DECIMAL)
  - [ ] description (VARCHAR)
  - [ ] effective_from (DATETIME) ✅ NEW
  - [ ] effective_to (DATETIME) ✅ NEW
  - [ ] is_active (TINYINT)
  - [ ] created_by (VARCHAR)
  - [ ] created_at (DATETIME)
  - [ ] updated_by (VARCHAR, nullable)
  - [ ] updated_at (DATETIME, nullable)

## Step 3: Application Update
- [ ] Close running Visual Studio instance
- [ ] Close running web application / IIS
- [ ] Delete `bin/` folder (clean build)
- [ ] Delete `obj/` folder (clean build)
- [ ] Open solution in Visual Studio
- [ ] Build → Clean Solution
- [ ] Build → Rebuild Solution
- [ ] Verify no compilation errors

## Step 4: Testing
- [ ] Start the application
- [ ] Clear browser cache (Ctrl+Shift+Delete)
- [ ] Navigate to **Pricing Master** page
- [ ] Verify page loads without errors ✅
- [ ] Test Product dropdown loads ✅
- [ ] Test GST dropdown loads ✅
- [ ] Verify grid displays any existing records ✅
- [ ] Try adding a new pricing record ✅
- [ ] Try editing a pricing record ✅
- [ ] Try deleting a pricing record ✅

## Step 5: Validation
- [ ] No SQL errors in browser console
- [ ] No errors in Visual Studio Output window
- [ ] No errors in web.config connection string
- [ ] Application load time is normal
- [ ] Database backup still available (in case rollback needed)

## Step 6: Documentation
- [ ] Update team documentation if needed
- [ ] Note timestamp of fix in changelog
- [ ] Mark as "RESOLVED" in issue tracking
- [ ] Run `Database/verify_all_schemas.sql` to check other tables

## Rollback Plan (if needed)
- [ ] Restore database: `mysql -u root -p inventory_management < backup.sql`
- [ ] Restart application
- [ ] Verify previous state

## Sign-Off
- Date Fixed: ________________
- Fixed By: ________________
- Tested By: ________________
- Approved By: ________________

---

## Common Issues & Solutions

### Issue: "Access Denied" when running SQL
**Solution:** 
- Verify MySQL user credentials in web.config
- Check user has ALTER TABLE privilege
- Run: `GRANT ALL PRIVILEGES ON inventory_management.* TO 'root'@'localhost';`

### Issue: "Column already exists" error
**Solution:**
- Migration script already added the columns
- Skip migration and proceed to testing
- Verify columns with: `DESCRIBE pricing_master;`

### Issue: Still getting "Unknown column" error after migration
**Solution:**
1. Clear browser cache: Ctrl+Shift+Delete
2. Rebuild solution: Build → Rebuild Solution
3. Restart IIS: `iisreset` (Admin prompt)
4. Verify columns again: `DESCRIBE pricing_master;`
5. Check database connection in web.config

### Issue: Foreign Key constraint error
**Solution:**
- Product and gst_master tables may not exist
- Run: `SHOW TABLES;` to list all tables
- Contact DBA to ensure all required tables exist

### Issue: Migration script won't execute
**Solution:**
- Copy script line by line if batch failing
- Check for special characters in SQL
- Verify MySQL connection is open
- Try different MySQL client (Workbench vs Command Line)

---

## Performance Optimization (Optional)
After successful migration, consider adding these indexes:
```sql
CREATE INDEX idx_pricing_status ON pricing_master(effectiveStatus);
CREATE INDEX idx_pricing_effective_dates ON pricing_master(effective_from, effective_to);
```

See `Database/pricing_master_complete.sql` for complete index definitions.

---

## Post-Implementation
- [ ] Monitor application for 24 hours
- [ ] Check error logs periodically
- [ ] Verify no other modules showing similar errors
- [ ] Plan schema validation tests for other modules
- [ ] Update deployment documentation

---

## Related Documentation
- ✅ [SOLUTION_SUMMARY.md](SOLUTION_SUMMARY.md)
- ✅ [PRICING_MASTER_FIX.md](PRICING_MASTER_FIX.md)
- ✅ [SCHEMA_RESOLUTION.md](SCHEMA_RESOLUTION.md)
- ✅ [IMPLEMENTATION_COMPLETE.md](IMPLEMENTATION_COMPLETE.md)

**Last Updated:** 2026-07-06  
**Status:** READY FOR IMPLEMENTATION
