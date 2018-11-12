<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagementRapport.aspx.cs" Inherits="Ncdo.Company.WebInterface.Management_Rapportage.ManagementRapport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: 'Segoe UI', arial, sans-serif;
            font-size: 12px;
            padding: 20px;
        }

            body table
            {
                background-color: #d6d6d6;
                font-size: 9pt;
                width: 100%;
            }

                body table tr td
                {
                    height: 15px;
                    text-align: center;
                }

                body table tr th
                {
                    background-color: #ffffff;
                    height: 100px;
                    text-align: center;
                }

                body table tr.even td
                {
                    background-color: #ffffff;
                }

                body table tr.oneven td
                {
                    background-color: #eeeeee;
                }

        table .vertical_text
        {
        }

        table .vertical_image
        {
            background-position: bottom center;
            background-repeat: no-repeat;
            height: 15px;
            width: 15px;
        }

        table .vertical_ingeboekt
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-ingeboekt.png);
        }

        table .vertical_eersteinspectie
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-eersteinspectie.png);
        }

        table .vertical_pretest
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-pretest.png);
        }

        table .vertical_demontage
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-demonteren.png);
        }

        table .vertical_stralen
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-stralen.png);
        }

        table .vertical_tweedeinspectie
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-tweedeinspectie.png);
        }

        table .vertical_eeni
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-eenl.png);
        }

        table .vertical_conserveren1elaag
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-conserveren1elaag.png);
        }

        table .vertical_machinaal
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-machinaal.png);
        }

        table .vertical_montage
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-montage.png);
        }

        table .vertical_testen
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-testen.png);
        }

        table .vertical_conserveren2elaag
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-conserveren2elaag.png);
        }

        table .vertical_O2clean
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-O2clean.png);
        }

        table .vertical_transportgereedmaken
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-transportgereedmaken.png);
        }

        table .vertical_qualitycontrol
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-qualitycontrol.png);
        }

        table .vertical_transport
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-transport.png);
        }

        table .vertical_werkvoorbereiding
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-werkvoorbereiding.png);
        }

        table .vertical_afmonteren
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-afmonteren.png);
        }

        table .vertical_technischafgerond
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-afgerond.png);
        }

        table .vertical_calculatie
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-calculatie.png);
        }

        table .vertical_compleet
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-compleet.png);
        }

        table .vertical_totminuten
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-totaalinminuten.png);
        }

        table .vertical_toturen
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-totaalinuren.png);
        }

        table .vertical_navision
        {
            background-image: url(http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Item/Images/table-navision.png);
        }

        h2
        {
            color: #333;
            display: inline-block;
            font-family: 'Magra', arial, sans-serif;
            font-size: 14pt;
            font-weight: normal;
            margin: 0px;
/*            margin-bottom: 10px;
            margin-top: 10px;
*/            overflow: hidden;
            text-align: left;
            width: auto;
        }

        a.button-10
        {
            color: #FFFFFF;
            font-size: 10pt;
            text-decoration: none;
            text-transform: Capitalize;
        }

        .button-10, a.button-10
        {
            border: 0px;
            border-radius: 3px;
            color: white;
            font-family: 'Segoe UI', arial, sans-serif;
            font-size: 12px;
            height: auto;
            line-height: 20px;
            margin-right: 5px;
            /*            moz-border-radius: 3px;
            ms-border-radius: 3px;
*/
            padding: 3px;
            text-align: center;
            text-transform: Capitalize;
            /*            webkit-border-radius: 3px;
*/
            width: auto;
        }

            .button-10:hover, a.button-10:hover
            {
                background: #5e5e5e;
                cursor: pointer;
            }

        .rood
        {
            background-color: #c41010 !important;
        }

        .groen
        {
            background-color: #00870d !important;
        }

        .ImageHolder
        {
            display: block;
            float: right;
            padding-bottom: 10px;
            text-align: right;
            width: auto;
        }

        .btnHolder
        {
            float: right;
            text-align: right;
            width: auto;
        }

        .legenda
        {
            float: right;
            margin-bottom: 10px;
            width: 150px;
        }

        .rapport-box
        {
            display: block;
            margin-right: 9px;
            margin-left: 5px;
            /*           width: 460px;
 */
        }

        .rapport-kolom
        {
            width: 230px;
            float: left;
            border: 1px solid #d6d6d6;
            padding: 1px 5px 1px 5px;
            margin: 10px 10px 10px 0px;
        }

        .rapport-kolom-groot
        {
            width: 350px;
            float: left;
            border: 1px solid #d6d6d6;
            padding: 1px 5px 1px 5px;
            margin: 10px 10px 10px 0px;
        }

        .rapport-kolom-project
        {
            width: 500px;
            float: left;
        }

        .rapport li
        {
            list-style-type: none;
        }

        .rapport-row
        {
            clear: both;
            height: auto;
            display: block;
            float: left;
            width: 230px;
        }

        .rapport-row-groot
        {
            clear: both;
            height: auto;
            display: block;
            float: left;
            width: 350px;
        }

        .rapport-row-project
        {
            clear: both;
            height: auto;
            display: block;
            float: left;
            width: 500px;
        }

        .rapport-item
        {
            float: left;
            height: 27px;
            line-height: 27px;
            width: 100px;
            font-size: 12px;
            margin-right: 10px;
            text-align: left;
        }

        .rapport-uren-item
        {
            float: left;
            height: 27px;
            line-height: 27px;
            width: 75px;
            font-size: 12px;
            margin-right: 10px;
            text-align: left;
        }

        .rapport-item-project
        {
            float: left;
            height: 27px;
            line-height: 27px;
            width: 100px;
            font-size: 12px;
            margin-right: 10px;
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2 style="padding-top: 1px;">Item management rapportage</h2>
        <asp:Panel ID="pnlImage" runat="server" Visible="false" CssClass="ImageHolder">
            <img src="http://Company.Ncdo.nu/DesktopModules/Company/Beheer/Project/Company_logo.png" alt="Company" height="49" width="228" /><br />
        </asp:Panel>
        <asp:Panel ID="pnlButtons" runat="server" CssClass="btnHolder">
            <asp:Button ID="btnToPdf" runat="server" Text="Download Pdf" OnClick="btnToPdf_Click" CssClass="button-10 groen" />
            <input type="button" id="btnClose" onclick=" javascript: window.close(); return false; " class="button-10 rood" value="Venster sluiten" />
        </asp:Panel>

        <div class="clearboth">&nbsp;</div>
        <div class="clearboth">&nbsp;</div>

        <asp:Panel ID="FilterPanel2" runat="server">
            <div class="rapport-kolom">
                <div class="rapport-row">
                    <div class="rapport-item">
                        Item nummer:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextItemNummer" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Projectnummer:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextProjectNummer" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Omschrijving:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextOmschrijving" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="Label13" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        <!--Resultaat:-->
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextResultaat" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        <!--Resultaat:-->
                    </div>
                    <div class="rapport-item">
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                    </div>
                    <div class="rapport-item">
                    </div>
                </div>
            </div>

            <div class="rapport-kolom">
                <div class="rapport-row">
                    <div class="rapport-item">
                        Klant:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextKlant" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Locatie:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextLocatie" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Item Soort:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextItemSoort" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Item Type:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextItemType" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Begindatum:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextBegindatum" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Einddatum:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextEinddatum" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="rapport-kolom">
                <h2>Opbrengsten</h2>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Basis:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextBasisOpbrengsten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Materiaal:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextMateriaalOpbrengsten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Meerwerk:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextMeerwerk" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="leeg1" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="leeg2" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-item" style="font-weight: bold">
                    Totaal:
                </div>
                <div class="rapport-item">
                    <asp:Label ID="TextTotaleOpbrengsten" runat="server" Style="font-weight: bold"></asp:Label>
                </div>
            </div>
            <div class="rapport-kolom">
                <h2>Kosten</h2>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Uren:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextBasisKosten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Materiaal:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextMateriaalKosten" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Materieel:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextMaterieel" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Onderaannemer:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextOnderaannemer" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item">
                        Overige:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextOverige" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row">
                    <div class="rapport-item" style="font-weight: bold">
                        Totaal:
                    </div>
                    <div class="rapport-item">
                        <asp:Label ID="TextTotaleKosten" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="rapport-kolom-groot">
                <h2>Uren</h2>
                <div class="rapport-row-groot">
                    <div class="rapport-uren-item">
                    </div>
                    <div class="rapport-uren-item">
                        Basis
                    </div>
                    <div class="rapport-uren-item">
                        Workfl.
                    </div>
                    <div class="rapport-uren-item">
                        Navis.
                    </div>
                </div>
                <div class="rapport-row-groot">
                    <div class="rapport-uren-item">
                        Toezicht
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextToezichtBasis" runat="server"></asp:Label>
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextToezichtWorkfl" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row-groot">
                    <div class="rapport-uren-item">
                        Wvb
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextWvbBasis" runat="server"></asp:Label>
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextWvbWorkfl" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row-groot">
                    <div class="rapport-uren-item">
                        Directen
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextDirectenBasis" runat="server"></asp:Label>
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextDirectenWorkfl" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row-groot">
                    <div class="rapport-uren-item" style="font-weight: bold">
                        Totaal
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextMinutenBasis" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextMinutenWorkfl" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                    <div class="rapport-uren-item">
                        <asp:Label ID="TextMinutenNavis" runat="server" Style="font-weight: bold"></asp:Label>
                    </div>
                </div>
                <div class="rapport-row-groot">
                    <div class="rapport-uren-item" style="font-weight: bold">
                    </div>
                    <div class="rapport-uren-item">
                    </div>
                    <div class="rapport-uren-item">
                    </div>
                    <div class="rapport-uren-item">
                    </div>
                </div>
            </div>
        </asp:Panel>

        <div class="clearboth">&nbsp;</div>

            <div class="Company_list2-foto">
                <img id="Companydetailbigfoto" />
                <div id="imtectdetailfotoslider" runat="server" class="fotothumbholder">
            
                </div>
            </div>

        <asp:Table ID="ItemOverzicht" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="1">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell CssClass="vertical_text" VerticalAlign="Bottom" Width="90" Style="text-align: left;">Medewerker</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_ingeboekt">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_eersteinspectie">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_werkvoorbereiding">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_pretest">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_demontage">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_stralen">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_tweedeinspectie">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_conserveren1elaag">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_machinaal">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_montage">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_testen">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_eeni">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_O2clean">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_conserveren2elaag">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_afmonteren">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_qualitycontrol">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_transportgereedmaken">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_calculatie">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_totminuten">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_toturen">&nbsp;</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="vertical_image vertical_navision">&nbsp;</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </form>
</body>
</html>
