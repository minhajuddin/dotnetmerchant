<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" %>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<asp:Content runat="server" ContentPlaceHolderID="TitleContent">
    Login
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">
     <% Html.RenderPartial("LoginControl"); %>
</asp:Content>