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
                <asp:Button ID="btn_addd" runat="server" CssClass="btn btn-info" Text="Add Designations" OnClick="btn_addd_Click" />
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
                                <asp:TemplateField HeaderText="Hourly Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("Hourly_Rate") %>'></asp:Label>
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
                            <EmptyDataTemplate>
                                <tr>
                                    <td colspan="10" style="text-align: center;"><h3>No data found</h3></td>
                                </tr>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="False">
                <ContentTemplate>
                    <div class="pd-20 card-box mb-30">
                        <div class="col-md-12 col-sm-12">
                            <div class="title">
                                <h5>
                                    <asp:Label ID="lbl_heading" runat="server" Text="Add"></asp:Label></h5>
                                <hr />
                            </div>
                        </div>
                        <br />
                        <form>
                            <!-- Designation Name -->
                            <div class="form-group row">
                                <asp:Label ID="Label15" runat="server" Text="Designation Name" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="Designation_Name" runat="server" class="form-control" placeholder="Enter Designation Name"></asp:TextBox>
                                </div>
                            </div>
                            <%-- Hourly Rate --%>
                            <div class="form-group row">
                                <asp:Label ID="Label16" runat="server" Text="Hourly Rate" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                                <div class="col-sm-12 col-md-10">
                                    <asp:TextBox ID="Rate" runat="server" class="form-control" placeholder="Enter Hourly Rate"></asp:TextBox>
                                </div>
                            </div>
                            <div class="btn-set">
                                <asp:Button ID="Add_Designation" OnClick="Add_Designation_Click" runat="server" Text="Add" class="btn btn-primary" />
                            </div>
                        </form>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br>
            <br>
            <br>
            <br>
            <br>
            <br>
        </div>
    </div>
</asp:Content>

