<%@ Page Title="" Language="C#" MasterPageFile="~/Page/Site.Master" AutoEventWireup="true" CodeBehind="ItSupportMng.aspx.cs" Inherits="FavLink.Page.ItSupportMng" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
    <script src="../Scripts/bootstrap-dialog.min.js" type="text/javascript"></script>
    <script src="../Scripts/BootstrapValidator.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="row">
        <form id="Form1" runat="server">
        <div class="col-lg-6">
            <section class="panel">
        <header class="panel-heading">
            <h3>IT人员管理</h3>
        </header>
        <div class="panel-body" style="overflow: auto">
            <div class="col-lg-9 form-group">
                <asp:Label ID="Label1" Text="name:" runat="server" />
                <asp:TextBox ID="txtItName" runat="server"></asp:TextBox>
            </div>
            <div class="col-lg-9 form-group">
                <asp:Label ID="Label2" Text="Phone Number:" runat="server" />
                <asp:TextBox ID="txtItPhone" runat="server"></asp:TextBox>
                <asp:Button ID="btnAddItSupport" runat="server" Text="Add New" OnClick="btnAddItSupport_OnClick" />
            </div>

            <asp:GridView ID="gvItSupports" runat="server" Height="138px" Width="450px"
                AutoGenerateColumns="False"
                OnRowCancelingEdit="GVItSupports_RowCancelingEdit"
                OnRowDeleting="GVItSupports_RowDeleting"
                OnRowEditing="GVItSupports_RowEditing"
                OnRowUpdating="GVItSupports_RowUpdating">
                <Columns>
                    <%--<asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ReadOnly="True" />--%>
                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
                    <asp:BoundField DataField="phone_number" HeaderText="Phone Number" SortExpression="phone_number" />
                    <%--<asp:CommandField HeaderText="Select" ShowSelectButton="True" ButtonType="Button" />--%>
                    <asp:CommandField HeaderText="Edit" ShowEditButton="True" ButtonType="Button" />
                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ButtonType="Button" />
                </Columns>
                <EmptyDataRowStyle BackColor="Red" />
                <HeaderStyle BackColor="#0000CC" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#864" HorizontalAlign="Center" ForeColor="White" />
            </asp:GridView>
        </div>
    </section>
        </div>

        <div class="col-lg-6">
            <section class="panel">
            <header class="panel-heading">
                <h3>Hotline</h3>
            </header>
        <div class="panel-body" style="overflow: auto">
            <div class="col-lg-9 form-group">
                <asp:Label ID="Label3" Text="name:" runat="server" />
                <asp:TextBox ID="txtHotlineName" runat="server"></asp:TextBox>
            </div>
            <div class="col-lg-9 form-group">
                <asp:Label ID="Label4" Text="Phone Number:" runat="server" />
                <asp:TextBox ID="txtHotlinePhone" runat="server"></asp:TextBox>
            </div>
            <div class="col-lg-9 form-group">
                <asp:Label ID="Label5" Text="Category:" runat="server" />
                <asp:DropDownList ID="ddlHotlineCategory" runat="server"></asp:DropDownList>
                <asp:Button ID="Button2" runat="server" Text="Add New Hotline" OnClick="btnAddHotline_OnClick" />
            </div>

            <asp:GridView ID="gvHotline" runat="server" Height="138px" Width="450px"
                AutoGenerateColumns="False"
                OnRowCancelingEdit="GVHotline_RowCancelingEdit"
                OnRowDeleting="GVHotline_RowDeleting"
                OnRowEditing="GVHotline_RowEditing"
                OnRowUpdating="GVHotline_RowUpdating">
                <Columns>
                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
                    <asp:BoundField DataField="phone_number" HeaderText="Phone Number" SortExpression="phone_number" />
                    <asp:BoundField DataField="category" HeaderText="Category" SortExpression="category" ReadOnly/>
                    <asp:CommandField HeaderText="Edit" ShowEditButton="True" ButtonType="Button" />
                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ButtonType="Button" />
                </Columns>
                <EmptyDataRowStyle BackColor="Red" />
                <HeaderStyle BackColor="#0000CC" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#864" HorizontalAlign="Center" ForeColor="White" />
            </asp:GridView>
        </div>
            </section>
        </div>
        </form>
    </div>
</asp:Content>
