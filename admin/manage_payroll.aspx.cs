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
        int month, year, i;

        // Initialize the connection once
        void startcon()
        {
            cs = new Employee();
            con = cs.getcon();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            startcon(); // Initialize connection
        }

        protected void give_payroll_Click(object sender, EventArgs e)
        {
            string empId = P_Emp_Id.SelectedValue.ToString();
            month = int.Parse(P_Month.SelectedValue);
            year = int.Parse(P_Year.Text);

            decimal totalWorkingHours = GetTotalWorkingHours(empId, month, year);
            decimal payAmount = CalculatePayAmount(totalWorkingHours);

            if (totalWorkingHours > 0)
            {
                InsertPayrollData(empId, month, year, totalWorkingHours, payAmount);
                UpdateWorkingHourStatus(empId, month, year); // Mark records as inactive
            }
            else
            {
                // Handle the case where there are no working hours to process payroll
                Console.WriteLine("No working hours found for the given employee and time period.");
            }
        }

        private decimal GetTotalWorkingHours(string empId, int month, int year)
        {
            startcon();
            decimal totalWorkingHours = 0;

            // Ensure the connection is open before executing the command
            cmd = new SqlCommand("SELECT SUM(CountHour) AS TotalWorkingHours " +
                                 "FROM Working_Hour_tbl " +
                                 "WHERE W_EmpID = @EmpId AND MONTH(ClockInTime) = @Month AND YEAR(ClockInTime) = @Year AND Status = 'Active'", con);
            cmd.Parameters.AddWithValue("@EmpId", empId);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);

            //con.Open(); // Open connection
            object obj = cmd.ExecuteScalar(); // Execute the query and get the result
            con.Close(); // Close the connection

            if (obj != DBNull.Value && obj != null)
            {
                totalWorkingHours = Convert.ToDecimal(obj);
            }

            return totalWorkingHours;
        }

        private decimal CalculatePayAmount(decimal totalWorkingHours)
        {
            decimal hourlyRate = 1000.0m; // Define hourly rate
            return totalWorkingHours * hourlyRate; // Calculate pay amount
        }

        private void InsertPayrollData(string empId, int month, int year, decimal totalWorkingHours, decimal payAmount)
        {
            startcon();
            if (totalWorkingHours <= 0)
            {
                Console.WriteLine("No working hours to insert for payroll.");
                return; // Prevent inserting if there are no working hours
            }

            string insertQuery = "INSERT INTO Payroll_tbl(P_Emp_Id, P_Month, P_Year, P_TotalWorkingHours, P_PayAmount) " +
                                 "VALUES(@EmpId, @Month, @Year, @TotalWorkingHours, @PayAmount)";

            // Use the existing connection
            using (cmd = new SqlCommand(insertQuery, con))
            {
                cmd.Parameters.AddWithValue("@EmpId", empId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@TotalWorkingHours", totalWorkingHours);
                cmd.Parameters.AddWithValue("@PayAmount", payAmount);

                //con.Open(); // Open the connection before executing the insert command
                cmd.ExecuteNonQuery(); // Execute the insert command
                con.Close(); // Close the connection after executing the command
            }
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

                //con.Open(); // Open connection before executing the update command
                cmd.ExecuteNonQuery(); // Execute the update command
                con.Close(); // Close the connection
            }
        }
    }
}
