# Inventory Management - Master Modules Conversion Plan

## Overview
This document outlines the complete strategy to port all master modules from the external Node.js/React application to the existing ASP.NET Web Forms application.

## Current Status

### ✅ Existing Modules (ASP.NET)
1. **Customer Master** - Fully implemented with IL class
2. **Area Master** - Fully implemented with IL class
3. **Price Master** - Stub exists (empty page)

### ❌ Modules Needing Implementation
1. **Product Master**
2. **Product Category Master**
3. **Employee Master**
4. **GST Master** (Goods & Service Tax)
5. **Pricing Master** (Complete the stub)
6. **Role Master**
7. **User Master**
8. **Product-GST Mapping**

---

## Architecture Pattern

### Frontend (ASP.NET Web Forms)
**File Structure:**
```
InventoryManagement/
├── ProductMaster.aspx
├── ProductMaster.aspx.cs
├── ProductMaster.aspx.designer.cs
├── EmployeeMaster.aspx
├── EmployeeMaster.aspx.cs
├── EmployeeMaster.aspx.designer.cs
└── [similar for other masters]
```

**Pattern for ProductMaster.aspx:**
- Form section for creating/adding new records
- GridView for displaying and editing records
- Edit/Delete/Cancel buttons per row
- Pagination support

### Backend (IL Classes - Data Access Layer)
**File Structure:**
```
InventoryManagement.IL/
├── ClsProductMaster.cs ✅ CREATED
├── ClsEmployeeMaster.cs ✅ CREATED
├── ClsGSTMaster.cs ✅ CREATED
├── ClsProductCategory.cs (TODO)
├── ClsPricingMaster.cs (TODO)
├── ClsRoleMaster.cs (TODO)
├── ClsUserMaster.cs (TODO)
└── ClsProductGSTMapping.cs (TODO)
```

**Pattern (following existing code):**
- Properties for each field matching database columns
- GetMaster() - Retrieve all records
- Create() - Insert new record
- Update() - Modify existing record
- Delete() - Remove record
- GetRelatedData() - For dropdowns (e.g., GetAreaList())

---

## Implementation Roadmap

### Phase 1: Complete Basic Structures
**Status:** In Progress

Completed:
- ✅ ClsProductMaster.cs
- ✅ ClsEmployeeMaster.cs
- ✅ ClsGSTMaster.cs

Next:
- ClsProductCategoryMaster.cs
- ClsPricingMaster.cs
- ClsRoleMaster.cs
- ClsUserMaster.cs

### Phase 2: Create ASP.NET Pages
For each Master module create:
1. MasterName.aspx (UI - Form + GridView)
2. MasterName.aspx.cs (Code-behind - Event handlers)
3. MasterName.aspx.designer.cs (Auto-generated controls)

### Phase 3: Testing & Refinement
- Unit test CRUD operations
- Validate data validation rules
- Test GridView pagination
- Verify role-based access (if applicable)

### Phase 4: Integration
- Add to master menu
- Link from dashboard
- Update navigation

---

## Key Implementation Details

### Database Tables Used

| Module | Table | Primary Key | Notes |
|--------|-------|-------------|-------|
| Product | Product | ProductID (INT) | Linked to product_category |
| Employee | employee_master | emp_code (AUTO) | New table |
| GST | gst_master | gst_id (AUTO) | New table |
| Product Category | product_category | category_id (AUTO) | Auto-increment |
| Pricing | pricing_master | pricing_id (AUTO) | Complex - tracks historical pricing |
| Role | role_master | role_id (STRING) | Semantic IDs like ROLE_ADMIN |
| User | user_master | user_id (STRING) | Password hashing required |
| Product-GST Mapping | Product_GST_Mapping | ProductID | Links Product + GST tables |

### Field Mapping Examples

**Product Fields:**
```
ProductID → PRODUCT_ID (PK)
ProductName → PRODUCT_NAME
category_id → CATEGORY_ID (FK to product_category)
UnitPrice → UNIT_PRICE
StockQuantity → STOCK_QUANTITY
available_out_delivery → AVAILABLE_OUT_DELIVERY (YES/NO)
FlexiPrice → FLEXI_PRICE (YES/NO)
```

**Employee Fields:**
```
emp_code → EMP_CODE (PK)
emp_name → EMP_NAME
designation → DESIGNATION
department → DEPARTMENT
mobile_no → MOBILE_NO
email_id → EMAIL_ID
join_date → JOIN_DATE
salary → SALARY
status → STATUS (ACTIVE/INACTIVE)
```

---

## Code Templates

### IL Class Template
```csharp
public class ClsXxxMaster
{
    // Properties matching database columns
    public int ID { get; set; }
    public string NAME { get; set; }
    
    // CRUD Methods
    public DataTable GetXxxMaster()
    public int CreateXxx(ClsXxxMaster obj)
    public int UpdateXxx(ClsXxxMaster obj)
    public int DeleteXxx(ClsXxxMaster obj)
    
    // Support Methods
    public DataTable GetRelatedData() // For dropdowns
}
```

### ASP.NET Code-Behind Template
```csharp
public partial class XxxMaster : System.Web.UI.Page
{
    readonly ClsXxxMaster objXxxMaster = new ClsXxxMaster();
    DataTable dtXxxMaster = new DataTable();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDropdowns();
            BindGridView();
        }
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    protected void grdXxxMaster_RowEditing(object sender, GridViewEditEventArgs e)
    protected void grdXxxMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    protected void grdXxxMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    
    private void BindGridView()
    private void LoadDropdowns()
    private void ClearControls()
    public void ShowResult(int rowsAffected)
}
```

---

## External System Integration (Reference)

The Node.js/React application provides these API endpoints that we're replicating in ASP.NET:

**API Endpoints (Node.js):**
```
GET    /master/customers
POST   /master/customers
PUT    /master/customers/:id
DELETE /master/customers/:id

GET    /master/products
POST   /master/products
PUT    /master/products/:id
DELETE /master/products/:id

[Similar patterns for all other masters]
```

**Our ASP.NET Approach:**
Instead of REST APIs, we use Web Forms with direct database access through IL classes.

---

## Dependencies & Prerequisites

### Database Requirements
Ensure these tables exist in MySQL:
- product_category ✅
- Product ✅
- employee_master ✅
- gst_master ✅
- pricing_master ✅
- role_master ✅
- user_master ✅
- Product_GST_Mapping ✅

### ASP.NET Requirements
- .NET Framework (existing project version)
- MySQL connector already configured
- Web Forms infrastructure in place

### Code-Behind Requirements
- Import InventoryManagement.IL namespace
- Create readonly objects for each IL class
- Implement standard CRUD event handlers

---

## Testing Checklist

### For Each Master Module
- [ ] Create new record validation
- [ ] Read/retrieve all records
- [ ] Update existing record
- [ ] Delete record
- [ ] GridView pagination works
- [ ] Dropdown lists populate correctly
- [ ] Error handling on invalid data
- [ ] ViewState management for caching
- [ ] User session handling

---

## Next Steps

1. **Create remaining IL classes** (ProductCategory, Pricing, Role, User)
2. **Create ProductMaster.aspx** as template
3. **Replicate pattern** for other modules
4. **Add navigation links** to master menu
5. **Test CRUD operations** end-to-end
6. **Validate data** against business rules

---

## Notes

- All modules follow the same CRUD pattern established in CustomerMaster and AreaMaster
- ViewState is used for client-side caching in GridView
- Transactions with rollback are implemented for data integrity
- Audit fields (CREATED_BY, UPDATED_BY, timestamps) are tracked
- Soft deletes may be needed for some modules (marked inactive vs deleted)

---

**Last Updated:** 2024
**Status:** In Progress - Phase 1
**Priority Modules:** Product, Employee, GST (most frequently used)
