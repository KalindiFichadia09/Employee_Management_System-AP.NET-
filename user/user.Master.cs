using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace project_sem_6_.user
{
    public partial class user : System.Web.UI.MasterPage
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        SqlDataReader rd;
        Employee cs;

        void startcon()
        {
            con = new SqlConnection();
            cs = new Employee();
            con = cs.getcon();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                startcon();
                cmd = new SqlCommand("select * from Employee_tbl where Emp_Company_Email=@Email", con);
                cmd.Parameters.AddWithValue("@Email", Session["username"].ToString());
                rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        Emp_Username.Text = rd["Emp_Full_Name"].ToString();
                        Emp_Image.Attributes["src"] = rd["Emp_Image"].ToString();
                        ViewState["empID"] = rd["Emp_Employee_Id"].ToString();
                    }
                }
                else
                {
                    Emp_Username.Text = "No data found.";
                }
                rd.Close();
            }
            else
            {
                Response.Redirect("../login.aspx");
            }
        }

        protected void btnlogout_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["ClockInTime"] != null)
            {
                DateTime clockInTime = (DateTime)Session["ClockInTime"];
                DateTime clockOutTime = DateTime.Now;
                double countHour = (clockOutTime - clockInTime).TotalHours;

                startcon();

                // Update ClockOutTime
                cmd = new SqlCommand("UPDATE Working_Hour_tbl SET ClockOutTime = @ClockOutTime, CountHour = @CountHour WHERE W_EmpId = @EmpId AND ClockOutTime IS NULL", con);
                cmd.Parameters.AddWithValue("@ClockOutTime", clockOutTime);
                cmd.Parameters.AddWithValue("@CountHour", countHour);
                cmd.Parameters.AddWithValue("@EmpId", ViewState["empID"]);
                cmd.ExecuteNonQuery();

                Session.Remove("ClockInTime");
            }

            Session.Abandon();
            Response.Redirect("../login.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("profile.aspx");
        }
    }
}
