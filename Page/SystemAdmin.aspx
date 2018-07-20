<%@ Page Title="" Language="C#" MasterPageFile="~/Page/Site.Master" AutoEventWireup="true"
    CodeBehind="SystemAdmin.aspx.cs" Inherits="FavLink.Page.SystemAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
    <script src="../Scripts/bootstrap-dialog.min.js" type="text/javascript"></script>
    <script src="../Scripts/BootstrapValidator.min.js" type="text/javascript"></script>
    <style>
        .page-heading h3
        {
            color: #49586e;
            font-size: 25px;
            font-weight: normal;
            margin: 10px 0;
        }
    </style>
    <script type="text/javascript">
        function ShowSelectIcon(str) {
            var obj = $('#selLinkIcon');
            obj.removeAttr('class');
            obj.attr('class', 'fa ' + str);
        }
        function DoSelectIcon(v) {
            var obj = $(v).find('i');
            var icon = obj.attr('icon');
            $("#<%=hidSelectedIcon.ClientID%>").val(icon);
            ShowSelectIcon(icon);
        }
        
        function ShowSelecctionIcon(str) {
            var obj = $('#selLinkColor');
            obj.removeAttr('class');
            obj.attr('class', 'label ' + str);
            obj.text(str);
        }
        function DoSelectColor(v) {
            var obj = $(v).find('span');
            var color = obj.text();
            $("#<%=hidSelectedColor.ClientID%>").val(color);
            ShowSelecctionIcon(color);
        }

        function DeleteLink(v) {
            var tr = $(v).parents("tr");
            var linkID = $(tr[0]).attr("editkey");
            var para = {'linkid':linkID};
            AjaxSend("SystemAdmin.aspx/DeleteLink", JSON.stringify(para), function (result) {
                if (result.d) {
                    var data = eval("(" + result.d + ")");
                    if (1 == data.ret) {
                        location.assign(location.href);
                    } else {
                        BootstrapDialog.alert(data.errMsg);
                    }
                }
            });
        }

        $(function () {
            $('#tabLinks tbody').on('click', 'tr', function () {
                var row = $(this);
                var tds = $(row).find("td");
                var lid = $(tds[0]).text();
                var lname = $(tds[1]).text();
                var lurl = $(tds[2]).text();
                var lcate = $(tds[3]).text();
                var vicon = $($(tds[4]).find("i"));
                var licon = $.trim($(vicon[0]).attr("class").replace("fa ", ""));
                var lcolor = $($(tds[5]).find("span")).text();

                $("#<%=hidSelectedLink.ClientID%>").val(lid);
                $("#<%=txtLinkName.ClientID%>").val(lname);
                $("#<%=txtLinkUrl.ClientID%>").val(lurl);
                $('#<%=ddlLinkCate.ClientID%> option').each(function () {
                    if ($(this).text().toLowerCase() == lcate.toLowerCase()) {
                        this.selected = true;
                        return;
                    }
                });

                $("#<%=hidSelectedIcon.ClientID%>").val(licon);
                ShowSelectIcon(licon);
                $("#<%=hidSelectedColor.ClientID%>").val(lcolor);
                ShowSelecctionIcon(lcolor);
            });

            //for reload
            var sicon = $("#<%=hidSelectedIcon.ClientID%>").val();
            ShowSelectIcon(sicon);
            var scolor = $("#<%=hidSelectedColor.ClientID%>").val();
            ShowSelecctionIcon(scolor);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">
                System Admin
            </h1>
        </div>
    </div>
    <form id="Form1" runat="server">
        <section class="wrapper">
            <div class="row">
                <div class="col-lg-5">
                    <section class="panel">
                        <header class="panel-heading">
                            <h3>用户管理 User Management</h3>
                        </header>
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
                    </section>
                </div>
                <div class="col-lg-7">
                    <section class="panel">
                        <header class="panel-heading">
                            <h3>链接管理 Links Management</h3>
                        </header>
                        <div class="panel-body">
                            <fieldset>
                                <div class="form-group">
                                    <span>*name:</span>
                                    <input id="txtLinkName" type="text" 
                                        class="form-control" autocomplete="off" 
                                        style="width: 200px"
                                        runat="server"/>
                                </div>
                                <div class="form-group">
                                    <span>*Site Address:</span>
                                    <input id="txtLinkUrl" type="text" 
                                        class="form-control" autocomplete="off" 
                                        runat="server"/>
                                </div>
                                <div class="form-group">
                                    <span>*Category:</span>
                                    <asp:DropDownList ID="ddlLinkCate" runat="server"></asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <span>*Icon:</span>
                                    <div class="btn-group">
                                        <button class="btn btn-small">select</button>
                                        <button class="btn btn-large dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <asp:Repeater ID="rptIcons" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <a href="#" onclick="DoSelectIcon(this)">
                                                            <i class="fa <%#Eval("Value")%>" icon="<%#Eval("Value")%>"></i>
                                                        </a>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <i id="selLinkIcon"></i>
                                </div>
                                <div class="form-group">
                                    <span>*Background Color:</span>
                                    <div class="btn-group">
                                        <button class="btn btn-small">select</button>
                                        <button class="btn btn-large dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <asp:Repeater ID="rptColors" runat="server">
                                                <ItemTemplate>
                                                    <li><a href="#" onclick="DoSelectColor(this)">
                                                        <span class="label <%#Eval("Value")%>"><%#Eval("Value")%></span>
                                                    </a></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <span id="selLinkColor"></span>
                                </div>
                                <div class="col-lg-9 form-group">
                                    <asp:HiddenField ID="hidSelectedIcon" runat="server" />
                                    <asp:HiddenField ID="hidSelectedColor" runat="server" />
                                    <asp:HiddenField ID="hidSelectedLink" runat="server" />                                    
                                    <asp:Button ID="btnAddLink" runat="server" Text="Add Link" OnClick="btnAddLink_OnClick" />
                                    <asp:Button ID="btnUpdateLink" runat="server" Text="Update Link" OnClick="btnUpdateLink_OnClick" />
                                </div>
                            </fieldset>

                            <div class="col-lg-11" style="height: 250px; overflow-y:auto">
                                <table class="table table-striped table-hover" id="tabLinks"
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>名称 name</th>
                                            <th>网址 Site Address</th>
                                            <th>分类 Category</th>
                                            <th>图标 Icon</th>
                                            <th>背景色 BackgroundColor</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptLinks" runat="server">
                                            <ItemTemplate>
                                                <tr editkey='<%#Eval("ID")%>'>
                                                    <td><%#Eval("ID")%></td>
                                                    <td><%#Eval("name")%></td>
                                                    <td><%#Eval("url")%></td>
                                                    <td><%#Eval("categoryName")%></td>
                                                    <td>
                                                        <i class="fa <%#Eval("icon")%>"></i>
                                                    </td>
                                                    <td>
                                                        <span class="label <%#Eval("bgColor")%>"><%#Eval("bgColor")%></span>
                                                    </td>
                                                    <td>
                                                        <a onclick="DeleteLink(this)" style="color: orangered; cursor: pointer">
                                                            <i class="fa fa-remove fa-fw"></i>
                                                        </a>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </section>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-6">
                    <section class="panel">
                        <header class="panel-heading">
                            <h3>IT人员管理</h3>
                        </header>
                        <div class="panel-body" style="overflow: auto">
                            <div class="col-lg-9 form-group">
                                <asp:Label Text="name:" runat="server" />
                                <asp:TextBox ID="txtItName" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-9 form-group">
                                <asp:Label Text="Phone Number:" runat="server" />
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
                            <h3>IT人员排班 IT shift Arrangement</h3>
                        </header>
                        <div class="panel-body" style="overflow: auto">
                            <div class="col-lg-9 form-group">
                                <asp:Label Text="IT support name:" runat="server" />
                                <asp:DropDownList ID="ddlItSupportUsers" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-lg-9 form-group">
                                <asp:Label Text="Project:" runat="server" />
                                <asp:TextBox ID="txtItSupportProject" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-9 form-group">
                                <asp:Label Text="Shift:" runat="server" />
                                <asp:DropDownList ID="ddlItSupportShifts" runat="server"></asp:DropDownList>
                                <asp:Label Text="Month:" runat="server" />
                                <asp:TextBox ID="txtShiftMonth" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-9 form-group">
                                <asp:Button ID="btnAddItShift" runat="server" Text="Add Shift Arrange" OnClick="btnAddItShift_OnClick" />
                                <asp:Button ID="btnUpdateItShift" runat="server" Text="Update Shift Arrange" OnClick="btnUpdateItShift_OnClick" />
                            </div>

                            <asp:GridView ID="gvItShift" runat="server" Height="138px" Width="450px"
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
                    </section>
                </div>
            </div>
        </section>
    </form>

</asp:Content>
