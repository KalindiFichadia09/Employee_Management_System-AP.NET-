<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true" CodeBehind="manage_designations.aspx.cs" Inherits="project_sem_6_.admin.manage_designations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder3">
    <div class="main-container">
        <div class="pd-ltr-20 xs-pd-20-10">
            <div class="min-height-200px">
                <div class="page-header">
                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div class="title">
                                <h4>Manage Designations</h4>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Button ID="btn_addc" runat="server" CssClass="btn btn-info" Text="Add Designation" OnClick="btn_addc_Click" />
                <br />
                <br />
                <div class="pd-20 card-box mb-30">
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" OnRowCommand="GridView1_RowCommand" class="table table-bordered text-center" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation Name">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Designation_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Base Salary">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("BaseSalary") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="cmd_edit" class="btn btn-success">Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="cmd_remove" class="btn btn-danger">Remove</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="False">
                <ContentTemplate>
                    <div class="pd-20 card-box mb-30">
                        <div class="col-md-12 col-sm-12">
                            <div class="title">
                                <h5>
                                    <asp:Label ID="lbl_heading" runat="server" Text="Add"></asp:Label>
                                </h5>
                                <hr />
                            </div>
                        </div>
                        <form>
                            <div class="form-group row">
                                <asp:Label ID="Label4" runat="server" Text="Designation Name" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="Designation_Name" runat="server" class="form-control" placeholder="Enter Designation Name"></asp:TextBox>
                                </div>
                            </div>
                            <!-- Input fields for additional columns -->
                            <div class="form-group row">
                                <asp:Label ID="Label5" runat="server" Text="Base Salary" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="BaseSalary" runat="server" class="form-control" placeholder="Enter Base Salary"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label ID="Label6" runat="server" Text="HRA" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="HRA" runat="server" class="form-control" placeholder="Enter HRA"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label ID="Label7" runat="server" Text="DA" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="DA" runat="server" class="form-control" placeholder="Enter DA"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label ID="Label8" runat="server" Text="TA" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="TA" runat="server" class="form-control" placeholder="Enter TA"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label ID="Label9" runat="server" Text="Other Allowances" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="OtherAllowances" runat="server" class="form-control" placeholder="Enter Other Allowances"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label ID="Label10" runat="server" Text="Deductions" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="Deductions" runat="server" class="form-control" placeholder="Enter Deductions"></asp:TextBox>
                                </div>
                            </div>
                            <div class="btn-set">
                                <asp:Button ID="Add_Designation" OnClick="Add_Designation_Click" runat="server" Text="Add" class="btn btn-primary" />
                            </div>
                        </form>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
        </div>
    </div>
</asp:Content>
