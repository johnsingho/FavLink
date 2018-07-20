<%@ Page Title="IT Support contact" Language="C#" MasterPageFile="~/Page/Site.Master" AutoEventWireup="true" CodeBehind="Contactus.aspx.cs" Inherits="FavLink.Page.Contactus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
<div >
    <div class="row">
        <div class="col-md-11">
            <h1 class="page-header" style="text-align: center;">
                IT Support Arrangment
            </h1>
        </div>
    </div>
    <div>
        <%=GetHotlinesList() %>
    </div>

<div class="col-md-8" style="font-size:14px;">
    <h4 class='underline'>Shift Arrange(<%=GetYearMon() %>)</h4>
    <div class="lsLeft">
<table class="table table-striped table-bordered table-condensed cf" id="itSupportTable" aria-describedby="itSupport_info">
    <thead>
        <tr role="row" class="rowhead">
            <th role="columnheader" tabindex="0" aria-controls="itSupportTable" rowspan="1" colspan="1">
                name
            </th>
            <th role="columnheader" tabindex="0" aria-controls="itSupportTable" rowspan="1" colspan="1">
                Phone Number
            </th>
            <th role="columnheader" tabindex="0" aria-controls="itSupportTable" rowspan="1" colspan="1">
                Shift
            </th>
            <th role="columnheader" tabindex="0" aria-controls="itSupportTable" rowspan="1" colspan="1"
                class="col-md-5">
                Project
            </th>
        </tr>
    </thead>
    <tbody role="alert" aria-live="polite" aria-relevant="all">
    <asp:Repeater ID="rptLinks" runat="server">
    <ItemTemplate>
        <tr class="<%#GetRowClass(Container.ItemIndex+1)%>">
            <td class="left">
                <span><%#Eval("name")%></span>
            </td>
            <td class="left">
                <span><%#Eval("phone_number")%></span>
            </td>
            <td class="left">
                <span><%#GetShiftStr(Eval("shift"))%></span>
            </td>
            <td class="left">
                <span><%#Eval("project")%></span>
            </td>
        </tr>
        </ItemTemplate>
    </asp:Repeater>

    </tbody>
</table>
</div>
</div>
</div>

</asp:Content>


