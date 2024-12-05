using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace project_sem_6_.admin
{ 
    public partial class manage_designations : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        Employee cs;

        void startcon()
        {
            con = new SqlConnection();
            cs = new Employee();
            con = cs.getcon();
        }
        void clear()
        {
            Designation_Name.Text = string.Empty;
            Rate.Text = string.Empty;
        }

        void fillgrid()
        {
            GridView1.DataSource = cs.Designation_Filldata();
            GridView1.DataBind();
        }

        void filltext()
        {
            ds = new DataSet();
            ds = cs.Designation_Select(Convert.ToInt16(ViewState["id"]));
            Designation_Name.Text = (ds.Tables[0].Rows[0]["Designation_Name"]).ToString();
            Rate.Text = (ds.Tables[0].Rows[0]["Hourly_Rate"]).ToString();
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

            if (e.CommandName == "cmd_edit")
            {
                UpdatePanel1.Visible = true;
                //Label10.Visible = true;
                //HRA.Visible = true;
                //Label11.Visible = true;
                //DA1.Visible = true;
                //Label12.Visible = true;
                //TA.Visible = true;
                Add_Designation.Text = "Update";
                lbl_heading.Text = "Update";
                filltext();
            }
            if (e.CommandName == "cmd_remove")
            {
                
                cs.Designation_Remove(Convert.ToInt16(ViewState["id"]));
                fillgrid();
            }
        }

        protected void Add_Designation_Click(object sender, EventArgs e)
        {
            startcon();

            if (Add_Designation.Text == "Add")
            {
                cs.Designation_Add(Designation_Name.Text, Convert.ToDecimal(Rate.Text));
                fillgrid();
                clear();
            }
            else
            {
                cs.Designation_Edit(Convert.ToInt16(ViewState["id"]), Designation_Name.Text, Convert.ToDecimal(Rate.Text));
                fillgrid();
                clear();
            }

            Response.Redirect(Request.RawUrl);
        }

        protected void btn_addd_Click(object sender, EventArgs e)
        {
            UpdatePanel1.Visible = true;
        }
    }
}