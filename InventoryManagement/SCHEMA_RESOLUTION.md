# Pricing Master - Schema Resolution Guide

## Problem
**Error:** "Error loading dropdowns: Unknown column 'effective_from' in 'field list'"  
**Module:** PricingMaster.aspx  
**Root Cause:** The `gst_master` table schema is incomplete. When PricingMaster tries to load the GST dropdown, it queries for `effective_from` and `effective_to` columns that don't exist.

---

## Solution Steps

### Step 1: Execute Database Migration
Run the following SQL migration to add the missing columns to the `gst_master` table:

```sql
ALTER TABLE gst_master 
ADD COLUMN IF NOT EXISTS effective_from DATETIME NULL AFTER description,
ADD COLUMN IF NOT EXISTS effective_to DATETIME NULL AFTER effective_from;
```

**File Location:** `/InventoryManagement/Database/migrate_gst_master.sql`

### Step 2: Verify the Migration
After running the migration, verify all columns exist:

```sql
-- Check gst_master table structure
DESCRIBE gst_master;

-- Or use INFORMATION_SCHEMA:
SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'inventory_management' 
  AND TABLE_NAME = 'gst_master'
ORDER BY ORDINAL_POSITION;
```

**Expected Columns:**
- `gst_id` (INT, Primary Key, Auto Increment)
- `gst_percentage` (DECIMAL)
- `description` (VARCHAR)
- `effective_from` (DATETIME, NULL) ✅ **NEW**
- `effective_to` (DATETIME, NULL) ✅ **NEW**
- `is_active` (TINYINT)
- `created_by` (VARCHAR)
- `created_at` (DATETIME)
- `updated_by` (VARCHAR, NULL)
- `updated_at` (DATETIME, NULL)

### Step 3: Verify Database Connection
Ensure your `web.config` has correct database connection string:

```xml
<configuration>
  <connectionStrings>
    <add name="InventoryManagementEntities" 
         connectionString="Server=localhost;Database=inventory_management;Uid=root;Pwd=your_password;" 
         providerName="MySql.Data.MySqlClient" />
  </connectionStrings>
</configuration>
```

### Step 4: Test the Pricing Master Page
1. Rebuild the solution
2. Navigate to the Pricing Master page
3. The dropdowns should now load without errors

---

## Code Context

### ClsGSTMaster.cs - Expected Table Structure
```csharp
public DataTable GetGSTMaster()
{
    // This query expects these columns from gst_master:
    // - gst_id
    // - gst_percentage
    // - description
    // - effective_from          ✅ NOW AVAILABLE
    // - effective_to            ✅ NOW AVAILABLE
    // - is_active
}
```

### PricingMaster.aspx.cs - LoadDropdowns() Method
```csharp
// This method loads the GST dropdown:
// DataTable dtGST = objGSTMaster.GetGSTMaster();  
// ↑ This calls the query above, which requires effective_from and effective_to
```

### Error Trigger Point
The error occurs in **LoadDropdowns()** when it tries to populate the GST dropdown. The GST dropdown is used in the form when adding/editing pricing records.

---

## Additional Notes

### Schema Design Rationale
- **effective_from / effective_to:** Support for time-based GST rate validity. Allows the system to:
  - Maintain GST rate history over time
  - Apply different rates based on effective dates
  - Query active GST rates for a specific date range
  
- **Audit Fields (created_by, created_at, updated_by, updated_at):** 
  - Track who created/modified GST records
  - Track when changes occurred
  - Required for audit trails and compliance

---

## Related Files Updated
- `migrate_gst_master.sql` - Migration script
- `SCHEMA_RESOLUTION.md` - This document

## Testing Checklist
- [ ] Migration script executed successfully
- [ ] All new columns verified in database
- [ ] Application rebuilt
- [ ] Pricing Master page loads without errors
- [ ] Product dropdown populates correctly
- [ ] GST dropdown populates correctly ✅ (This was failing)
- [ ] Create new pricing record works
- [ ] Edit/Delete operations work as expected
