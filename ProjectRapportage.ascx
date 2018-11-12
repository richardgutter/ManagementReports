<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectRapportage.ascx.cs" Inherits="Ncdo.Company.WebInterface.Management_Rapportage.ProjectRapportage" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css" media="all">
    html
    {
        overflow: hidden;
    }
</style>
<div class="Company_iframe_page" id="test">
    <script type="text/javascript">

        function resize() {
            var height = getDocHeight();
            var elements = window.top.document.getElementsByTagName("div");

            for (var i = 0; i < elements.length; i++) {
                var containerPageViewID = "rpvManagementInfo";

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

    <h2>Management rapportage</h2>
    <asp:Panel ID="pnlTabs" runat="Server" CssClass="Detail_TabsWrap">
        <telerik:RadTabStrip ID="rtsKlant" runat="server" Skin="Web20" MultiPageID="rmpItem"
            SelectedIndex="0" CssClass="tabStrip noBorder" Width="697px"><Tabs><telerik:RadTab Text="Overzicht"></telerik:RadTab></Tabs></telerik:RadTabStrip>
        <div class="Company_c_content noMargin">
            <telerik:RadMultiPage ID="rmpCalculatie" runat="server" SelectedIndex="0" CssClass="multiPage" EnableAjaxSkinRendering="true" ScrollBars="Hidden"><telerik:RadPageView ID="rpvCalculatieOverzicht" runat="server" Height="500" Width="700px">
                    <telerik:RadGrid runat="server" ID="rgOverzicht" Width="680px"
                        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10" EnableLinqExpressions="false">
                        <MasterTableView NoMasterRecordsText="Geen regels om te tonen." DataKeyNames="Item_Id" ClientDataKeyNames="Item_Id">
                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="<b>{4}</b>  Pagina <b>{0}</b> van <b>{1}</b> | Item <b>{2}</b> t/m <b>{3}</b> van de <b>{5}</b>. " ShowPagerText="True" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="Item_Id" UniqueName="Item_Id" HeaderText="Item_Id" Visible="false" />
                                <telerik:GridBoundColumn DataField="TagNr" UniqueName="TagNr" HeaderText="Tag Nr" />
                                <telerik:GridBoundColumn DataField="Item_Identifier" UniqueName="Item_Identifier" HeaderText="Item nummer" />
                                <telerik:GridBoundColumn DataField="Item_Soort_Id" UniqueName="Item_Soort_Id" HeaderText="Soort" />
                                <telerik:GridBoundColumn DataField="TotaalPrijs" UniqueName="TotaalPrijs" HeaderText="Bedrag (in euro)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                <telerik:GridTemplateColumn UniqueName="OpenUrl">
                                    <ItemTemplate>
                                        <a href="" target="_self">Openen</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NumericPages" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true">
                            <ClientEvents OnRowDataBound="rgItems_RowDataBound" />
                            <DataBinding SelectMethod="GetDataAndCount"
                                Location="~/DesktopModules/Company/Beheer/Project/ItemGrid.asmx"
                                SortParameterType="Linq" FilterParameterType="Linq">
                            </DataBinding>
                        </ClientSettings>
                    </telerik:RadGrid>

                    <div class="Company-calculatie-bottom">
                        <div class="TotalHolder2">
                            <div class="totaltext">
                                Totaal bedrag opbrengsten: 
                            </div>
                            <div class="totalamount">
                                <asp:Label ID="lblTotaalbedragOpbrengsten" runat="Server" CssClass="Company-calculatie-groen"></asp:Label>
                            </div>
                            <br />
                            <div class="totaltext">
                                Totaal bedrag kosten: 
                            </div>
                            <div class="totalamount">
                                <asp:Label ID="lblTotaalbedragKosten" runat="Server" CssClass="Company-calculatie-groen"></asp:Label>
                            </div>
                            <br />
                            <div class="totaltext">
                                Verschil: 
                            </div>
                            <div class="totalamount">
                                <asp:Label ID="lblTotaalVerschil" runat="Server" CssClass="Company-calculatie-groen"></asp:Label>
                            </div>
                        </div>
                    </div>


                </telerik:RadPageView></telerik:RadMultiPage>
        </div>
    </asp:Panel>

    <script type="text/javascript">

        function rgItems_RowDataBound(sender, args) {
            var link = args.get_item().get_cell("Item_Soort_Id");
            var Value = parseInt(link.innerHTML);

            $.ajax({
                type: 'POST',
                data: { SoortId: Value },
                url: '<%= ResolveClientUrl("~/DesktopModules/Company/Beheer/Item/Items.asmx/GetSoort") %>',
                dataType: 'text',
                async: false,
                success: function (data, status) {

                    link.innerHTML = data;

                },
                error: function (data, status, e) {
                }
            });

            var OpenUrl = args.get_item().get_cell("OpenUrl");
            OpenUrl.innerHTML = '<a href="<%= sOpenUrl %>' + args.get_dataItem()["Item_Id"] + '" target="_self" class="Company_button-10 Company_blauw" target="_blank">Openen</a>';
        }

    </script>

    <asp:LinkButton ID="btnShowRapport" runat="server" Text="Toon management rapportage" onclick="btnShowRapport_Click" CssClass="Company_icon-bekijken"></asp:LinkButton>
    
    <div class="ClearBoth">&nbsp;</div>

    <asp:Panel ID="FilterPanel2" runat="server" CssClass="Company_c_content" Height="450">
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <h2>Opbrengsten</h2>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Basis:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextBasisOpbrengsten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Materiaal:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextMateriaalOpbrengsten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Meerwerk:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextMeerwerk" runat="server"></asp:Label>
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
                    <div class="Company_list1-item">
                        <asp:Label ID="TextBasisKosten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Materiaal:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextMateriaalKosten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Materieel:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextMaterieel" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Onderaannemer:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextOnderaannemer" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Overige:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextOverige" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item" style="font-weight: bold">
                        Totaal:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextTotaleOpbrengsten" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item" style="font-weight: bold">
                        Totaal:
                    </div>
                    <div class="Company_list1-item">
                        <asp:Label ID="TextTotaleKosten" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="clearboth">&nbsp;</div>

        <div class="Company_rapportage-wrap">
            <h2>Uren</h2>
            <div class="Company_rapportage">
                <div class="Company_rapportage-row">
                    <div class="Company_rapportage-item">
                    </div>
                    <div class="Company_rapportage-item">
                        Basis
                    </div>
                    <div class="Company_rapportage-item">
                        Workfl.
                    </div>
                    <div class="Company_rapportage-item">
                        Navis.
                    </div>
                </div>
                <div class="Company_rapportage-row">
                    <div class="Company_rapportage-item">
                        Toezicht
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextToezichtBasis" runat="server"></asp:Label>
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextToezichtWorkfl" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_rapportage-item">
                        Wvb
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextWvbBasis" runat="server"></asp:Label>
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextWvbWorkfl" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_rapportage-row">
                    <div class="Company_rapportage-item">
                        Directen
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextDirectenBasis" runat="server"></asp:Label>
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextDirectenWorkfl" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_rapportage-row">
                    <div class="Company_rapportage-item" Style="font-weight: bold">
                        Totaal
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextMinutenBasis" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextMinutenWorkfl" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                    <div class="Company_rapportage-item">
                        <asp:Label ID="TextMinutenNavis" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
     <div class="clearboth">&nbsp;</div>

    <asp:Table ID="ItemOverzicht" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="1" Width="100%" class="Company-calculatie-rapportage-table">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell ID="Medewerkers" Text="Medewerkers" runat="server">Medewerkers</asp:TableHeaderCell>
            <asp:TableHeaderCell ID="TotMinuten" Text="TotMinuten" runat="server">TotMinuten</asp:TableHeaderCell>
            <asp:TableHeaderCell ID="TotUren" Text="TotUren" runat="server">TotUren</asp:TableHeaderCell>
            <asp:TableHeaderCell ID="Navision" Text="Navision" runat="server">Navision</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>

   <div class="clearboth">&nbsp;</div>

    <telerik:RadGrid runat="server" ID="rgDocument"
        AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10" AllowMultiRowSelection="true" EnableLinqExpressions="false" Visible="False">
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
</div>

<% if (!string.IsNullOrEmpty(SRedirectUrl))
   { %>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var pop = window.open('<%= SRedirectUrl %>');
            pop.onunload = function () { setTimeout(function () { search(this, null); }, 100); };
        });
    </script>
<% } %>