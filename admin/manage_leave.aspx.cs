using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Data;


namespace project_sem_6_.admin
{
    public partial class manage_leave : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        SqlDataReader rd;
        DataSet ds;
        Employee cs;

        void startcon()
        {
            con = new SqlConnection();
            cs = new Employee();
            con = cs.getcon();
        }

        void fillgrid()
        {
            GridView1.DataSource = cs.Leave_filldata();
            GridView1.DataBind();
        }
        void filltext()
        {
            ds = new DataSet();
            ds = cs.Leave_select_admin(Convert.ToInt16(ViewState["id"]));
            Emp_Id.Text = (ds.Tables[0].Rows[0][1]).ToString();
            Leave_Type.Text = (ds.Tables[0].Rows[0][2]).ToString();
            Leave_Category.Text = (ds.Tables[0].Rows[0][3]).ToString();
            Leave_Start_Date.Text = (ds.Tables[0].Rows[0][4]).ToString();
            Leave_End_Date.Text = (ds.Tables[0].Rows[0][5]).ToString();
            Leave_Remark.Text = (ds.Tables[0].Rows[0][6]).ToString();
            Leave_Action.Text = (ds.Tables[0].Rows[0][8]).ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            startcon();
            fillgrid();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt16(e.CommandArgument);
            ViewState["id"] = id;
            if (e.CommandName == "cmd_action")
            {
                UpdatePanel1.Visible = true;
                filltext();
            }
        }

        protected void btn_approve_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["id"] == null)
                {
                    Response.Write("<script>alert('Error: No record selected.');</script>");
                    return;
                }

                string l_action = "Approved";
                int leaveDays = 0;
                string leaveType = string.Empty;
                string empId = string.Empty;

                // Fetch leave details using ViewState["id"]
                cmd = new SqlCommand("SELECT L_Emp_Id, L_Type, L_Start_date, L_End_Date FROM Leave_tbl WHERE Id = @id", con);
                cmd.Parameters.AddWithValue("@id", ViewState["id"]);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    empId = reader["L_Emp_Id"].ToString();
                    leaveType = reader["L_Type"].ToString();
                    DateTime startDate = DateTime.Parse(reader["L_Start_date"].ToString());
                    DateTime endDate = DateTime.Parse(reader["L_End_Date"].ToString());
                    leaveDays = (endDate - startDate).Days + 1; // Inclusive of start and end date
                }
                else
                {
                    Response.Write("<script>alert('Error fetching leave details.');</script>");
                    reader.Close();
                    return;
                }
                reader.Close();

                // Update leave action
                cmd = new SqlCommand("UPDATE Leave_tbl SET L_Action = @action WHERE Id = @id", con);
                cmd.Parameters.AddWithValue("@action", l_action);
                cmd.Parameters.AddWithValue("@id", ViewState["id"]);
                cmd.ExecuteNonQuery();

                // Determine leave column to update
                string updateColumn = string.Empty;
                if (leaveType == "CL") updateColumn = "Used_CL";
                else if (leaveType == "PL") updateColumn = "Used_PL";
                else if (leaveType == "SL") updateColumn = "Used_SL";
                else
                {
                    Response.Write("<script>alert('Invalid leave type.');</script>");
                    return;
                }

                // Update leave count in Employee_tbl
                cmd = new SqlCommand($@"
                    UPDATE Employee_tbl 
                    SET {updateColumn} = {updateColumn} + @leaveDays 
                    WHERE Emp_Employee_Id = @empId AND Remaining_{leaveType} >= @leaveDays", con);
                cmd.Parameters.AddWithValue("@leaveDays", leaveDays);
                cmd.Parameters.AddWithValue("@empId", empId);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Successfully updated leave count
                    fillgrid();
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    // Insufficient remaining leaves
                    cmd = new SqlCommand("UPDATE Leave_tbl SET L_Action = 'Pending' WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", ViewState["id"]);
                    cmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Insufficient remaining leaves. Leave cannot be approved.');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }

        protected void btn_decline_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["id"] == null)
                {
                    Response.Write("<script>alert('Error: No record selected.');</script>");
                    return;
                }

                string l_action = "Declined";
                cmd.Parameters.AddWithValue("@action", l_action);
                cmd.Parameters.AddWithValue("@action", l_action);
                cmd.Parameters.AddWithValue("@id", ViewState["id"]);
                cmd.ExecuteNonQuery();

                fillgrid();
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }

    }
}