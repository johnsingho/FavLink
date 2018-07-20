<%@ Page Title="" Language="C#" MasterPageFile="~/Page/Site.Master" AutoEventWireup="true" CodeBehind="FavIndex.aspx.cs" Inherits="FavLink.Page.FavIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-12">
                        <h1 class="page-header">
                            <%=FavLink.Entity.Common.GetLinkPageTitle(CateID)%>
                        </h1>
                    </div>
                </div>
                <div class="row">
                <asp:Repeater ID="rptLinks" runat="server">
                    <ItemTemplate>
                        <a class="quick-button metro span2 <%#Eval("bg_color")%>" href='<%#Eval("url")%>'>
                            <i class="fa <%#Eval("icon")%>"></i>
                            <p><%#Eval("name")%></p>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
                <%--<div class="clearfix"></div>--%>
            </div>
        </div>
</asp:Content>
