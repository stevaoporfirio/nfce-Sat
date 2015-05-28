<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RelatorioNFCE._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 245px;
        }
        .style2
        {
            width: 77px;
        }
        .style3
        {
            width: 136px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Visualizar</h2>
    <p>
        <table style="width:100%;">
            <tr>
                <td class="style1" colspan="2">
                    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3">
                </td>
                <td class="style2">
                </td>
                <td>
                    </td>
                <td>
                    </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Value="0">Todas</asp:ListItem>
                        <asp:ListItem Value="1">Enviadas</asp:ListItem>
                        <asp:ListItem Value="2">Rejeitadas</asp:ListItem>
                        <asp:ListItem Value="4">Fila Contingência</asp:ListItem>
                        <asp:ListItem Value="8">Env. Conting.</asp:ListItem>
                        <asp:ListItem Value="16">Rejeitada Conting.</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                        Text="Relatório" />
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>

    </p>
    <p>
        <asp:GridView ID="GridView1" runat="server" BackColor="White" 
            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            GridLines="Vertical" onrowdatabound="GridView1_RowDataBound">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
                   <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" Text="Editar" OnClick="lnkEdit_Click" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                </Columns>
        </asp:GridView>
        </p>
</asp:Content>
