using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace project_sem_6_.admin
{
    public partial class manage_payroll : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        Employee cs;
        int month, year;

        void startcon()
        {
            cs = new Employee();
            con = cs.getcon();
        }
        void fillgrid()
        {
            GridView1.DataSource = cs.Payroll_filldata();
            GridView1.DataBind();
        }
        void filltext()
        {
            ds = new DataSet();
            ds = cs.Payroll_select_admin(Convert.ToInt16(ViewState["id"]));
            Emp_Id.Text = (ds.Tables[0].Rows[0][1]).ToString();
            P_Month_S.Text = (ds.Tables[0].Rows[0][2]).ToString();
            P_Year_S.Text = (ds.Tables[0].Rows[0][3]).ToString();
            P_TotalWorkingHours.Text = (ds.Tables[0].Rows[0][4]).ToString();
            P_BaseSalary.Text = (ds.Tables[0].Rows[0][5]).ToString();
            P_HRA.Text = (ds.Tables[0].Rows[0][6]).ToString();
            P_DA.Text = (ds.Tables[0].Rows[0][7]).ToString();
            P_TA.Text = (ds.Tables[0].Rows[0][8]).ToString();
            P_OtherAllowances.Text = (ds.Tables[0].Rows[0][9]).ToString();
            P_Deductions.Text = (ds.Tables[0].Rows[0][10]).ToString();
            P_TotalPayable.Text = (ds.Tables[0].Rows[0][11]).ToString();
            P_NetSalary.Text = (ds.Tables[0].Rows[0][12]).ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            startcon();
            fillgrid();
            //Response.Redirect(Request.RawUrl);
        }

        protected void give_payroll_Click(object sender, EventArgs e)
        {
            string empId = P_Emp_Id.SelectedValue.ToString();
            month = int.Parse(P_Month.SelectedValue);
            year = int.Parse(P_Year.Text);

            decimal totalWorkingHours = GetTotalWorkingHours(empId, month, year);
            decimal hourlyRate = GetHourlyRate(empId);
            decimal baseSalary = totalWorkingHours * hourlyRate;

            if (totalWorkingHours > 0)
            {
                // Calculate payroll components
                decimal hra = baseSalary * 0.2m; // Example: 20% of Base Salary
                decimal da = baseSalary * 0.1m; // Example: 10% of Base Salary
                decimal ta = baseSalary * 0.05m; // Example: Fixed TA
                decimal otherAllowances = 1500; // Example: Fixed Other Allowances
                decimal deductions = baseSalary * 0.05m; // Example: 5% of Base Salary
                decimal totalPayable = baseSalary + hra + da + ta + otherAllowances;
                decimal netSalary = totalPayable - deductions;

                // Insert into Payroll_tbl
                InsertPayrollData(empId, month, year, totalWorkingHours, baseSalary, hra, da, ta, otherAllowances, deductions, totalPayable, netSalary);

                // Update Working Hour Status
                UpdateWorkingHourStatus(empId, month, year);
            }
            else
            {
                Console.WriteLine("No working hours found for the given employee and time period.");
            }
            Response.Redirect(Request.RawUrl);
        }

        private decimal GetTotalWorkingHours(string empId, int month, int year)
        {
            startcon();
            decimal totalWorkingHours = 0;

            cmd = new SqlCommand("SELECT SUM(CountHour) AS TotalWorkingHours " +
                                 "FROM Working_Hour_tbl " +
                                 "WHERE W_EmpID = @EmpId AND MONTH(ClockInTime) = @Month AND YEAR(ClockInTime) = @Year AND Status = 'Active'", con);
            cmd.Parameters.AddWithValue("@EmpId", empId);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);

            object obj = cmd.ExecuteScalar();
            con.Close();

            if (obj != DBNull.Value && obj != null)
            {
                totalWorkingHours = Convert.ToDecimal(obj);
            }

            return totalWorkingHours;
        }

        private decimal GetHourlyRate(string empId)
        {
            startcon();
            decimal hourlyRate = 0;

            cmd = new SqlCommand("SELECT D.Hourly_Rate " +
                                 "FROM Employee_tbl E " +
                                 "INNER JOIN Designation_tbl D ON E.Emp_Designation = D.Designation_Name " +
                                 "WHERE E.Emp_Employee_Id = @EmpId", con);
            cmd.Parameters.AddWithValue("@EmpId", empId);

            object hourlyRateObj = cmd.ExecuteScalar();
            con.Close();

            if (hourlyRateObj != DBNull.Value)
            {
                hourlyRate = Convert.ToDecimal(hourlyRateObj);
            }

            return hourlyRate;
        }

        private void InsertPayrollData(
            string empId, int month, int year, decimal totalWorkingHours,
            decimal baseSalary, decimal hra, decimal da, decimal ta,
            decimal otherAllowances, decimal deductions, decimal totalPayable, decimal netSalary)
        {
            startcon();

            string insertQuery = @"INSERT INTO Payroll_tbl
                (P_Emp_Id, P_Month, P_Year, P_TotalWorkingHours, P_BaseSalary, P_HRA, P_DA, P_TA, 
                P_OtherAllowances, P_Deductions, P_TotalPayable, P_NetSalary) 
                VALUES (@EmpId, @Month, @Year, @TotalWorkingHours, @BaseSalary, @HRA, @DA, @TA, 
                @OtherAllowances, @Deductions, @TotalPayable, @NetSalary)";

            using (cmd = new SqlCommand(insertQuery, con))
            {
                cmd.Parameters.AddWithValue("@EmpId", empId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@TotalWorkingHours", totalWorkingHours);
                cmd.Parameters.AddWithValue("@BaseSalary", baseSalary);
                cmd.Parameters.AddWithValue("@HRA", hra);
                cmd.Parameters.AddWithValue("@DA", da);
                cmd.Parameters.AddWithValue("@TA", ta);
                cmd.Parameters.AddWithValue("@OtherAllowances", otherAllowances);
                cmd.Parameters.AddWithValue("@Deductions", deductions);
                cmd.Parameters.AddWithValue("@TotalPayable", totalPayable);
                cmd.Parameters.AddWithValue("@NetSalary", netSalary);

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt16(e.CommandArgument);
            ViewState["id"] = id;
            if (e.CommandName == "cmd_action")
            {
                UpdatePanel2.Visible = true;
                filltext();
            }
        }

        protected void give_Click(object sender, EventArgs e)
        {
            UpdatePanel1.Visible = true;
        }

        private void UpdateWorkingHourStatus(string empId, int month, int year)
        {
            startcon();

            string updateQuery = "UPDATE Working_Hour_tbl SET Status = 'Inactive' " +
                                 "WHERE W_EmpID = @EmpId AND MONTH(ClockInTime) = @Month AND YEAR(ClockInTime) = @Year AND Status = 'Active'";

            using (cmd = new SqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@EmpId", empId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
