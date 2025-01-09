using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace project_sem_6_.user
{
    public partial class working_hour : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        SqlDataReader rd;
        DataSet ds;
        Employee cs;
        string mail;

        void startcon()
        {
            con = new SqlConnection();
            cs = new Employee();
            con = cs.getcon();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            startcon();
            if (Session["username"] != null)
            {
                mail = Session["username"].ToString();
                cmd = new SqlCommand("select * from Employee_tbl where Emp_Company_Email=@Email", con);
                cmd.Parameters.AddWithValue("@Email", mail);
                rd = cmd.ExecuteReader();
                if (rd.HasRows && rd.Read())
                {
                    ViewState["W_EmpId"] = rd["Emp_Employee_Id"].ToString();
                    rd.Close();
                }
                else
                {
                    rd.Close();
                }
            }
            else
            {
                // Automatically clock out if the user was clocked in
                if (ViewState["W_EmpId"] != null)
                {
                    RecordClockOut();
                }
            }

            // Only fill the grid if W_EmpId is available
            if (ViewState["W_EmpId"] != null)
            {
                fillgrid();
            }
        }

        DataSet filldata()
        {
            da = new SqlDataAdapter("select * from Working_hour_tbl where W_EmpId=@EmpId", con);
            da.SelectCommand.Parameters.AddWithValue("@EmpId", ViewState["W_EmpId"]);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        void fillgrid()
        {
            if (ViewState["W_EmpId"] != null)
            {
                GridView1.DataSource = filldata();
                GridView1.DataBind();
            }
        }

        protected void btnClockIn_Click(object sender, EventArgs e)
        {
            RecordClockIn();
            Response.Redirect(Request.RawUrl);

        }

        protected void btnClockOut_Click(object sender, EventArgs e)
        {
            RecordClockOut();
            Response.Redirect(Request.RawUrl);
        }

        private void RecordClockIn()
        {
            if (ViewState["W_EmpId"] != null)
            {
                Session["ClockInTime"] = DateTime.Now;
                cmd = new SqlCommand("insert into Working_Hour_tbl (W_EmpId, ClockInTime, Status) VALUES (@EmpId, @ClockInTime,@Status)", con);
                cmd.Parameters.AddWithValue("@EmpId", ViewState["W_EmpId"]);
                cmd.Parameters.AddWithValue("@ClockInTime", (DateTime)Session["ClockInTime"]);
                cmd.Parameters.AddWithValue("@Status", "Active");
                cmd.ExecuteNonQuery();
            }
        }

        private void RecordClockOut()
        {
            if (Session["ClockInTime"] != null && ViewState["W_EmpId"] != null)
            {
                TimeSpan duration = DateTime.Now - (DateTime)Session["ClockInTime"];
                double countHour = duration.TotalHours;

                cmd = new SqlCommand("update Working_Hour_tbl set ClockOutTime=@ClockOutTime, CountHour=@CountHour where W_EmpId=@EmpId AND ClockOutTime IS NULL", con);
                cmd.Parameters.AddWithValue("@ClockOutTime", DateTime.Now);
                cmd.Parameters.AddWithValue("@CountHour", countHour);
                cmd.Parameters.AddWithValue("@EmpId", ViewState["W_EmpId"]);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
