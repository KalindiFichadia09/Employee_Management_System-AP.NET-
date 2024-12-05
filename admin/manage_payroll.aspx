<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true" CodeBehind="manage_payroll.aspx.cs" Inherits="project_sem_6_.admin.manage_payroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder3">
    <div class="main-container">
        <div class="xs-pd-20-10 pd-ltr-20">
            <div class="page-header">
                <div class="row">
                    <div class="col-md-6 col-sm-12">
                        <div class="title">
                            <h4>Manage Payroll</h4>
                        </div>
                        <nav aria-label="breadcrumb" role="navigation">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Manage Payroll</li>
                            </ol>
                        </nav>
                    </div>
                </div>
            </div>
            <asp:Button ID="give" runat="server" CssClass="btn btn-info" Text="Give Payroll to Employee" OnClick="give_Click"/>
                <br />
                <br />
            <div class="pd-20 card-box mb-30">
                <div class="table-responsive">
                    <asp:GridView ID="GridView1" runat="server" OnRowCommand="GridView1_RowCommand" class="table table-bordered text-center" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr no">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee Id">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("P_Emp_Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Month">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("P_Month") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("P_Year") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net Salary">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("P_NetSalary") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="cmd_action" class="btn btn-info">See</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
            <ContentTemplate>
                <div class="pd-20 card-box mb-30">
                    <div class="col-md-12 col-sm-12">
                        <div class="title">
                            <h5>
                                <asp:Label ID="lbl_heading" runat="server" Text="Give Payroll"></asp:Label>
                            </h5>
                            <hr />
                        </div>
                    </div>
                    <br />
                    <form>
                        <div class="form-group row">
                            <asp:Label ID="Label6" runat="server" Text="Employee Id" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:DropDownList ID="P_Emp_Id" class="custom-select form-control" runat="server" DataSourceID="emp_name_with_id" DataTextField="Emp_Full_Name" DataValueField="Emp_Employee_Id"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="emp_name_with_id" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Employee_Management_db.mdf;Integrated Security=True" ProviderName="System.Data.SqlClient" SelectCommand="SELECT [Emp_Full_Name], [Emp_Employee_Id] FROM [Employee_tbl]"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="Label7" runat="server" Text="Month" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:DropDownList ID="P_Month" runat="server" class="custom-select form-control">
                                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="Label8" runat="server" Text="Year :" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:TextBox ID="P_Year" runat="server" placeholder="Enter Year" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="btn-set">
                            <asp:Button ID="give_payroll" runat="server" Text="Give Payroll" OnClick="give_payroll_Click" class="btn btn-primary" />
                        </div>
                    </form>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false">
            <ContentTemplate>
                <div class="pd-20 card-box mb-30">
                    <div class="title">
                        <h5>
                            <h5>Payroll Information</h5>
                            <hr />
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                Employee Id :
                                    <asp:Label ID="Emp_Id" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                Month : 
                                    <asp:Label ID="P_Month_S" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                Year :
                                    <asp:Label ID="P_Year_S" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                Total Working Hours :
                                    <asp:Label ID="P_TotalWorkingHours" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                Base Salary :
                                    <asp:Label ID="P_BaseSalary" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                House Rent Allowance : 
                                    <asp:Label ID="P_HRA" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                Dearness Allowance:
                                    <asp:Label ID="P_DA" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                Travell Allowance : 
                                    <asp:Label ID="P_TA" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                Other Allowances :
                                    <asp:Label ID="P_OtherAllowances" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                Deductions : 
                                    <asp:Label ID="P_Deductions" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                Total Payable :
                                    <asp:Label ID="P_TotalPayable" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                Net Salary : 
                                    <asp:Label ID="P_NetSalary" runat="server" class="h5 text-black-50" Text="Label"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <%--</div>
       </div>--%>
        <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>

