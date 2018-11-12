<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemRapportage.ascx.cs" Inherits="Ncdo.Company.WebInterface.Management_Rapportage.ItemRapportage" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style type="text/css" media="all">
    html { overflow: hidden; }
</style>
<div class="Company_iframe_page">
    <script type="text/javascript">

        function resize() {
            var height = getDocHeight();
            var elements = window.top.document.getElementsByTagName("div");

            for (var i = 0; i < elements.length; i++) {
                var containerPageViewID = "rpvDocumenten";

                if (elements[i].id.indexOf(containerPageViewID) > -1) {
                    elements[i].style.height = height + "px";
                    break;
                }
            }
        }

        if (window.addEventListener)
            window.addEventListener("load", resize, false);
        else if (window.attachEvent)
            window.attachEvent("onload", resize);
        else window.onload = resize;

        function getDocHeight() {
            var D = document;
            return Math.max(
                Math.max(D.body.scrollHeight, D.documentElement.scrollHeight),
                Math.max(D.body.offsetHeight, D.documentElement.offsetHeight),
                Math.max(D.body.clientHeight, D.documentElement.clientHeight)
            );
        }

    </script>
     <asp:Panel ID="Panel1" runat="server" CssClass="Company_c_content">
     <asp:Panel ID="FilterPanel2" runat="server" CssClass="Company_c_content">
       <div class="Company_list1-wrap">
            <div class="Company_list1">
                <h2>Opbrengsten</h2>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Basis:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextBasis" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Materiaal:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextMateriaalOpbrengsten" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Meerwerk:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextMeerwerk" runat="server" Width="85px"></asp:TextBox>&nbsp;&nbsp;
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <h2>Kosten</h2>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Uren:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextUren" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Materiaal:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextMateriaalKosten" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Materieel:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextMaterieel" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Onderaannemer:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextOnderaannemer" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Overige:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextOverige" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
         <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Totaal:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextTotaleOpbrengsten" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Totaal:
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextTotaleKosten" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
                 <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                    </div>
                    <div class="Company_list1-item">
                        <h2>Uren</h2>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Basis
                    </div>
                    <div class="Company_list1-item">
                        Workfl.
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextUrenBasis" runat="server"></asp:TextBox>
                    </div>
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextUrenWorkfl" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        &nbsp
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Navis.
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-input">
                        <asp:TextBox ID="TextUrenNavis" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_list-row">
            <div class="Company_list1-item">
                Toezicht
            </div>
            <div class="Company_list1-input">
                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
            </div>
            <div class="Company_list1-input">
                <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="Company_list-row">
            <div class="Company_list1-item">
                Wvb
            </div>
            <div class="Company_list1-input">
                <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
            </div>
            <div class="Company_list1-input">
                <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="Company_list-row">
            <div class="Company_list1-item">
                Directen
            </div>
            <div class="Company_list1-input">
                <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
            </div>
            <div class="Company_list1-input">
                <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
            </div>
        </div>
     </asp:Panel>
    <div class="clearboth">&nbsp;</div>

     <asp:Panel ID="Panel2" runat="server" CssClass="Company_c_content">
    <telerik:RadGrid runat="server" ID="rgDocument" 
                     AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10"  AllowMultiRowSelection="true" EnableLinqExpressions="false" Visible="False">
        <MasterTableView NoMasterRecordsText="Geen items om te tonen." DataKeyNames="Medewerkers" ClientDataKeyNames="Document_Id">
            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="<b>{4}</b>  Pagina <b>{0}</b> van <b>{1}</b> | Item <b>{2}</b> t/m <b>{3}</b> van de <b>{5}</b>. " ShowPagerText="True" />
            <Columns>
                <telerik:GridBoundColumn DataField="Medewerkers" UniqueName="Medewerkers" HeaderText="Medewerkers"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TotMinuten" UniqueName="TotMinuten" HeaderText="TotMinuten"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TotUren" UniqueName="TotUren" HeaderText="TotUren"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Navision" UniqueName="Navision" HeaderText="Navision"></telerik:GridBoundColumn>
            </Columns>
            <PagerStyle Mode="NumericPages" />
        </MasterTableView>
        <ClientSettings>
            <ClientEvents OnRowDataBound="rgDocument_RowDataBound" />
            <DataBinding SelectMethod="GetDataAndCount" 
                         Location="~/DesktopModules/Company/Beheer/Item/DocumentenGrid.asmx"
                         SortParameterType="Linq" FilterParameterType="Linq">
            </DataBinding>
        </ClientSettings>
    </telerik:RadGrid>
    <script type="text/javascript">
        var $jParent = window.parent.jQuery.noConflict();

        function rgDocument_RowDataBound(sender, args) {

        }

        function search(sender, eventArgs) {
            eventArgs.cancelBubble = true;
            eventArgs.returnValue = false;
            if (eventArgs.stopPropagation) {
                eventArgs.stopPropagation();
                eventArgs.preventDefault();
            }

            var tableView = $find("<%= rgDocument.ClientID %>").get_masterTableView();
            var vFilterExpressions = tableView.get_filterExpressions();

            vFilterExpressions.clear();

            tableView.rebind();

        }

    </script>
    <div class="clearboth">&nbsp;</div>
            <asp:Table ID="ItemOverzicht" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="1">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ID="Medewerkers" Text="Medewerkers" runat="server" >Medewerkers</asp:TableHeaderCell>
                    <asp:TableHeaderCell ID="TotMinuten" Text="TotMinuten" runat="server" >TotMinuten</asp:TableHeaderCell>
                    <asp:TableHeaderCell ID="TotUren" Text="TotUren" runat="server" >TotUren</asp:TableHeaderCell>
                    <asp:TableHeaderCell ID="Navision" Text="Navision" runat="server" >Navision</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
     </asp:Panel>
  </asp:Panel>
</div>