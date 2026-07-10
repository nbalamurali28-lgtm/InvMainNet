# Inventory Management System - Deployment Guide

## Build Status: ✅ SUCCESS

Both the IL (Data Access) and Web Application projects compiled successfully with **0 errors**.

### Compiled Components
- **InventoryManagement.IL.dll** - Data access layer (13 classes)
- **InventoryManagement.dll** - Web application (16 ASPX pages)

### Master Pages Implemented (8 modules)
1. ✅ **ProductMaster.aspx** - Product catalog management
2. ✅ **EmployeeMaster.aspx** - Employee records
3. ✅ **CategoryMaster.aspx** - Product categories
4. ✅ **GSTMaster.aspx** - GST tax rates
5. ✅ **PricingMaster.aspx** - Product pricing with effective dates
6. ✅ **RoleMaster.aspx** - RBAC role definitions
7. ✅ **UserMaster.aspx** - User accounts and roles
8. ✅ **ProductGSTMapping.aspx** - Product-GST relationships

### Data Access Classes (InventoryManagement.IL)
All classes support MySQL database operations with parameterized queries:
- ClsProductMaster
- ClsProductCategoryMaster
- ClsEmployeeMaster
- ClsGSTMaster
- ClsPricingMaster
- ClsRoleMaster
- ClsUserMaster
- ClsProductGSTMapping
- ClsUtility (connection and transaction management)
- Plus existing classes (ClsAreaMaster, ClsCustomerMaster, ClsDeliveryInformation, ClsLogin)

---

## Deployment Methods

### Option 1: IIS Express (Quickest - Recommended for Testing)
IIS Express is pre-configured in the project.

**Using Visual Studio 2019 or later:**
```
1. Open InventoryManagement.slnx in Visual Studio
2. Press F5 (Debug) or Ctrl+F5 (Run without debugging)
3. Application will open in browser at https://localhost:44334
```

**Using VS Code with IIS Express extension:**
```
1. Install "IIS Express" extension in VS Code
2. Click IIS Express icon in sidebar
3. Select InventoryManagement project
4. Click "Start"
```

### Option 2: IIS (Production Deployment)

**Prerequisites:**
- IIS role installed on Windows
- .NET Framework 4.7.2 Runtime
- MySQL database running

**Steps:**

1. **Copy Application Files**
   ```
   Source: E:\RamInv\Inventory-Management-.NET-Source-Code-main\InventoryManagement
   Destination: C:\inetpub\wwwroot\InventoryManagement
   ```

2. **Create IIS Website**
   - Open IIS Manager (inetmgr.exe)
   - Right-click "Sites" → Add Website
   - **Name:** InventoryManagement
   - **Physical Path:** C:\inetpub\wwwroot\InventoryManagement
   - **Binding:** http, localhost, Port 8080 (or your choice)
   - **Application Pool:** DefaultAppPool (or create new with .NET 4.7.2)

3. **Configure Application Pool**
   - Right-click Application Pool → Basic Settings
   - .NET Framework version: v4.0.30319
   - Pipeline Mode: Integrated

4. **Database Connection**
   - Verify MySQL is running
   - Default connection in web.config: `Server=localhost; Database=inventory_management; Uid=root; Pwd=root;`
   - Create database if not exists: `CREATE DATABASE inventory_management;`

5. **Set Permissions**
   ```
   icacls "C:\inetpub\wwwroot\InventoryManagement" /grant "IIS_IUSERS:(OI)(CI)F" /T
   ```

6. **Test**
   - Navigate to http://localhost:8080 (or configured port)
   - Login page should load

---

## Testing Checklist

After deployment:

- [ ] Default.aspx loads (home page)
- [ ] Login.aspx is accessible
- [ ] Navigation menu displays "Masters" dropdown with 10 items
- [ ] ProductMaster.aspx loads and displays grid
- [ ] ProductMaster form shows Category dropdown with data
- [ ] Create new product and verify it appears in grid
- [ ] Edit and delete product operations work
- [ ] All 8 new master pages (Product, Employee, Category, GST, Pricing, Role, User, ProductGSTMapping) load
- [ ] Each master page shows records from MySQL database
- [ ] Form validations work (required fields)
- [ ] Session tracking logs operations

---

## Build Files Location

```
Compiled Assemblies:
E:\RamInv\Inventory-Management-.NET-Source-Code-main\InventoryManagement\InventoryManagement\bin\

Key Files:
- InventoryManagement.dll (web application)
- InventoryManagement.IL.dll (data layer)
- All NuGet dependencies (MySql.Data, etc.)

Web Content:
- *.aspx files (pages)
- *.Master (master pages)
- Content/ (CSS, Bootstrap)
- Scripts/ (JavaScript)
- bin/web.config (configuration)
```

---

## Troubleshooting

**Issue: "Parser Error - Could not load type 'InventoryManagement.ProductMaster'"**
- **Solution:** Ensure all ASPX pages are in project .csproj file (already fixed in this build)

**Issue: "The type 'ClsProductMaster' could not be found"**
- **Solution:** Ensure InventoryManagement.IL.dll is in bin folder and web.config has proper reference

**Issue: MySQL connection fails**
- **Solution:** 
  - Verify MySQL service is running
  - Check connection string in web.config
  - Ensure database exists
  - Verify credentials (root/root by default)

**Issue: "Access Denied" in IIS**
- **Solution:** Run `icacls` command above to grant IIS app pool permissions to application folder

---

## Build Commands Used

```powershell
# Clean and build IL project
msbuild InventoryManagement.IL\InventoryManagement.IL.csproj /t:Clean,Build

# Clean and build main web application  
msbuild InventoryManagement\InventoryManagement.csproj /t:Rebuild

# Restore NuGet packages
msbuild InventoryManagement\InventoryManagement.csproj /t:Restore
```

---

## Project Structure

```
InventoryManagement/
├── InventoryManagement.slnx (Solution file)
├── InventoryManagement/ (Web Application)
│   ├── bin/ (Compiled DLLs)
│   ├── *.aspx (ASPX pages)
│   ├── *.aspx.cs (Code-behind)
│   ├── *.Master (Master pages)
│   ├── App_Start/ (Route and bundle config)
│   ├── Content/ (CSS, Bootstrap)
│   ├── Scripts/ (JavaScript)
│   └── web.config
├── InventoryManagement.IL/ (Data Access Layer)
│   ├── Cls*.cs (Data classes)
│   ├── bin/ (Compiled IL.dll)
│   └── InventoryManagement.IL.csproj
├── packages/ (NuGet dependencies)
└── DEPLOYMENT_GUIDE.md (This file)
```

---

## Next Steps

1. Choose deployment method (IIS Express recommended for initial testing)
2. Start the application
3. Test master pages functionality
4. Verify database CRUD operations
5. Configure for production IIS if needed
6. Deploy to Internet Explorer by browsing to application URL

