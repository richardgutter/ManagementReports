<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Detail.ascx.cs" Inherits="Ncdo.Company.WebInterface.Management_Rapportage.Detail" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Panel ID="pnlDetail" runat="server">
    <h2 id="projectTitle" runat="server">
        Project detail</h2>
   <asp:Panel ID="pnlGeneralData" runat="server" CssClass="Company_c_content">
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Project nummer:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblProjectNummer" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Omschrijving:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="txtOmschrijving" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Klant:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblKlant" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Locatie:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblLocatie" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Technisch contact:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblTechContact" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Administratief:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblAdminContact" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item ">
                        Opdrachtnummer:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="txtOpdrachtnummer" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item ">
                        Aanneemsom in &euro;:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="txtAanneemsom" runat="server" MaxLength="150"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_list1-wrap">
            <div class="Company_list1">
                <div class="Company_list-row">
                    <div class="Company_list1-item ">
                        Opdrachtdatum:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblOpdrachtdatum" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Werkvoorbereider:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblWerkvoorbereider" runat="server"></asp:Label>
                        <script type="text/javascript">
                            function OnClientSelectedIndexChanged(sender, eventArgs) {
                                var item = eventArgs.get_item();
                                $("#<%= txtInkoper.ClientID %>").val(item.get_text());
                            }
                        </script>
                    </div>
                </div>
                <div class="Company_list-row" id="ContactRow" runat="server">
                    <div class="Company_list1-item">
                        Begin datum:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblBeginDatum" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row" id="Div1" runat="server">
                    <div class="Company_list1-item">
                        Eind datum:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblEindDatum" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Soort werk:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblSoortWerk" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="Company_list-row">
                    <div class="Company_list1-item">
                        Project status:
                    </div>
                    <div class="Company_list1-input">
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="Company_button-wrap">
            <asp:Button ID="btnAnnuleren" runat="server" CssClass="Company_button Company_blauw"
                        Text="Terug" OnClick="btnAnnuleren_Click"></asp:Button>
        </div>
    </asp:Panel>
    <asp:ValidationSummary ID="vsProject" runat="server" DisplayMode="BulletList" HeaderText="Wij willen u wijzen op de volgende fouten:"
                           ShowSummary="true" ValidationGroup="Company_ProjectDetailValidatie" CssClass="dnnFormMessage dnnFormValidationSummary" />
       <asp:Panel ID="pnlTabs" runat="Server" CssClass="Detail_TabsWrap">
        <telerik:RadTabStrip ID="rtsKlant" runat="server" Skin="Web20" MultiPageID="rmpKlant"
                             SelectedIndex="0" CssClass="tabStrip noBorder"><Tabs><telerik:RadTab Text="Klant gegevens" Visible="False"></telerik:RadTab><telerik:RadTab Text="Opstart gegevens" Visible="False"></telerik:RadTab><telerik:RadTab Text="Projectgegevens" Visible="False"></telerik:RadTab><telerik:RadTab Text="Items" Visible="False"></telerik:RadTab><telerik:RadTab Text="Calculatie" Visible="False"></telerik:RadTab><telerik:RadTab Text="Facturatie" Visible="False"></telerik:RadTab><telerik:RadTab Text="Documenten" Visible="False"></telerik:RadTab><telerik:RadTab Text="Management Rapportage"  Selected="True"></telerik:RadTab></Tabs></telerik:RadTabStrip>
        <div class="Company_c_content noMargin">
            <telerik:RadMultiPage ID="rmpKlant" runat="server" SelectedIndex="0" CssClass="multiPage"
                                  EnableAjaxSkinRendering="true" ScrollBars="Hidden"><telerik:RadPageView ID="rpvKlantGegevens" runat="server" Visible="False">
                    <div class="Company_list1-wrap">
                        <div class="Company_list1">
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Klantnummer:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtOpdrachtgeverklantnummer" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Werkadrescode:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtWerkadrescode" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Projecttype:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlProjecttype" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Projecttype..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="DIR-DERDEN" Value="DIR-DERDEN" Selected="true" />
                                            <telerik:RadComboBoxItem Text="DIR-Company" Value="DIR-Company" />
                                            <telerik:RadComboBoxItem Text="DIR-Company" Value="DIR-KPL" />
                                            <telerik:RadComboBoxItem Text="DIR-Company" Value="IND-CALCULATIE" />
                                            <telerik:RadComboBoxItem Text="DIR-Company" Value="IND-OVERIG" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Regio:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlRegio" runat="server" MarkFirstMatch="true" EmptyMessage="Kies regio..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Buitenland" Value="Buitenland" />
                                            <telerik:RadComboBoxItem Text="Midden-West" Value="Midden-West" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Noord-Oost" Value="Noord-Oost" />
                                            <telerik:RadComboBoxItem Text="Noord-West" Value="Noord-West" />
                                            <telerik:RadComboBoxItem Text="Zuid-Oost" Value="Zuid-Oost" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Soort:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlSoort" runat="server" MarkFirstMatch="true" EmptyMessage="Kies soort..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="AW - Aangenomen werk" Value="AW - Aangenomen werk"
                                                                     Selected="true" />
                                            <telerik:RadComboBoxItem Text="C - Consultency" Value="C - Consultency" />
                                            <telerik:RadComboBoxItem Text="DET - Detachering" Value="DET - Detachering" />
                                            <telerik:RadComboBoxItem Text="HAN - Handel" Value="HAN - Handel" />
                                            <telerik:RadComboBoxItem Text="REG - Regie" Value="REG - Regie" />
                                            <telerik:RadComboBoxItem Text="TA - Turn Around, Stops" Value="TA - Turn Around, Stops" />
                                            <telerik:RadComboBoxItem Text="URA - Unit-rate, aang.Werk karakter" Value="URA - Unit-rate, aang.Werk karakter" />
                                            <telerik:RadComboBoxItem Text="URR - Unit-rate, Regie karakter" Value="URR - Unit-rate, Regie karakter" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Kostenplaatscode:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlKostenplaatscode" runat="server" MarkFirstMatch="true"
                                                         EmptyMessage="Kies Kostenplaats..." Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="534" Value="534" Selected="true" />
                                            <telerik:RadComboBoxItem Text="110 - Industrie Noord" Value="110 - Industrie Noord" />
                                            <telerik:RadComboBoxItem Text="120 - E&I Oost" Value="120 - E&I Oost" />
                                            <telerik:RadComboBoxItem Text="131 - Machnical Noord" Value="131 - Machnical Noord" />
                                            <telerik:RadComboBoxItem Text="132 - Machnical Oost" Value="132 - Machnical Oost" />
                                            <telerik:RadComboBoxItem Text="141 - AS Noord" Value="141 - AS Noord" />
                                            <telerik:RadComboBoxItem Text="142 - AS Oost" Value="142 - AS Oost" />
                                            <telerik:RadComboBoxItem Text="150 - Process Solutions" Value="150 - Process Solutions" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Enkel/Hoofd/Deel:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlEHD" runat="server" MarkFirstMatch="true" EmptyMessage="Kies E / H / D..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Deelproject" Value="Deelproject" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Enkel" Value="Enkel" />
                                            <telerik:RadComboBoxItem Text="Hoofd" Value="Hoofd" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Hoofdproject:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtDeelprojectBehorendeBij" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="Company_list1-wrap">
                        <div class="Company_list1">
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Branche:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlBranche" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Branche..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="MI - Metaalindustrie DJ" Text="MI - Metaalindustrie DJ">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="OG - Olie en Gas CA 11 = DF 232" Text="OG - Olie en Gas CA 11 = DF 232"
                                                                     Selected="True"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="PP - Papier DE 21" Text="PP - Papier DE 21"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="RK - Rubber en Kunstof DH" Text="RK - Rubber en Kunstof DH">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="SBR - Scheepsbouw en Rep DM 351" Text="SBR - Scheepsbouw en Rep DM 351">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="TX - Vervaardigen van Textiel DB" Text="TX - Vervaardigen van Textiel DB">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="UT - Bouwnijverheid F" Text="UT - Bouwnijverheid F">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="VOA - Vervoer- Opslag- & Overslag, Airports I 60, 63"
                                                                     Text="VOA - Vervoer- Opslag- & Overslag, Airports I 60, 63"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Competenties:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlCompetenties" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Competenties..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="IA SW MES - Software Engineering IT (MES)" Text="IA SW MES - Software Engineering IT (MES)">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="IA SW P/S - Software Engineering PLC / SCADA" Text="IA SW P/S - Software Engineering PLC / SCADA">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="ME - Maintenance Engineering" Text="ME - Maintenance Engineering">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="PK - Process Kennis" Text="PK - Process Kennis">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="W EN - Mechanical Engineering" Text="W EN - Mechanical Engineering">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="W KR - Kleppen Revisie" Text="W KR - Kleppen Revisie"
                                                                     Selected="True"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="W P&M - Piping & Mechanical" Text="W P&M - Piping & Mechanical">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="W R - Rotating" Text="W R - Rotating"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Marktsector:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlMarktsector" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Marktsector..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="B - Buildings" Text="B - Buildings"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="I - Industry" Text="I - Industry" Selected="True">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="IN - Infrastructure" Text="IN - Infrastructure">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="M - Marine" Text="M - Marine"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="T - Telecoms" Text="T - Telecoms"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="O - Other" Text="O - Other"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Functie:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlFunctie" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Functie..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="C - Consultancy" Text="C - Consultancy"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="E - Engineering" Text="E - Engineering"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="I - Installation" Text="I - Installation"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="O - Other" Text="O - Other" Selected="True"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Technical discipline:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlTechnicalDiscipline" runat="server" MarkFirstMatch="true"
                                                         EmptyMessage="Kies Functie..." Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="Kies Technical discipline"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="E - Electrical Engineering" Text="E - Electrical Engineering">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="M - Mechanical Engineering" Text="M - Mechanical Engineering">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="I - ICT" Text="I - ICT"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="O - Other" Text="O - Other" Selected="True"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Business sector:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlBusinessSector" runat="server" MarkFirstMatch="true"
                                                         EmptyMessage="Kies Business sector..." Width="170px">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="C - Construction contracts" Text="C - Construction contracts">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="R - Redering service" Text="R - Redering service">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="S - Sales of goods" Text="S - Sales of goods"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="I - Insurance contracts" Text="I - Insurance contracts">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="L - Lease and rentals" Text="L - Lease and rentals">
                                            </telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="RO - Royalties" Text="RO - Royalties"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Gepland/Ongepland:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlPlanning" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Planning..."
                                                         Width="170px">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Gepland" Text="Gepland"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Ongepland" Text="Ongepland"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </telerik:RadPageView><telerik:RadPageView ID="rpvOpstartGegevens" runat="server" Visible="False">
                    <div class="Company_list1-wrap">
                        <div class="Company_list1">
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Kopier Project:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtKopierProject" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Trajectofferte:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtTrajectofferte" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Calculatienummer:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtCalculatienummer" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Overzettencalculatie naar:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtOverzettencalculatie" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Werkbegroting bevroren:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlWerkbegroting" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Ja/Nee..."
                                                         Width="170px">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="Kies "></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Ja" Text="Ja"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Nee" Text="Nee"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="Company_list1-wrap">
                        <div class="Company_list1">
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Calculator:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtCalculator" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Uitvoerder:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="rcbUitvoerder" runat="server" MarkFirstMatch="true" DataTextField="WeergaveNaam"
                                                         DataValueField="Medewerker_Id" ShowMoreResultsBox="true" EmptyMessage="Uitvoerder zoeken..."
                                                         Width="170px">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Projectleider:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="rcbProjectleider" runat="server" MarkFirstMatch="true" DataTextField="WeergaveNaam"
                                                         DataValueField="Medewerker_Id" ShowMoreResultsBox="true" EmptyMessage="Projectleider zoeken..."
                                                         Width="170px">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Inkoper (Expediter):
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtInkoper" runat="server" ReadOnly="true" CssClass="Company_list_disabled"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Opzichter (Klant):
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="rcbOpzichter" runat="server" MarkFirstMatch="true" DataTextField="Naam"
                                                         DataValueField="Contact_Id" ShowMoreResultsBox="true" EmptyMessage="Seleteer eerst een locatie."
                                                         Enabled="false" Width="170px">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </telerik:RadPageView><telerik:RadPageView ID="rpvProjectgegevens" runat="server" Visible="False">
                    <div class="Company_list1-wrap">
                        <div class="Company_list1">
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Verrekenmethode:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlVerrekenmethode" runat="server" MarkFirstMatch="true"
                                                         EmptyMessage="Kies Verrekenmethode..." Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="Kies "></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Aanneming" Text="Aanneming" Selected="True"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Regie" Text="Regie"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Contractsoort:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlContractsoort" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Contractsoort..."
                                                         Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Hoofdaanneming" Text="Hoofdaanneming" Selected="True">
                                            </telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Boeken op element:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlBoekenOpElementVerplicht" runat="server" MarkFirstMatch="true"
                                                         EmptyMessage="Kies Nee/Ja..." Width="170px" CssClass="Company_list_disabled">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Ja" Text="Ja"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Nee" Text="Nee" Selected="True"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Regie Debiteur:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtRegieDebiteur" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="Company_list1-wrap">
                        <div class="Company_list1">
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Ontvangst:
                                </div>
                                <div class="Company_list1-input">
                                    <telerik:RadComboBox ID="ddlOntvangst" runat="server" MarkFirstMatch="true" EmptyMessage="Kies Ontvangst..."
                                                         Width="170px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Projectlocatie" Value="Projectlocatie" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Vestiging" Value="Vestiging" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    Bedrag loondeel in &euro;:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtBedragLoondeelIn" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                            <div class="Company_list-row">
                                <div class="Company_list1-item">
                                    G-rekening %:
                                </div>
                                <div class="Company_list1-input">
                                    <asp:TextBox ID="txtGrekening" runat="server" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </telerik:RadPageView><telerik:RadPageView ID="rpvItems" runat="server" Visible="False">
                </telerik:RadPageView><telerik:RadPageView ID="rpvCalculatie" runat="server" Visible="False">
                </telerik:RadPageView><telerik:RadPageView ID="rpvFacturatie" runat="server" Visible="False">
                </telerik:RadPageView><telerik:RadPageView ID="rpvDocumenten" runat="server" Visible="False">
                </telerik:RadPageView><telerik:RadPageView ID="rpvManagementInfo" runat="server" Selected="True">
                </telerik:RadPageView></telerik:RadMultiPage>
        </div>
    </asp:Panel>
</asp:Panel>

<% if (!string.IsNullOrEmpty(sDownloadUrl))
   { %>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () { window.location = "<%= sDownloadUrl %>"; }, 750);
        });
    </script>
<% } %>