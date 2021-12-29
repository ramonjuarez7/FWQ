<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Front3._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Mapa del parque</h1>
        <p>
            <asp:Button ID="botonmapa" runat="server" OnClick="botonmapa_Click" Text="Ver mapa" />
        </p>
        <p>
            <asp:Label ID="Mapa" runat="server" Text=" "></asp:Label>
        </p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Usuarios</h2>
            <p>
                <asp:Button ID="botonusuarios" runat="server" Text="Ver usuarios" OnClick="botonusuarios_Click" />
            </p>
            <p>
                &nbsp;</p>
        </div>
    </div>

</asp:Content>
