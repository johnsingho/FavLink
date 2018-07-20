<%@ Page Title="" Language="C#" MasterPageFile="~/Page/Site.Master" AutoEventWireup="true" CodeBehind="UserMng.aspx.cs" Inherits="FavLink.Page.UserMng" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
    <script src="../Scripts/bootstrap-dialog.min.js" type="text/javascript"></script>
    <script src="../Scripts/BootstrapValidator.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="row">
        <div class="col-lg-8">
            <section class="panel">
                    <header class="panel-heading">
                        <h3>用户管理 User Management</h3>
                    </header>
                    <form id="Form1" runat="server">
                    <div class="panel-body" style="overflow: auto">
                        <div class="col-sm-9 form-group">
                            <asp:Label ID="Label1" Text="AD Account" runat="server" />
                            <asp:TextBox ID="txtNewADAccount" runat="server"></asp:TextBox>
                            <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAddUser_OnClick" />
                        </div>
                        <asp:GridView ID="gvUsers" runat="server" Height="138px" Width="450px"
                            AutoGenerateColumns="False"
                            OnRowCancelingEdit="GVUsers_RowCancelingEdit"
                            OnRowDeleting="GVUsers_RowDeleting"
                            OnRowEditing="GVUsers_RowEditing"
                            OnRowUpdating="GVUsers_RowUpdating">
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ReadOnly="True" />
                                <asp:BoundField DataField="ADAccount" HeaderText="ADAccount" SortExpression="ADAccount" ReadOnly="True" />
                                <asp:BoundField DataField="FullName" HeaderText="FullName" SortExpression="FullName" ReadOnly="True" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ReadOnly="True" />
                                <%--<asp:BoundField DataField="LastLogon" HeaderText="LastLogon" SortExpression="LastLogon" />--%>
                                <asp:BoundField DataField="IsValid" HeaderText="IsValid" SortExpression="IsValid" />
                                <%--<asp:BoundField DataField="IsAdmin" HeaderText="IsAdmin" SortExpression="IsAdmin" />--%>
                                <%--<asp:CommandField HeaderText="Select" ShowSelectButton="True" ButtonType="Button" />--%>
                                <asp:CommandField HeaderText="Edit" ShowEditButton="True" ButtonType="Button" />
                                <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ButtonType="Button" />
                            </Columns>
                            <EmptyDataRowStyle BackColor="Red" />
                            <HeaderStyle BackColor="#0000CC" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#864" HorizontalAlign="Center" ForeColor="White" />
                        </asp:GridView>
                    </div>
                    </form>
            </section>
        </div>
    </div>

</asp:Content>
