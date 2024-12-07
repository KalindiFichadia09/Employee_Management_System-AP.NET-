using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace project_sem_6_.user
{
    public partial class Payroll : System.Web.UI.Page
    {
        SqlConnection con;
        Employee cs;
        DataSet ds;
        int id;

        void startcon()
        {
            cs = new Employee();
            con = cs.getcon();
        }

        void fillgrid()
        {
            GridView1.DataSource = cs.Payroll_filldata_user(ViewState["Emp_Id"].ToString());
            GridView1.DataBind();
        }

        void filltext()
        {
            startcon();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Payroll_tbl WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet tempDs = new DataSet();
                    da.Fill(tempDs);

                    if (tempDs.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = tempDs.Tables[0].Rows[0];
                        ViewState["id"] = row["Id"].ToString();
                        Emp_Id.Text = row["P_Emp_Id"].ToString();
                        P_Month_S.Text = row["P_Month"].ToString();
                        ViewState["month"] = P_Month_S.Text;
                        P_Year_S.Text = row["P_Year"].ToString();
                        ViewState["year"] = P_Year_S.Text;
                        P_TotalWorkingHours.Text = row["P_TotalWorkingHours"].ToString();
                        P_BaseSalary.Text = row["P_BaseSalary"].ToString();
                        P_HRA.Text = row["P_HRA"].ToString();
                        P_DA.Text = row["P_DA"].ToString();
                        P_TA.Text = row["P_TA"].ToString();
                        P_OtherAllowances.Text = row["P_OtherAllowances"].ToString();
                        P_Deductions.Text = row["P_Deductions"].ToString();
                        P_TotalPayable.Text = row["P_TotalPayable"].ToString();
                        P_NetSalary.Text = row["P_NetSalary"].ToString();
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                startcon();
                if (Session["username"] != null)
                {
                    string mail = Session["username"].ToString();
                    using (SqlCommand cmd = new SqlCommand("SELECT Emp_Employee_Id FROM Employee_tbl WHERE Emp_Company_Email = @Email", con))
                    {
                        cmd.Parameters.AddWithValue("@Email", mail);
                        ViewState["Emp_Id"] = cmd.ExecuteScalar()?.ToString();
                    }
                    fillgrid();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
        }

        private void LoadData()
        {
            int payrollId = Convert.ToInt32(ViewState["id"]);
            startcon();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Payroll_tbl WHERE Id = @Id", con);
            da.SelectCommand.Parameters.AddWithValue("@Id", payrollId);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();

            // Update ViewState with month and year from the dataset
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                ViewState["month"] = row["P_Month"]?.ToString();
                ViewState["year"] = row["P_Year"]?.ToString();
            }
        }

        protected void btn_download_Click(object sender, EventArgs e)
        {
            UpdatePanel2.Visible = true;
            ReportDocument payslip = new ReportDocument();

            try
            {
                // Ensure LoadData is called to populate the dataset
                LoadData();

                // Validate month and year from ViewState
                string month = ViewState["month"]?.ToString();
                string year = ViewState["year"]?.ToString();

                if (string.IsNullOrEmpty(month) || string.IsNullOrEmpty(year))
                {
                    throw new Exception("Month and Year cannot be empty.");
                }

                // Create directory if it does not exist
                string directoryPath = Server.MapPath("~/user/SalarySlips");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Generate the file path
                string filePath = Path.Combine(directoryPath, $"SalarySlip_{ViewState["Emp_Id"]}_{month}_{year}.pdf");

                // Load and configure the Crystal Report
                string reportPath = Server.MapPath("~/user/payslipReport.rpt");
                payslip.Load(reportPath);
                payslip.SetDataSource(ds.Tables[0]);

                // Export the report to a PDF file in the server directory
                payslip.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

                // Provide the user with the link to download the file
                string relativePath = $"~/user/SalarySlips/SalarySlip_{ViewState["Emp_Id"]}_{month}_{year}.pdf";
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", $"attachment; filename=SalarySlip_{ViewState["Emp_Id"]}_{month}_{year}.pdf");
                Response.TransmitFile(Server.MapPath(relativePath));
                Response.End();
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmd_action")
            {
                id = Convert.ToInt32(e.CommandArgument);
                UpdatePanel1.Visible = true;
                filltext();
                LoadData();

                try
                {
                    // Create directory if it does not exist
                    string directoryPath = Server.MapPath("~/user/Data(XML)");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Validate month and year from ViewState
                    string month = ViewState["month"]?.ToString();
                    string year = ViewState["year"]?.ToString();

                    if (string.IsNullOrEmpty(month) || string.IsNullOrEmpty(year))
                    {
                        throw new Exception("Month and Year cannot be empty.");
                    }

                    // Generate the schema path
                    string schemaPath = Path.Combine(directoryPath, $"payslipdata_{ViewState["Emp_Id"]}_{month}_{year}.xml");
                    ds.WriteXmlSchema(schemaPath);
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                    throw new Exception("Error writing XML schema: " + ex.Message, ex);
                }
            }
        }
    }
}
