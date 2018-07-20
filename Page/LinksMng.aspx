<%@ Page Title="" Language="C#" MasterPageFile="~/Page/Site.Master" AutoEventWireup="true" CodeBehind="LinksMng.aspx.cs" Inherits="FavLink.Page.LinksMng" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
    <script src="../Scripts/bootstrap-dialog.min.js" type="text/javascript"></script>
    <script src="../Scripts/BootstrapValidator.min.js" type="text/javascript"></script>

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
            var para = { 'linkid': linkID };
            AjaxSend("LinksMng.aspx/DeleteLink", JSON.stringify(para), function (result) {
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
                //var lid = $(tds[0]).text();
                var lid = $(row).attr("editkey");
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
    <div class="col-lg-10">
        <header class="panel-heading">
            <h3>链接管理 Links Management</h3>
        </header>
        <form id="Form1" runat="server">
        <div class="row">
            <div class="col-lg-7">
                <div class="form-group">
                    <span>*name:</span>
                    <input id="txtLinkName" type="text" class="form-control" autocomplete="off" style="width: 200px"
                        runat="server" />
                </div>
                <div class="form-group">
                    <span>*Site Address:</span>
                    <input id="txtLinkUrl" type="text" class="form-control" autocomplete="off" runat="server" />
                </div>
                <div class="form-group">
                    <span>*Category:</span>
                    <asp:DropDownList ID="ddlLinkCate" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <span>*Icon:</span>
                    <div class="btn-group">
                        <button class="btn btn-large dropdown-toggle" data-toggle="dropdown">
                            <span style="margin-right: 20px;">select</span><span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu">
                            <asp:Repeater ID="rptIcons" runat="server">
                                <ItemTemplate>
                                    <li><a href="#" onclick="DoSelectIcon(this)"><i class="fa <%#Eval("Value")%>" icon="<%#Eval("Value")%>">
                                    </i></a></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <i id="selLinkIcon"></i>
                </div>
                <div class="form-group">
                    <span>*Background Color:</span>
                    <div class="btn-group">
                        <button class="btn btn-large dropdown-toggle" data-toggle="dropdown">
                            <span style="margin-right: 20px;">select</span><span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu">
                            <asp:Repeater ID="rptColors" runat="server">
                                <ItemTemplate>
                                    <li><a href="#" onclick="DoSelectColor(this)"><span class="label <%#Eval("Value")%>">
                                        <%#Eval("Value")%></span> </a></li>
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
            </div>
        </div>
        <div class="row">
            <div style="height: 300px; overflow-y: auto">
                <table class="table table-striped table-hover" id="tabLinks">
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
                                    <td><%#Container.ItemIndex+1%></td>
                                    <td><%#Eval("name")%></td>
                                    <td><%#Eval("url")%></td>
                                    <td><%#Eval("categoryName")%></td>
                                    <td><i class="fa <%#Eval("icon")%>"></i></td>
                                    <td><span class="label <%#Eval("bgColor")%>"><%#Eval("bgColor")%></span></td>
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
        </form>
</div>
</asp:Content>
