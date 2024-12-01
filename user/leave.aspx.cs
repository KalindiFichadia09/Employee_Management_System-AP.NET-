using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace project_sem_6_.user
{
    public partial class leave : System.Web.UI.Page
    {
        SqlConnection con;
        Employee cs;
        string flnm, mail;

        void StartCon()
        {
            cs = new Employee();
            con = cs.getcon();
        }

        public void ImageUpload()
        {
            if (L_Document.HasFile)
            {
                flnm = "~/image/" + Path.GetFileName(L_Document.FileName);
                L_Document.SaveAs(Server.MapPath(flnm));
            }
            else
            {
                flnm = null;
            }
        }

        protected void btn_show_form_Click(object sender, EventArgs e)
        {
            UpdatePanel1.Visible = !UpdatePanel1.Visible;
        }

        protected void btn_apply_Click(object sender, EventArgs e)
        {
            try
            {
                StartCon();
                ImageUpload();

                // Calculate the number of days of leave
                DateTime startDate = DateTime.Parse(L_Start_Date.Text);
                DateTime endDate = DateTime.Parse(L_End_Date.Text);
                int leaveDays = (endDate - startDate).Days + 1;

                // Check if there are enough remaining leaves for the selected type
                string remainingLeaveQuery = "";
                switch (L_Type.SelectedValue)
                {
                    case "CL":
                        remainingLeaveQuery = "SELECT Remaining_CL FROM Employee_tbl WHERE Emp_Employee_Id = @EmpId";
                        break;

                    case "PL":
                        remainingLeaveQuery = "SELECT Remaining_PL FROM Employee_tbl WHERE Emp_Employee_Id = @EmpId";
                        break;

                    case "SL":
                        remainingLeaveQuery = "SELECT Remaining_SL FROM Employee_tbl WHERE Emp_Employee_Id = @EmpId";
                        break;
                }

                int remainingLeaves = 0;
                using (SqlCommand cmd = new SqlCommand(remainingLeaveQuery, con))
                {
                    cmd.Parameters.AddWithValue("@EmpId", ViewState["L_Emp_Id"].ToString());
                    remainingLeaves = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (remainingLeaves < leaveDays)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Not enough remaining leaves for this type. Please check your leave summary.');", true);
                    return;
                }

                // Insert leave application data
                cs.Apply_leave_insert(
                    ViewState["L_Emp_Id"].ToString(),
                    L_Type.SelectedValue,
                    L_Category.SelectedValue,
                    L_Start_Date.Text,
                    L_End_Date.Text,
                    L_Remarks.Text,
                    flnm,
                    "Pending"
                );

                // Update the leave counts in Employee_tbl
                UpdateLeaveCounts(ViewState["L_Emp_Id"].ToString(), L_Type.SelectedValue, leaveDays);

                ClearFields();
                FillGrid();
                PopulateLeaveSummary();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Leave application submitted successfully!');", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('An error occurred while applying for leave. Please try again later.');", true);
            }
            Response.Redirect(Request.RawUrl);
        }


        void UpdateLeaveCounts(string empId, string leaveType, int leaveDays)
        {
            string updateQuery = "";

            switch (leaveType)
            {
                case "CL":
                    updateQuery = @"
                UPDATE Employee_tbl
                SET Used_CL = ISNULL(Used_CL, 0) + @LeaveDays, 
                    Remaining_CL = ISNULL(Total_CL, 0) - (ISNULL(Used_CL, 0) + @LeaveDays)
                WHERE Emp_Employee_Id = @EmpId";
                    break;

                case "PL":
                    updateQuery = @"
                UPDATE Employee_tbl
                SET Used_PL = ISNULL(Used_PL, 0) + @LeaveDays, 
                    Remaining_PL = ISNULL(Total_PL, 0) - (ISNULL(Used_PL, 0) + @LeaveDays)
                WHERE Emp_Employee_Id = @EmpId";
                    break;

                case "SL":
                    updateQuery = @"
                UPDATE Employee_tbl
                SET Used_SL = ISNULL(Used_SL, 0) + @LeaveDays, 
                    Remaining_SL = ISNULL(Total_SL, 0) - (ISNULL(Used_SL, 0) + @LeaveDays)
                WHERE Emp_Employee_Id = @EmpId";
                    break;
            }

            using (SqlCommand cmd = new SqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@LeaveDays", leaveDays);
                cmd.Parameters.AddWithValue("@EmpId", empId);
                cmd.ExecuteNonQuery();
            }
        }

        void PopulateLeaveSummary()
        {
            StartCon();
            DataTable leaveSummary = new DataTable();
            string query = @"
                SELECT 
                    'Casual Leave (CL)' AS LeaveType, Total_CL AS Total, Used_CL AS Used, Remaining_CL AS Remaining
                FROM Employee_tbl
                WHERE Emp_Employee_Id = @EmpId

                UNION ALL

                SELECT 
                    'Privilege Leave (PL)' AS LeaveType, Total_PL AS Total, Used_PL AS Used, Remaining_PL AS Remaining
                FROM Employee_tbl
                WHERE Emp_Employee_Id = @EmpId

                UNION ALL

                SELECT 
                    'Sick Leave (SL)' AS LeaveType, Total_SL AS Total, Used_SL AS Used, Remaining_SL AS Remaining
                FROM Employee_tbl
                WHERE Emp_Employee_Id = @EmpId";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@EmpId", ViewState["L_Emp_Id"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(leaveSummary);
            }

            leaveSummaryTable.Rows.Clear();

            if (leaveSummary.Rows.Count > 0)
            {
                foreach (DataRow row in leaveSummary.Rows)
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    tr.Cells.Add(new HtmlTableCell { InnerText = row["LeaveType"].ToString() });
                    tr.Cells.Add(new HtmlTableCell { InnerText = row["Total"].ToString() });
                    tr.Cells.Add(new HtmlTableCell { InnerText = row["Used"].ToString() });
                    tr.Cells.Add(new HtmlTableCell { InnerText = row["Remaining"].ToString() });
                    leaveSummaryTable.Rows.Add(tr);
                }
            }
            else
            {
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell
                {
                    ColSpan = 4,
                    InnerText = "No leave data available.",
                    Align = "center"
                };
                tr.Cells.Add(tc);
                leaveSummaryTable.Rows.Add(tr);
            }
        }

        void FillGrid()
        {
            GridView1.DataSource = cs.Leave_select(ViewState["L_Emp_Id"].ToString());
            GridView1.DataBind();
        }

        void ClearFields()
        {
            L_Category.ClearSelection();
            L_Type.ClearSelection();
            L_Start_Date.Text = "";
            L_End_Date.Text = "";
            L_Remarks.Text = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StartCon();
                if (Session["username"] != null)
                {
                    mail = Session["username"].ToString();
                    using (SqlCommand cmd = new SqlCommand("SELECT Emp_Employee_Id FROM Employee_tbl WHERE Emp_Company_Email=@Email", con))
                    {
                        cmd.Parameters.AddWithValue("@Email", mail);
                        ViewState["L_Emp_Id"] = cmd.ExecuteScalar()?.ToString();
                    }
                    PopulateLeaveSummary();
                    FillGrid();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label actionLabel = (Label)e.Row.FindControl("Label9");
                if (actionLabel != null)
                {
                    string actionText = actionLabel.Text;
                    switch (actionText)
                    {
                        case "Approved":
                            actionLabel.CssClass = "text-success";
                            break;
                        case "Pending":
                            actionLabel.CssClass = "text-info";
                            break;
                        case "Declined":
                            actionLabel.CssClass = "text-danger";
                            break;
                        default:
                            actionLabel.CssClass = "text-muted";
                            break;
                    }
                }
            }
        }
    }
}
