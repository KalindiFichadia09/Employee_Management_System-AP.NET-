using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using Table = iText.Layout.Element.Table;
using iText.Kernel.Pdf.Canvas.Draw;

namespace project_sem_6_.user
{
    public partial class Payroll : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        Employee cs;
        string flnm, mail;
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
            da = new SqlDataAdapter("select * from Payroll_tbl where Id='" + id + "' ", con);
            ds = new DataSet();
            da.Fill(ds);
            ViewState["id"] = (ds.Tables[0].Rows[0][0]).ToString();
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
            if (!IsPostBack)
            {
                startcon();
                if (Session["username"] != null)
                {
                    mail = Session["username"].ToString();
                    using (SqlCommand cmd = new SqlCommand("SELECT Emp_Employee_Id FROM Employee_tbl WHERE Emp_Company_Email=@Email", con))
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

        protected void btn_download_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected payroll ID from ViewState
                int payrollId = Convert.ToInt32(ViewState["id"]);
                if (payrollId <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select a valid payroll record first!');", true);
                    return;
                }

                // Create a database connection and fetch payroll data for the selected record
                startcon();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Payroll_tbl WHERE Id=@Id", con);
                da.SelectCommand.Parameters.AddWithValue("@Id", payrollId);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];

                    // Construct the file name for the PDF
                    string fileName = $"SalarySlip_{row["P_Emp_Id"]}_{row["P_Month"]}_{row["P_Year"]}.pdf";
                    string folderPath = Server.MapPath("~/SalarySlips/");

                    // Ensure the folder exists (create if not)
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Full path of the PDF file
                    string filePath = Path.Combine(folderPath, fileName);

                    // Create and write to the PDF document using iTextSharp
                    using (PdfWriter writer = new PdfWriter(filePath))
                    {
                        using (PdfDocument pdfDoc = new PdfDocument(writer))
                        {
                            Document document = new Document(pdfDoc);

                            // Add title and salary slip heading
                            document.Add(new Paragraph("DeskApp")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                                .SetFontSize(18).SetTextAlignment(TextAlignment.CENTER));

                            document.Add(new Paragraph("Salary Slip")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                                .SetFontSize(14).SetTextAlignment(TextAlignment.CENTER));

                            // Add a separator line
                            document.Add(new LineSeparator(new SolidLine()));

                            // Add payroll details
                            document.Add(new Paragraph($"Employee ID: {row["P_Emp_Id"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"Month: {row["P_Month"]} {row["P_Year"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"Total Working Hours: {row["P_TotalWorkingHours"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"Base Salary: {row["P_BaseSalary"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"HRA: {row["P_HRA"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"DA: {row["P_DA"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"TA: {row["P_TA"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"Other Allowances: {row["P_OtherAllowances"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"Deductions: {row["P_Deductions"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"Total Payable: {row["P_TotalPayable"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));
                            document.Add(new Paragraph($"Net Salary: {row["P_NetSalary"]}")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)));

                            // Close the document
                            document.Close();
                        }
                    }

                    // After the PDF is generated, send it to the user for download
                    if (File.Exists(filePath))
                    {
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
                        Response.TransmitFile(filePath);
                        Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to generate the PDF file.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No data found for the selected payroll!');", true);
                }
            }
            catch (Exception ex)
            {
                // Simple error handling: show the message but avoid throwing exceptions that break the flow
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }




        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmd_action")
            {
                id = Convert.ToInt16(e.CommandArgument);
                //ViewState["id"] = id;
                if (e.CommandName == "cmd_action")
                {
                    UpdatePanel1.Visible = true;
                    filltext();
                }
            }
        }
    }
}