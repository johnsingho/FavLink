<%@ Page Title="" Language="C#" MasterPageFile="~/Page/Site.Master" AutoEventWireup="true" CodeBehind="ItShiftMng.aspx.cs" Inherits="FavLink.Page.ItShiftMng" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
    <script src="../Scripts/bootstrap-dialog.min.js" type="text/javascript"></script>
    <script src="../Scripts/BootstrapValidator.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
<div class="col-lg-11">
        <header class="panel-heading">
            <h3>IT人员排班 IT shift Arrangement</h3>
        </header>
        <form id="Form1" runat="server">
        <div style="overflow: auto">
            <div class="col-lg-9 form-group">
                <asp:Label ID="Label1" Text="IT support name:" runat="server" />
                <asp:DropDownList ID="ddlItSupportUsers" runat="server"></asp:DropDownList>
            </div>
            <div class="col-lg-9 form-group">
                <asp:Label ID="Label2" Text="Project:" CssClass="col-lg-1" runat="server" />
                <asp:TextBox ID="txtItSupportProject" CssClass="col-lg-8" runat="server"></asp:TextBox>
            </div>
            <div class="col-lg-9 form-group">
                <div class="col-lg-2">                
                <asp:Label ID="Label3" Text="Shift:" runat="server" />
                <asp:DropDownList ID="ddlItSupportShifts" runat="server"></asp:DropDownList>
                </div>
                <div class="col-lg-3">
                <asp:Label ID="Label4" Text="Month:" runat="server" />
                <%--<asp:TextBox ID="txtShiftMonth" runat="server"></asp:TextBox>--%>
                <asp:DropDownList ID="ddlShiftMonth" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="col-lg-9 form-group">
                <div class="col-lg-5">
                <asp:Button ID="btnAddItShift" runat="server" Text="Add Shift Arrange" OnClick="btnAddItShift_OnClick" />
                <asp:Button ID="btnUpdateItShift" runat="server" Text="Update Shift Arrange" OnClick="btnUpdateItShift_OnClick" />
                </div>
                <div class="col-lg-5">
                    <asp:Label Text="根据月份过滤显示 Filter by month:" runat="server" />
                    <asp:DropDownList ID="ddlFilterMonth" 
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlFilterMonth_SelectedChanged"
                        runat="server" >
                    </asp:DropDownList>
                </div>
            </div>

            <asp:GridView ID="gvItShift" runat="server" Height="200px" Width="650px"
                AutoGenerateColumns="False"
                OnSelectedIndexChanged="GVItShift_SelectedIndexChanged"
                OnRowCancelingEdit="GVItShift_RowCancelingEdit"
                OnRowDeleting="GVItShift_RowDeleting"
                OnRowEditing="GVItShift_RowEditing">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ReadOnly="True" />
                    <asp:BoundField DataField="ITSupportName" HeaderText="ITSupportName" SortExpression="ITSupportName" />
                    <asp:BoundField DataField="project" HeaderText="project" SortExpression="project" />
                    <asp:BoundField DataField="shiftName" HeaderText="shift" SortExpression="shiftName" />
                    <asp:BoundField DataField="month" HeaderText="month" SortExpression="month" />
                    <asp:CommandField HeaderText="Select" ShowSelectButton="True" ButtonType="Button" />
                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ButtonType="Button" />
                </Columns>
                <EmptyDataRowStyle BackColor="Red" />
                <HeaderStyle BackColor="#0000CC" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#864" HorizontalAlign="Center" ForeColor="White" />
            </asp:GridView>
        </div>
        </form>
</div>
</asp:Content>
