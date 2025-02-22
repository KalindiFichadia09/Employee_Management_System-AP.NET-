﻿<%@ Page Title="" Language="C#" MasterPageFile="~/user/user.Master" AutoEventWireup="true" CodeBehind="leave.aspx.cs" Inherits="project_sem_6_.user.leave" %>

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
                                <h4>Leave</h4>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Button ID="btn_show_form" runat="server" Text="Apply for Leave" OnClick="btn_show_form_Click" CssClass="btn btn-info" />
                <br /><br />

                <div class="pd-20 card-box mb-30">
                    <div class="leave-summary">
                        <table class="table table-bordered text-center" runat="server" id="leaveSummaryTable">
                            <thead>
                                <tr>
                                    <th>Leave Type</th>
                                    <th>Total Leaves</th>
                                    <th>Used Leaves</th>
                                    <th>Remaining Leaves</th>
                                </tr>
                            </thead>
                            <tbody>
                                <%-- Rows will be dynamically added from the code-behind --%>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="pd-20 card-box mb-30">
                    <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" class="table table-bordered text-center" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr no">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Type">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("L_Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Category">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("L_Category") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Start Date">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("L_Start_Date") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave End Date">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("L_End_Date") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("L_Remarks") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Document">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" Height="150" Width="150" ImageUrl='<%# Eval("L_Document") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Label ID="Label9" runat="server" Text='<%# Eval("L_Action") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="False" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="pd-20 card-box mb-30">
                        <div class="form-group row">
                            <asp:Label ID="Label1" runat="server" Text="Leave Type :" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:DropDownList ID="L_Type" runat="server" class="custom-select col-12">
                                    <asp:ListItem>--Select leave type--</asp:ListItem>
                                    <asp:ListItem Value="CL">CL (Casual Leave)</asp:ListItem>
                                    <asp:ListItem Value="PL">PL (Privilege Leave)</asp:ListItem>
                                    <asp:ListItem Value="SL">SL (Sick Leave)</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="Label4" runat="server" Text="Leave Category :" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:DropDownList ID="L_Category" runat="server" class="custom-select col-12">
                                    <asp:ListItem>--Select leave category--</asp:ListItem>
                                    <asp:ListItem>Full day</asp:ListItem>
                                    <asp:ListItem>Half day</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="Label5" runat="server" Text="Leave Start Date :" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:TextBox ID="L_Start_Date" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="Label6" runat="server" Text="Leave End Date :" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:TextBox ID="L_End_Date" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="Label7" runat="server" Text="Remarks :" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:TextBox ID="L_Remarks" runat="server" TextMode="SingleLine" placeholder="Enter Remarks" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="Label8" runat="server" Text="Attach Document :" class="col-sm-12 col-md-2 col-form-label"></asp:Label>
                            <div class="col-sm-12 col-md-10">
                                <asp:FileUpload ID="L_Document" runat="server" class="form-control" />
                            </div>
                        </div>
                        <div class="btn-set">
                            <asp:Button ID="btn_apply" runat="server" Text="Apply" OnClick="btn_apply_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btn_apply" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>