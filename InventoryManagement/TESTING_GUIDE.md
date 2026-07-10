# Inventory Management System - Testing Guide

## Pre-Test Setup Checklist

- [ ] MySQL service running (Server=localhost, Database=inventory_management, Uid=root, Pwd=root)
- [ ] Application successfully built (0 errors)
- [ ] InventoryManagement.dll and InventoryManagement.IL.dll in bin folder
- [ ] web.config connection string verified

---

## Test Method 1: IIS Express (Recommended - Fastest)

### Start Application
```
Option A: Visual Studio 2019+
1. Open solution: E:\RamInv\Inventory-Management-.NET-Source-Code-main\InventoryManagement\InventoryManagement.slnx
2. Right-click "InventoryManagement" project → Set as Startup Project
3. Press F5 (Debug) or Ctrl+F5 (Run without debugging)
4. Browser opens to https://localhost:44334

Option B: Command Line
cd E:\RamInv\Inventory-Management-.NET-Source-Code-main\InventoryManagement
"C:\Program Files\IIS Express\iisexpress.exe" /path:InventoryManagement /port:8080
```

### First Page Load Test
- [ ] Application starts without errors
- [ ] Page loads in < 3 seconds
- [ ] No "yellow screen of death" (YSOD) errors
- [ ] Default.aspx displays successfully

---

## Test Method 2: IIS (Full Production Test)

### Manual IIS Setup & Test
```powershell
# PowerShell (Run as Administrator)

# 1. Copy application to IIS folder
Copy-Item -Path "E:\RamInv\Inventory-Management-.NET-Source-Code-main\InventoryManagement\InventoryManagement" `
          -Destination "C:\inetpub\wwwroot\InventoryManagement" -Recurse -Force

# 2. Set app pool permissions
icacls "C:\inetpub\wwwroot\InventoryManagement" /grant "IIS_IUSERS:(OI)(CI)F" /T

# 3. Open IIS Manager
inetmgr.exe
```

**Then in IIS Manager:**
1. Click "Sites" folder
2. Right-click → "Add Website"
3. **Site Name:** InventoryManagement
4. **Physical Path:** C:\inetpub\wwwroot\InventoryManagement
5. **Binding:** 
   - Type: http
   - IP: All Unassigned
   - Port: 8080
   - Host: localhost
6. Click OK
7. Navigate to http://localhost:8080

---

## Core Functionality Tests

### Test 1: Home Page & Navigation
```
Steps:
1. Load application home page
2. Verify page title: "Inventory Management System" or similar
3. Check navigation menu loads
4. Hover over "Masters" dropdown
5. Verify all 10 items visible:
   - Area Master
   - Customer Master
   - Product Master (NEW)
   - Product Category (NEW)
   - Employee Master (NEW)
   - GST Master (NEW)
   - Pricing Master (NEW)
   - Role Master (NEW)
   - User Master (NEW)
   - Product-GST Mapping (NEW)

Expected Result: ✅ All menu items appear, no JavaScript errors
```

### Test 2: Database Connectivity
```
Steps:
1. Open browser Developer Tools (F12)
2. Go to Console tab
3. Click any master page (e.g., Product Master)
4. Check console for JavaScript errors

Expected Result: ✅ No errors, page loads grid with data
```

### Test 3: ProductMaster Page (Core Test)
```
URL: http://localhost:8080/ProductMaster.aspx (or IIS Express equivalent)

Visual Inspection:
- [ ] Page title displays: "Product Master"
- [ ] Form appears on left side with fields:
    * Product Name (TextBox) - required
    * Category (DropDownList) - populated from database
    * Description (TextBox)
    * Unit Price (TextBox, decimal)
    * Stock Quantity (TextBox, number)
    * Reorder Level (TextBox)
    * Weight (TextBox)
    * Gas Type (TextBox)
    * Is Active (CheckBox) - checked by default
    * Available Out Delivery (CheckBox)
    * Flexi Price (CheckBox)
- [ ] Save and Reset buttons present
- [ ] GridView appears on right side
- [ ] Grid columns visible: ProductID, ProductName, Category, UnitPrice, StockQuantity, IsActive, Edit, Delete

Database Check:
- [ ] Grid contains existing products from database
- [ ] Category dropdown shows values (e.g., "Electronics", "Furniture")
- [ ] If empty, check MySQL database has data
```

### Test 4: Create Product (CRUD - Create)
```
Steps:
1. In Product Master form, enter:
   - Product Name: "Test Product 001"
   - Category: Select from dropdown
   - Unit Price: 99.99
   - Stock Quantity: 100
   - Click Save

Expected Results:
- [ ] Success message appears: "Product created successfully" or similar (green alert)
- [ ] New product appears in grid below
- [ ] Grid refreshes automatically
- [ ] Session logs operation (check browser console for traces if logging enabled)

Validation Test:
- [ ] Try clicking Save without Product Name → Should show error: "Product Name is required"
- [ ] Try clicking Save without Category → Should show error: "Category is required"
```

### Test 5: Read Product (CRUD - Read)
```
Steps:
1. Observe GridView data
2. Select a product row
3. Check all columns display correct data:
   - Product ID (numeric)
   - Product Name (text)
   - Category Name (text from joined table)
   - Unit Price (formatted as currency if configured)
   - Stock Quantity (numeric)
   - Is Active (checkbox or Yes/No)

Expected Result: ✅ All data displays correctly, matches database
```

### Test 6: Update Product (CRUD - Update)
```
Steps:
1. Click Edit button on any row in grid
2. Form populates with current values
3. Modify a field (e.g., change Unit Price)
4. Click Save

Expected Results:
- [ ] Success message: "Product updated successfully"
- [ ] Grid refreshes with new values
- [ ] Database record actually updated (verify in MySQL)

Check:
- [ ] Original product is still there
- [ ] Modified field shows new value
- [ ] All other fields unchanged
```

### Test 7: Delete Product (CRUD - Delete)
```
Steps:
1. Click Delete button on any product row
2. Browser may show confirm dialog: "Are you sure you want to delete?"
3. Confirm deletion

Expected Results:
- [ ] Success message: "Product deleted successfully"
- [ ] Product removed from grid
- [ ] Grid row count decreased by 1
- [ ] Database record deleted (verify in MySQL)
```

---

## Master Page Tests (8 New Modules)

Test each master page following the same CRUD pattern as ProductMaster:

### Test 8: EmployeeMaster.aspx
```
URL: /EmployeeMaster.aspx

Form Fields to Verify:
- Employee Name (required)
- Designation (text)
- Department (text)
- Mobile Number (text)
- Email (text, should validate email format)
- Address (text)
- City (text)
- State (text)
- Pincode (numeric)
- Join Date (date picker)
- Salary (decimal)
- Status (DropDownList: ACTIVE/INACTIVE/ONLEAVE)

Test Operations:
- [ ] Create: Add new employee with all fields
- [ ] Read: Grid displays employee records
- [ ] Update: Edit employee record
- [ ] Delete: Remove employee record
- [ ] Verify: Check MySQL employee_master table
```

### Test 9: CategoryMaster.aspx
```
URL: /CategoryMaster.aspx

Form Fields:
- Category Name (required text)

Test:
- [ ] Create: Add "Test Category"
- [ ] Grid updates with new category
- [ ] Edit category name
- [ ] Delete category (if not in use by products)
- [ ] Verify ProductMaster Category dropdown includes new category

Expected: Category dropdown in ProductMaster reflects changes
```

### Test 10: GSTMaster.aspx
```
URL: /GSTMaster.aspx

Form Fields:
- GST Percentage (required decimal, e.g., 18.00)
- Description (text)
- Effective From (date picker)
- Effective To (date picker)
- Is Active (checkbox)

Test:
- [ ] Create: Add "18%" GST
- [ ] Grid shows: gst_id, gst_percentage, description, is_active
- [ ] Edit GST percentage
- [ ] Delete if not used in pricing
- [ ] Check PricingMaster GST dropdown reflects changes
```

### Test 11: PricingMaster.aspx
```
URL: /PricingMaster.aspx

Form Fields:
- Product (DropDownList - from products, required)
- Base Price (decimal, required)
- GST (DropDownList - from GST master, required)
- Save button

Test:
- [ ] Product dropdown populates from ProductMaster
- [ ] GST dropdown populates from GSTMaster
- [ ] Create pricing: Select product, enter base price, select GST
- [ ] Grid displays: pricing_id, ProductName, base_price, gst_percentage, effective_from, effective_to
- [ ] Complex join query works correctly
```

### Test 12: RoleMaster.aspx
```
URL: /RoleMaster.aspx

Form Fields:
- Role ID (semantic text, e.g., "ROLE_ADMIN", "ROLE_USER", required)
- Role Name (required)
- Description (text)
- Is Active (checkbox)

Test:
- [ ] Create: Role ID = "ROLE_MANAGER", Role Name = "Manager"
- [ ] Grid displays roles
- [ ] Edit role name
- [ ] Check UserMaster role dropdown reflects roles
```

### Test 13: UserMaster.aspx
```
URL: /UserMaster.aspx

Form Fields:
- Username (required, text)
- Password (required, TextMode=Password, only on creation)
- Full Name (text)
- Email (text, should validate)
- Role (DropDownList, from RoleMaster, required)
- Is Active (checkbox)

Test:
- [ ] Create new user:
    * Username: "testuser"
    * Password: "Test@123"
    * Full Name: "Test User"
    * Email: "test@example.com"
    * Role: Select from dropdown
    * Save
- [ ] User ID auto-generated (verify in grid)
- [ ] Grid shows: user_id, username, full_name, role_name, email, is_active
- [ ] Edit: Try changing username → Should update
- [ ] Edit: Password field NOT shown (only on creation)
- [ ] Verify role_name joins correctly from role_master table

Database Check:
- [ ] Query: SELECT * FROM user_master WHERE username='testuser';
- [ ] Verify: user_id, username, full_name, role_id, email, is_active present
```

### Test 14: ProductGSTMapping.aspx
```
URL: /ProductGSTMapping.aspx

Form Fields:
- Product (DropDownList - required)
- GST (DropDownList - required)
- "Assign GST" button

Test:
- [ ] Product dropdown populates
- [ ] GST dropdown populates
- [ ] Select a product and GST
- [ ] Click "Assign GST"
- [ ] Record appears in grid
- [ ] Grid columns: ProductID, ProductName, gst_percentage
- [ ] Click Delete on a mapping to remove

Expected: Junction table Product_GST_Mapping updated in database
```

---

## Advanced Tests

### Test 15: Database Integration
```
Steps:
1. Open MySQL Workbench or command line
2. Login: mysql -u root -p (password: root)
3. Select database: USE inventory_management;
4. Run query: SHOW TABLES;

Verify Tables Exist:
- [ ] product (products)
- [ ] product_category (categories)
- [ ] employee_master (employees)
- [ ] gst_master (tax rates)
- [ ] pricing_master (pricing)
- [ ] role_master (roles)
- [ ] user_master (users)
- [ ] product_gst_mapping (product-GST relationships)

Then for each table:
1. INSERT test record via web form
2. Query table: SELECT * FROM [table_name];
3. Verify record appears in results
4. DELETE record via web form
5. Query table: Verify record deleted
```

### Test 16: Form Validation
```
For each master page:
1. Try submitting form with missing required fields
2. Expected: Red error message (alert-danger)
3. Check specific validations:
   
ProductMaster:
- [ ] Product Name required
- [ ] Category required
- [ ] Unit Price accepts only numbers
- [ ] Stock Quantity accepts only integers

UserMaster:
- [ ] Username required
- [ ] Password required (on create)
- [ ] Email format validated (if regex applied)
- [ ] Role required

PricingMaster:
- [ ] Product required
- [ ] Base Price required
- [ ] GST required
```

### Test 17: GridView Operations
```
For each master page GridView:
1. Inline Edit (click Edit button in row)
   - [ ] Form fields populate with row data
   - [ ] Other fields cleared or auto-populated
   - [ ] Save updates the row
   - [ ] Cancel restores original data

2. Inline Delete (click Delete button in row)
   - [ ] Confirm dialog appears (if configured)
   - [ ] Row removed from grid
   - [ ] Database record deleted

3. Pagination (if applicable)
   - [ ] Grid shows "Page 1 of X"
   - [ ] Click "Next" navigates pages
   - [ ] Click "Previous" goes back
```

### Test 18: Session & Audit Tracking
```
Steps:
1. Open browser Developer Tools (F12)
2. Go to Application → Cookies
3. Find Session cookie: "ASP.NET_SessionId"
4. Note session ID

Then:
1. Create/Update/Delete any record
2. Check application code-behind for Session["UserID"]
3. If logging implemented:
   - [ ] Audit table updated with User ID
   - [ ] Operation (Create/Update/Delete) logged
   - [ ] Timestamp recorded
   - [ ] Query: SELECT * FROM audit_log WHERE user_id = [session_id];
```

---

## Error Testing (Negative Tests)

### Test 19: Database Offline
```
Steps:
1. Stop MySQL service: net stop MySQL80 (or your version)
2. Try to load any master page
3. Expected: Error message (not crash)
   Example: "Unable to connect to database. Please check connection string."

Then:
1. Restart MySQL: net start MySQL80
2. Retry page → Should work again
```

### Test 20: Invalid Inputs
```
For each text input:
1. Try SQL injection: ' OR '1'='1
   Expected: ✅ Should fail or be parameterized (no injection possible)

2. Try XSS: <script>alert('XSS')</script>
   Expected: ✅ Should be encoded/escaped

3. Try very long input (1000+ chars)
   Expected: ✅ Should either truncate or show validation error

4. Try special characters: !@#$%^&*()
   Expected: ✅ Should accept or show appropriate error
```

---

## Performance Tests

### Test 21: Page Load Time
```
Steps:
1. Open Developer Tools (F12) → Network tab
2. Navigate to ProductMaster.aspx
3. Observe "Finish" time at bottom

Expected:
- [ ] Page fully loads in < 3 seconds
- [ ] No failed requests (all HTTP 200 or 304)
- [ ] All resources load (CSS, JS, images)
```

### Test 22: Large Dataset
```
Steps:
1. Insert 1000+ products via MySQL or via form repeatedly
2. Load ProductMaster.aspx
3. Check:
   - [ ] Grid still responsive
   - [ ] No timeout errors
   - [ ] Pagination works (if configured)
   - [ ] Filtering works (if implemented)
```

---

## Browser Compatibility Test (Internet Explorer)

### Test 23: Internet Explorer 11
```
Steps:
1. Open Internet Explorer 11
2. Navigate to http://localhost:8080 (or IIS Express port)
3. Check:
   - [ ] Page renders correctly (no layout broken)
   - [ ] Bootstrap CSS works
   - [ ] Forms functional
   - [ ] Buttons clickable
   - [ ] No console errors (F12 → Console)
   - [ ] AJAX calls work if implemented
   
Known IE Issues to Watch:
- [ ] CSS Grid might not work (use flexbox)
- [ ] ES6 JavaScript might fail (use ES5 polyfills if needed)
- [ ] DatePicker might need IE-compatible plugin
```

### Test 24: Other Browsers
```
Test in:
- [ ] Chrome (latest)
- [ ] Firefox (latest)
- [ ] Edge (Chromium-based)
- [ ] Safari (if on Mac)

For each, verify:
- [ ] Layout displays correctly
- [ ] Forms work
- [ ] No console errors
- [ ] CRUD operations succeed
```

---

## Final Sign-Off Checklist

```
Functionality:
- [ ] All 8 master pages load without errors
- [ ] CRUD operations work (Create, Read, Update, Delete)
- [ ] Database records persist
- [ ] Form validation works
- [ ] Navigation menu complete

Database:
- [ ] MySQL connection established
- [ ] All tables accessible
- [ ] Data inserts correctly
- [ ] Data updates correctly
- [ ] Data deletes correctly

UI/UX:
- [ ] Bootstrap styling applied
- [ ] Forms look professional
- [ ] Alerts display (success/error/warning)
- [ ] GridView displays data clearly
- [ ] Buttons responsive

Performance:
- [ ] Pages load quickly (< 3s)
- [ ] No 500 errors
- [ ] No timeout issues

Security:
- [ ] No SQL injection possible
- [ ] Parameterized queries used
- [ ] Session management working

Browser:
- [ ] Works in Internet Explorer 11
- [ ] Works in Chrome/Firefox/Edge
```

---

## Quick Test Script

```powershell
# Run this to quickly verify application is running

$url = "http://localhost:8080"  # Change if using different port

# Test if page loads
try {
    $response = Invoke-WebRequest -Uri $url -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Application is running!"
        Write-Host "Status: $($response.StatusCode)"
    }
} catch {
    Write-Host "❌ Failed to load application"
    Write-Host "Error: $_"
    Write-Host "Make sure IIS/IIS Express is running on port 8080"
}
```

---

## Troubleshooting Failed Tests

| Issue | Cause | Solution |
|-------|-------|----------|
| Page won't load (404) | Application not deployed | Check IIS binding, use IIS Express |
| Yellow Screen of Death (YSOD) | Code error | Check web.config debug=true, check event viewer |
| "Could not open connection to database" | MySQL not running | Start MySQL service |
| Grid empty | No data in table | Insert test data via INSERT statement |
| Category dropdown empty | CategoryMaster records don't exist | Create category first |
| Form won't submit | Validation error | Check required fields filled |
| Edit doesn't work | GridView RowEditing not firing | Check AutoEventWireup="true" in .aspx |
| Delete fails | Foreign key constraint | Delete dependent records first |

