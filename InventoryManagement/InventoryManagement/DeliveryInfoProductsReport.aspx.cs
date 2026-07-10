using InventoryManagement.IL;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class DeliveryInfoProductsReport : System.Web.UI.Page
    {
        public ClsDeliveryInformation objDeliveryInformation = new ClsDeliveryInformation();
        decimal footerTotalQuantity, footerTotalAmount = 0.00M;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Optionally, load default data here
                    TxtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    TxtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtFromDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select a From Date.');", true);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtToDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select To Date.');", true);
                    return;
                }

                DataTable dt = new DataTable();
                dt = objDeliveryInformation.GetDeliveryInfoProductsReport(Convert.ToDateTime(TxtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(TxtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss"));

                GrdViewDeliveryInfo.DataSource = dt;
                GrdViewDeliveryInfo.DataBind();

                /*if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showSuccessMessage('No records found for the selected date range.','Success');", true);
                }*/
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GrdViewDeliveryInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Check if the current row is a data row (not header/footer)
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //e.Row.Cells[0].CssClass = "bg-primary text-white";
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].CssClass = "bg-primary text-white";
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].CssClass = "alignDataRowTextRight"; // Delivery Id

                    footerTotalQuantity = footerTotalQuantity + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Quantity"));
                    e.Row.Cells[4].CssClass = "alignDataRowTextRight"; // Quantity

                    e.Row.Cells[5].CssClass = "alignDataRowTextRight"; // Price

                    footerTotalAmount = footerTotalAmount + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
                    e.Row.Cells[6].CssClass = "alignDataRowTextRight"; // Total Amount
                }

                // Check if the current row being bound is the footer
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    // Access cells by index and set text
                    // You can use a calculated variable here
                    e.Row.Cells[4].Text = Convert.ToString(footerTotalQuantity);
                    e.Row.Cells[4].CssClass = "alignFooterTextRight"; // Quantity

                    e.Row.Cells[6].Text = Convert.ToString(footerTotalAmount);
                    e.Row.Cells[6].CssClass = "alignFooterTextRight"; // Amount
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtFromDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select a From Date.');", true);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtToDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select To Date.');", true);
                    return;
                }

                if (GrdViewDeliveryInfo.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showSuccessMessage('No records found for the selected date range.','Error');", true);
                    return;
                }

                // 1. Clear response and set headers for Excel download
                string filename = "Delivery_Info_Products_Report_" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).Replace(":", "-") + ".xls";
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                // 2. Disable paging to export all data
                GrdViewDeliveryInfo.AllowPaging = false;
                // BindData(); // Re-bind data source here

                // 3. Render GridView to StringWriter
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    GrdViewDeliveryInfo.RenderControl(hw);
                    Response.Output.Write(sw.ToString());
                }

                Response.Flush();
                Response.End();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // 4. Override to bypass form validation for GridView
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required for RenderControl
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/DeliveryInformationReport.aspx", true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}