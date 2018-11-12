<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Overzicht.ascx.cs" Inherits="Ncdo.Company.WebInterface.Management_Rapportage.Overzicht" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Panel ID="pnlOverzicht" runat="server">
    <h2>Projecten</h2>
    <div class="Company_c_content">
        <asp:Panel ID="FilterPanel" runat="server" DefaultButton="btnFilter" CssClass="Company_zoekfilter">
            <div class="Company_zoekfilter_links">
                Zoeken
            </div>
    
            <div class="Company_zoekfilter_midden">
                <div class="Company_zoekfilter_midden-selectbox">
                    Klant:<br />
                    <telerik:RadComboBox ID="rlbKlant" runat="server" MarkFirstMatch="true" DataTextField="Bedrijfsnaam" DataValueField="Klant_Id"
                                         EnableLoadOnDemand="True" OnItemsRequested="rlbKlant_ItemsRequested"
                                         ShowMoreResultsBox="true" EmptyMessage="Klant zoeken..." Width="150px"></telerik:RadComboBox>
                </div>
                <div class="Company_zoekfilter_midden-selectbox">
                    Locatie:<br />
                    <telerik:RadComboBox ID="rlbLocatie" runat="server" MarkFirstMatch="true" DataTextField="Naam" DataValueField="Locatie_Id"
                                         EnableLoadOnDemand="True" OnItemsRequested="rlbLocatie_ItemsRequested"
                                         ShowMoreResultsBox="true" EmptyMessage="Locatie zoeken..." Width="150px"></telerik:RadComboBox>
                </div>
                <div class="Company_zoekfilter_midden-selectbox">
                    Status:<br />
                    <telerik:RadComboBox ID="rlbStatus" runat="server" MarkFirstMatch="true"
                                         EmptyMessage="Status..." Width="150px">
                        <Items>
                            <telerik:RadComboBoxItem Text="Gestart" Value="1" />
                            <telerik:RadComboBoxItem Text="Technisch gereed" Value="3" />
                            <telerik:RadComboBoxItem Text="Administratief gereed" Value="4" />
                            <telerik:RadComboBoxItem Text="Voltooid" Value="2" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="Company_zoekfilter_midden-selectbox">
                    Projectnummer:<br />
                    <asp:TextBox ID="txtProjectnummer" runat="server"></asp:TextBox>
                </div>
                <div class="Company_zoekfilter_midden-selectbox">
                    Opdrachtnummer:<br />
                    <asp:TextBox ID="txtOpdrachtnummer" runat="server"></asp:TextBox>
                </div>
                <div class="Company_zoekfilter_midden-selectbox">
                    <asp:CheckBox ID="cbVoltooid" runat="Server" Text="Laat voltooide projecten zien" />
                </div>
            </div>
            <div class="Company_zoekfilter_rechts">
                <asp:Button ID="btnFilter" runat="server" Text="Zoeken" OnClientClick=" search(this, event); "  CssClass="Company_button Company_blauw" /><br />
                <asp:LinkButton ID="btnClearFilter" runat="server" Text="Leeg velden" 
                                onclick="btnClearFilter_Click"></asp:LinkButton>
            </div>
        </asp:Panel>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Navision import:
                    </div>
                    <div class="Company_list1-input">
                        <asp:FileUpload ID="fileUpload" runat="server" />
                    </div>
                </div>
            <asp:LinkButton ID="btnOpslaan" runat="server" CssClass="Company_button Company_groen" Text="Importeren" onclick="btnImportNavision_Click"></asp:LinkButton>

        <div class="ClearBoth">&nbsp;</div>
        <telerik:RadGrid runat="server" ID="rgProject" 
                         AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="false" PageSize="10"  AllowMultiRowSelection="true" EnableLinqExpressions="false">
            <MasterTableView NoMasterRecordsText="Geen items om te tonen." DataKeyNames="Project_Id" ClientDataKeyNames="Project_Id">
                <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="<b>{4}</b>  Pagina <b>{0}</b> van <b>{1}</b> | Item <b>{2}</b> t/m <b>{3}</b> van de <b>{5}</b>. " ShowPagerText="True" />
                <Columns>
                    <telerik:GridBoundColumn DataField="ProjectNr" UniqueName="ProjectNr" HeaderText="Projectnummer" />
                    <telerik:GridBoundColumn DataField="OpdrachtNummer" UniqueName="Opdrachtnummer" HeaderText="Opdrachtnummer" />
                    <telerik:GridBoundColumn DataField="Klant_Id" UniqueName="Klant_Id" HeaderText="Klant" />
                    <telerik:GridBoundColumn DataField="Locatie_Id" UniqueName="Locatie_Id" HeaderText="Locatie" />

                    <telerik:GridTemplateColumn UniqueName="EditUrl">
                        <ItemTemplate>
                            <a href="" target="_self">Bewerken</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NumericPages" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDataBound="rgProject_RowDataBound" />
                <DataBinding SelectMethod="GetDataAndCount" 
                             Location="~/DesktopModules/Company/Beheer/Project/ProjectGrid.asmx"
                             SortParameterType="Linq" FilterParameterType="Linq">
                </DataBinding>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
        </telerik:RadGrid>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            <% if (bFilter)
               { %>;
            setTimeout(function () { search(this, null); }, 100);
            <% } %>;
            $('#<%= cbVoltooid.ClientID %>').change(function () {
                FilterComplete(this);
            });
        });

        function rgProject_RowDataBound(sender, args) {
            var link = args.get_item().get_cell("Klant_Id");
            var Value = parseInt(link.innerHTML);

            $.ajax({
                type: 'POST',
                data: { KlantId: Value },
                url: '<%= ResolveClientUrl("~/DesktopModules/Company/Beheer/Item/Items.asmx/GetKlant") %>',
                dataType: 'text',
                async: false,
                success: function (data, status) {

                    link.innerHTML = data;

                },
                error: function (data, status, e) {
                }
            });


            link = args.get_item().get_cell("Locatie_Id");
            Value = parseInt(link.innerHTML);

            $.ajax({
                type: 'POST',
                data: { LocatieId: Value },
                url: '<%= ResolveClientUrl("~/DesktopModules/Company/Beheer/Klant/Klanten.asmx/GetLocatie") %>',
                dataType: 'text',
                async: false,
                success: function (data, status) {

                    link.innerHTML = data;

                },
                error: function (data, status, e) {
                }
            });

            var EditUrl = args.get_item().get_cell("EditUrl");
            EditUrl.innerHTML = '<a href="<%= sEditUrl %>' + args.get_dataItem()["Project_Id"] + '" target="_self" class="Company_button-10 Company_blauw">Openen</a>';
            }


            function search(sender, eventArgs) {

                var ProjectKlant = "";
                var ProjectLocatie = "";
                var ProjectStatus = "";
                var ProjectNummer = "";
                var ProjectOpdrachtnummer = "";
                var ProjectNaam = "";

                if (eventArgs != null) {
                    eventArgs.cancelBubble = true;
                    eventArgs.returnValue = false;
                    if (eventArgs.stopPropagation) {
                        eventArgs.stopPropagation();
                        eventArgs.preventDefault();
                    }
                }

                var tableView = $find("<%= rgProject.ClientID %>").get_masterTableView();
            var vFilterExpressions = tableView.get_filterExpressions();

            vFilterExpressions.clear();

            if ($('#<%= txtProjectnummer.ClientID %>').val() != "") {
                filterExpression = new Telerik.Web.UI.GridFilterExpression();
                filterExpression.set_fieldName("ProjectNr");
                filterExpression.set_columnUniqueName("ProjectNr");
                filterExpression.set_dataTypeName("System.String");
                filterExpression.set_filterFunction(Telerik.Web.UI.GridFilterFunction.Contains);
                ProjectNummer = $('#<%= txtProjectnummer.ClientID %>').val();
                filterExpression.set_fieldValue(ProjectNummer);

                vFilterExpressions.add(filterExpression);
            }

            if ($('#<%= txtOpdrachtnummer.ClientID %>').val() != "") {
                filterExpression = new Telerik.Web.UI.GridFilterExpression();
                filterExpression.set_fieldName("Opdrachtnummer");
                filterExpression.set_columnUniqueName("Opdrachtnummer");
                filterExpression.set_dataTypeName("System.String");
                filterExpression.set_filterFunction(Telerik.Web.UI.GridFilterFunction.Contains);
                ProjectOpdrachtnummer = $('#<%= txtOpdrachtnummer.ClientID %>').val();
                filterExpression.set_fieldValue(ProjectOpdrachtnummer);

                vFilterExpressions.add(filterExpression);
            }

            if ($find('<%= rlbKlant.ClientID %>').get_value() != "") {
                filterExpression = new Telerik.Web.UI.GridFilterExpression();
                filterExpression.set_fieldName("Klant_Id");
                filterExpression.set_columnUniqueName("Klant_Id");
                filterExpression.set_dataTypeName("System.UInt32");
                filterExpression.set_filterFunction(Telerik.Web.UI.GridFilterFunction.EqualTo);
                ProjectKlant = $find('<%= rlbKlant.ClientID %>').get_value();
                filterExpression.set_fieldValue(ProjectKlant);

                vFilterExpressions.add(filterExpression);
            }

            if ($find('<%= rlbLocatie.ClientID %>').get_value() != "") {
                filterExpression = new Telerik.Web.UI.GridFilterExpression();
                filterExpression.set_fieldName("Locatie_Id");
                filterExpression.set_columnUniqueName("Locatie_Id");
                filterExpression.set_dataTypeName("System.UInt32");
                filterExpression.set_filterFunction(Telerik.Web.UI.GridFilterFunction.EqualTo);
                ProjectLocatie = $find('<%= rlbLocatie.ClientID %>').get_value();
                filterExpression.set_fieldValue(ProjectLocatie);

                vFilterExpressions.add(filterExpression);
            }

            if ($find('<%= rlbStatus.ClientID %>').get_value() != "") {
                filterExpression = new Telerik.Web.UI.GridFilterExpression();
                filterExpression.set_fieldName("Status");
                filterExpression.set_columnUniqueName("Status");
                filterExpression.set_dataTypeName("System.UInt32");
                filterExpression.set_filterFunction(Telerik.Web.UI.GridFilterFunction.EqualTo);
                ProjectStatus = $find('<%= rlbStatus.ClientID %>').get_value();
                filterExpression.set_fieldValue(ProjectStatus);

                vFilterExpressions.add(filterExpression);
            }

            tableView.rebind();

            $.ajax({
                type: 'POST',
                data: { Klant: ProjectKlant, Locatie: ProjectLocatie, Status: ProjectStatus, Projectnummer: ProjectNummer, Opdrachtnummer: ProjectOpdrachtnummer },
                url: '<%= ResolveClientUrl("~/DesktopModules/Company/Beheer/Project/Projecten.asmx/RememberFilter") %>',
                dataType: 'text',
                async: false,
                success: function (data, status) {
                },
                error: function (data, status, e) {
                    alert(e);
                }
            });

        }

        function FilterComplete(sender) {
            $.ajax({
                type: 'POST',
                data: { Active: $('#<%= cbVoltooid.ClientID %>').is(':checked') },
                url: '<%= ResolveClientUrl("~/DesktopModules/Company/Beheer/Project/Projecten.asmx/ShowVoltooid") %>',
                dataType: 'text',
                async: false,
                success: function (data, status) {
                    search(this, null);
                },
                error: function (data, status, e) {
                    alert(e);
                }
            });
        }

    </script>
</asp:Panel>

<% if (!string.IsNullOrEmpty(sDownloadUrl))
   { %>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () { window.location = "<%= sDownloadUrl %>"; }, 750);
        });
    </script>
<% } %>