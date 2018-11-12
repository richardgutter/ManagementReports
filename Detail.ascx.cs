#region Directives

using System;
using System.Collections.Generic;
using System.Linq;
using Ncdo.Company.Data.Entiteiten;
using Ncdo.Company.Framework;
using Ncdo.Company.Framework.Security;
using Telerik.Web.UI;
using System.Globalization;
using Klant_Contacten = Ncdo.Company.Data.Functies.Klant_Contacten;
using Klant_Locaties = Ncdo.Company.Data.Functies.Klant_Locaties;
using Medewerkers = Ncdo.Company.Data.Functies.Medewerkers;
using Plannen = Ncdo.Company.Data.Functies.Plannen;

#endregion

namespace Ncdo.Company.WebInterface.Management_Rapportage
{
    public partial class Detail : CompanyModuleBase
    {
        public string OverzichtUrl { get; set; }
        public int ProjectId { get; set; }
        public string sDownloadUrl { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int Medewerker_Id;
            int Medewerker_Role;
            if (!Authentication.IsAuthenticated(out Medewerker_Id, out Medewerker_Role))
                Response.Redirect(GetUrl("Page_Account_Login") + "?ReturnUrl=" +
                                  Server.UrlEncode(Request.Url.ToString()));

            if (
                !(Medewerker_Role == (int) ApplicationDefinitions.MedewerkersRollen.Werkvoorbereiding ||
                  Medewerker_Role == (int) ApplicationDefinitions.MedewerkersRollen.Management))
            {
                Notifications.Error("U mag deze pagina niet bekijken.", this);
                pnlDetail.Visible = false;
                return;
            }

            try
            {
                OverzichtUrl = GetUrl("Page_ManagementItemRapportage");

                if (!Page.IsPostBack)
                    LoadData();

                if (Request.QueryString["i"] != null)
                {
                    string sItems = "";
                    string[] arrItems = Request.QueryString["i"].Split(';');

                    foreach (string item in arrItems)
                    {
                        Data.Entiteiten.Items i = Data.Functies.Items.GetById(Convert.ToInt32(item));
                        if (i != null)
                        {
                            if (string.IsNullOrEmpty(i.Project_Id.ToString()))
                            {
                                if (string.IsNullOrEmpty(sItems))
                                    sItems += i.Item_Identifier;
                                else
                                    sItems += ", " + i.Item_Identifier;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sItems))
                        Notifications.Warning("De volgende items worden aan dit project gekoppeld: " + sItems, this);

                    rtsKlant.SelectedIndex = 4;
                }

                if (Request["PD"] != null && Convert.ToInt32(Request["PD"]) > 0)
                {
                    sDownloadUrl =
                        ConvertRelativeUrlToAbsoluteUrl("/DesktopModules/Company/Beheer/Project/ProjectPDF.aspx");
                    sDownloadUrl += "?pd=" + Request["PD"] + "&se=true";
                }
            }
            catch (Exception ex)
            {
                Notifications.Error(ex.Message, this);
            }
        }
/*
        protected void rlbKlant_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            List<Klanten> k = Data.Functies.Klanten.GetFiltered(e.Text);

            IEnumerable<Klanten> kFiltered = k.Skip(e.NumberOfItems).Take(25);
            rlbKlant.DataSource = kFiltered;
            rlbKlant.DataBind();

            int endOffset = e.NumberOfItems + kFiltered.Count();
            int totalCount = k.Count();

            if (endOffset == totalCount)
                e.EndOfItems = true;

            e.Message = String.Format("Klanten 1-{0} van de {1}", endOffset, totalCount);
        }
*/
        private IEnumerable<T> MakeMeEnumerable<T>(T Entity)
        {
            yield return Entity;
        }

        /*        private void LoadData()
                {
                    int ProjectId = 0;

                    try
                    {
                        ProjectId = Convert.ToInt32(Request.QueryString["p"]);
                    }
                    catch
                    {
                    }

                    if (ProjectId > 0)
                    {
                        rpvDocumenten.ContentUrl = GetUrl("Page_Portaal_Projecten_Documenten") + "?p=" + ProjectId;
                        rpvItems.ContentUrl = GetUrl("Page_Portaal_Projecten_Items") + "?p=" + ProjectId;
                        rpvCalculatie.ContentUrl = GetUrl("Page_Portaal_Projecten_Calculatie") + "?p=" + ProjectId;

                        //Inladen Project gegevens
                        Data.Entiteiten.Projecten p = Data.Functies.Projecten.GetById(ProjectId);
                        if (p.Klant_Id != customerId)
                        {
                            CustomerAuthentication.LogOff();
                            Response.Redirect(GetUrl("Page_Portaal_Inlog"));
                        }



                        lblKlant.Text = Data.Functies.Klanten.GetById(p.Klant_Id).Bedrijfsnaam;
                        lblProjectNummer.Text = p.ProjectNr;
                        txtOpdrachtnummer.Text = p.OpdrachtNummer;
                        txtOmschrijving.Text = p.Omschrijving;
                        lblStatus.Text = Enum.GetName(typeof(ApplicationDefinitions.Statussen), p.Status).Replace("_", " ");

                        try
                        {
                            lblLocatie.Text = p.Locatie_Id != null ? Klant_Locaties.GetById((int)p.Locatie_Id).Naam : string.Empty;

                            lblTechContact.Text = p.TechnischContact_Id.ToString();

                            if (p.TechnischContact_Id > 0)
                            {
                                lblTechContact.Text = String.IsNullOrEmpty(Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Tussenvoegsels) ?
                                    Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Achternaam :
                                    Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Tussenvoegsels +
                                    " " + Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Achternaam;
                            }
                            else
                                lblTechContact.Text = "-";


                            if (p.AdministratiefContact_Id > 0)
                            {
                                lblAdminContact.Text = String.IsNullOrEmpty(Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Tussenvoegsels) ?
                                    Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Achternaam :
                                    Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Tussenvoegsels +
                                    " " + Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Achternaam;
                            }
                            else
                                lblAdminContact.Text = "-";

                           // lblOpzichter.Text = p.Opzichter > 0 ? p.Opzichter.ToString() : "-";
                        }
                        catch (Exception ex)
                        {
                            Notifications.Error(ex.Message, this);
                        }

                        lblUitvoerder.Text = p.Uitvoerder > 0 ? Medewerkers.GetById((int)p.Uitvoerder).WeergaveNaam : "-";
                        lblProjectleider.Text = p.Projectleider > 0 ? Medewerkers.GetById((int)p.Projectleider).WeergaveNaam : "-";
                        lblWerkvoorbereider.Text = p.Werkvoorbereider > 0 ? Medewerkers.GetById((int)p.Werkvoorbereider).WeergaveNaam : "-";
                        txtOpdrachtgeverklantnummer.Text = !string.IsNullOrEmpty(p.OpdrachtgeverKlantnummer) ? p.OpdrachtgeverKlantnummer : "-";
                        txtAanneemsom.Text = p.Aanneemsom;
                        lblOpdrachtdatum.Text = p.Opdrachtdatum.ToString();
                        lblSoortWerk.Text = p.Soortwerk;
         lblWerkvoorbereider.Text = p.Werkvoorbereider > 0 ? Medewerkers.GetById((int)p.Werkvoorbereider).WeergaveNaam : "-";
                       

           
                        txtKopierProject.Text = !string.IsNullOrEmpty(p.KopierProject) ? p.KopierProject : "-";
                        txtTrajectofferte.Text = !string.IsNullOrEmpty(p.Trajectofferte) ? p.Trajectofferte : "-";
                        txtCalculatienummer.Text = !string.IsNullOrEmpty(p.CalculatieNummer) ? p.CalculatieNummer : "-";
                        txtOverzettencalculatie.Text = !string.IsNullOrEmpty(p.OverzettenCalculatie) ? p.OverzettenCalculatie : "-";
                        lblWerkbegroting.Text = !string.IsNullOrEmpty(p.WerkbegrotingBevroren) ? p.WerkbegrotingBevroren : "-";
                        txtCalculator.Text = !string.IsNullOrEmpty(p.Calculator) ? p.Calculator : "-";

                        txtInkoper.Text = !string.IsNullOrEmpty(p.Inkoper) ? p.Inkoper : "-";
                        lblVerrekenmethode.Text = p.Verrekenmethode;
                        lblContractsoort.Text = p.Contractsoort;
                        lblBoekenOpElementVerplicht.Text = p.BoekenOpElementVerplicht;
                        txtRegieDebiteur.Text = !string.IsNullOrEmpty(p.RegieDebiteur) ? p.RegieDebiteur : "-";
                        lblBeginDatum.Text = p.Begindatum != null ? ((DateTime)p.Begindatum).ToString("dd-MM-yyyy", new CultureInfo("nl-NL")) : "";
                        lblEindDatum.Text = p.Einddatum != null ? ((DateTime)p.Einddatum).ToString("dd-MM-yyyy", new CultureInfo("nl-NL")) : "";
                        lblOntvangst.Text = p.Ontvangst;
                        txtBedragLoondeelIn.Text = !string.IsNullOrEmpty(p.BedragLoondeel) ? p.BedragLoondeel : "-";
                        txtGrekening.Text = !string.IsNullOrEmpty(p.GRekening) ? p.GRekening : "-";


                        txtWerkadrescode.Text = !string.IsNullOrEmpty(p.Werkadrescode) ? p.Werkadrescode : "-";
                        lblProjecttype.Text = p.Projecttype;
                        lblRegio.Text = p.Regio;
                        lblSoort.Text = p.Soort;
                        lblKostenplaatscode.Text = p.Kostenplaatscode;
                        lblEHD.Text = p.EnkelHoofdDeelproject;
                        txtDeelprojectBehorendeBij.Text = !string.IsNullOrEmpty(p.Deelproject) ? p.Deelproject : "-";
                        lblBranche.Text = p.Branche;
                        lblCompetenties.Text = p.Competenties;
                        lblMarktsector.Text = p.Marktsector;
                        lblFunctie.Text = p.Functie;
                        lblTechnicalDiscipline.Text = p.TechnicalDiscipline;
                        lblBusinessSector.Text = !string.IsNullOrEmpty(p.BusinessSector) ? p.BusinessSector : "-";
                        lblPlanning.Text = !string.IsNullOrEmpty(p.Planning) ? p.Planning : "-";

                    }
                    else
                    {
                        rtsKlant.Tabs.Where(t => t.Text == "Items").First().Visible = false;
                        rpvItems.Visible = false;
                        rtsKlant.Tabs.Where(t => t.Text == "Calculatie").First().Visible = false;
                        rpvCalculatie.Visible = false;
                        rtsKlant.Tabs.Where(t => t.Text == "Documenten").First().Visible = false;
                        rpvDocumenten.Visible = false;


                        Data.Entiteiten.Projecten dbproj = Data.Functies.Projecten.GetLatest();

                        int iProjectnr = 6001;

                        if (dbproj != null)
                        {
                            iProjectnr = Convert.ToInt32(dbproj.ProjectNr.Substring(5, 4));
                            iProjectnr += 1;
                        }


                        lblProjectNummer.Text = "1534" + DateTime.Now.ToString("yyyy").Substring(3, 1) + iProjectnr.ToString();


                        if (Request.QueryString["i"] != null)
                        {
                            string sItems = "";
                            string[] arrItems = Request.QueryString["i"].Split(';');
                            bool LoadItemData = false;


                            foreach (string item in arrItems)
                            {
                                Data.Entiteiten.Items i = Data.Functies.Items.GetById(Convert.ToInt32(item));

                                if (i != null)
                                {
                                    if (!LoadItemData)
                                    {
                                        IEnumerable<Klanten> inumK = MakeMeEnumerable(Data.Functies.Klanten.GetById(i.Klant_Id));

                                        LoadItemData = true;
                                    }

                                    if (string.IsNullOrEmpty(i.Project_Id.ToString()))
                                    {
                                        if (string.IsNullOrEmpty(sItems))
                                            sItems += i.Item_Identifier;
                                        else
                                            sItems += ", " + i.Item_Identifier;
                                    }
                                }
                            }
                        }
                    }
                }

        */
        private void LoadData()
        {
            rcbUitvoerder.DataSource =
                Medewerkers.GetByRol((int) ApplicationDefinitions.MedewerkersRollen.Voorman).OrderBy(m => m.WeergaveNaam);
            rcbUitvoerder.DataBind();
            rcbProjectleider.DataSource =
                Medewerkers.GetByRol((int) ApplicationDefinitions.MedewerkersRollen.Management).OrderBy(
                    m => m.WeergaveNaam);
            rcbProjectleider.DataBind();
          //  rcbWerkvoorbereider.DataSource =
           //     Medewerkers.GetByRol((int) ApplicationDefinitions.MedewerkersRollen.Werkvoorbereiding);
           // rcbWerkvoorbereider.DataBind();

            int ProjectId = 0;

            try
            {
                ProjectId = Convert.ToInt32(Request.QueryString["p"]);
            }
            catch
            {
            }

            if (ProjectId > 0)
            {
                // btnPrint.Visible = true;

                rpvDocumenten.ContentUrl = GetUrl("Page_Beheer_Project_Documenten") + "?p=" + ProjectId;
                rpvItems.ContentUrl = GetUrl("Page_Beheer_Project_Items") + "?p=" + ProjectId;
                rpvCalculatie.ContentUrl = GetUrl("Page_Beheer_Project_Calculatie") + "?p=" + ProjectId;
                rpvFacturatie.ContentUrl = GetUrl("Page_Beheer_Project_Facturatie") + "?p=" + ProjectId;
                rpvManagementInfo.ContentUrl = GetUrl("Page_ManagementItemRapportage_ProjectRapportage") + "?p=" + ProjectId;

                //Inladen Project gegevens
                Data.Entiteiten.Projecten p = Data.Functies.Projecten.GetById(ProjectId);

             //   IEnumerable<Klanten> inumK = MakeMeEnumerable(Data.Functies.Klanten.GetById(p.Klant_Id));
             //   rlbKlant.DataSource = inumK;
             //   rlbKlant.DataBind();

             //   rlbKlant.SelectedValue = p.Klant_Id.ToString();
                lblKlant.Text = Data.Functies.Klanten.GetById(p.Klant_Id).Bedrijfsnaam;
                lblProjectNummer.Text = p.ProjectNr;
                txtOpdrachtnummer.Text = p.OpdrachtNummer;
                txtOmschrijving.Text = p.Omschrijving;
             //   rlbStatus.SelectedValue = p.Status.ToString();
                lblStatus.Text = Enum.GetName(typeof(ApplicationDefinitions.Statussen), p.Status).Replace("_", " ");
                lblOpdrachtdatum.Text = p.Opdrachtdatum.ToString();
                lblSoortWerk.Text = p.Soortwerk;
                lblWerkvoorbereider.Text = p.Werkvoorbereider > 0 ? Medewerkers.GetById((int)p.Werkvoorbereider).WeergaveNaam : "-";

                txtKopierProject.Text = p.KopierProject;
                txtTrajectofferte.Text = p.Trajectofferte;
                txtCalculatienummer.Text = p.CalculatieNummer;
                txtOverzettencalculatie.Text = p.OverzettenCalculatie;
                ddlWerkbegroting.SelectedValue = p.WerkbegrotingBevroren;
                txtCalculator.Text = p.Calculator;
                rcbUitvoerder.SelectedValue = p.Uitvoerder.ToString();
                rcbProjectleider.SelectedValue = p.Projectleider.ToString();
             //   rcbWerkvoorbereider.SelectedValue = p.Werkvoorbereider.ToString();
                txtInkoper.Text = p.Inkoper;
                ddlVerrekenmethode.SelectedValue = p.Verrekenmethode;
                ddlContractsoort.SelectedValue = p.Contractsoort;
                ddlBoekenOpElementVerplicht.SelectedValue = p.BoekenOpElementVerplicht;
                txtRegieDebiteur.Text = p.RegieDebiteur;
                lblBeginDatum.Text = p.Begindatum != null ? ((DateTime)p.Begindatum).ToString("dd-MM-yyyy", new CultureInfo("nl-NL")) : "";
                lblEindDatum.Text = p.Einddatum != null ? ((DateTime)p.Einddatum).ToString("dd-MM-yyyy", new CultureInfo("nl-NL")) : "";
                //rdpBeginDatum.SelectedDate = p.Begindatum;
               // rdpEindDatum.SelectedDate = p.Einddatum;
                ddlOntvangst.SelectedValue = p.Ontvangst;
                txtBedragLoondeelIn.Text = p.BedragLoondeel;
                txtGrekening.Text = p.GRekening;
                txtOpdrachtgeverklantnummer.Text = p.OpdrachtgeverKlantnummer;
                txtAanneemsom.Text = p.Aanneemsom;
           //     try
           //     {
           //         rdpOpdrachtdatum.SelectedDate = Convert.ToDateTime(p.Opdrachtdatum);
           //     }
           //     catch
           //     {
           //         rdpOpdrachtdatum.SelectedDate = DateTime.Now;
           //     }
                txtWerkadrescode.Text = p.Werkadrescode;
                ddlProjecttype.SelectedValue = p.Projecttype;
                ddlRegio.SelectedValue = p.Regio;
                ddlSoort.SelectedValue = p.Soort;
                ddlKostenplaatscode.SelectedValue = p.Kostenplaatscode;
                ddlEHD.SelectedValue = p.EnkelHoofdDeelproject;
                txtDeelprojectBehorendeBij.Text = p.Deelproject;
                ddlBranche.SelectedValue = p.Branche;
                ddlCompetenties.SelectedValue = p.Competenties;
               // ddlSoortWerk.SelectedValue = p.Soortwerk;
                ddlMarktsector.SelectedValue = p.Marktsector;
                ddlFunctie.SelectedValue = p.Functie;
                ddlTechnicalDiscipline.SelectedValue = p.TechnicalDiscipline;
                ddlBusinessSector.SelectedValue = p.BusinessSector;
                ddlPlanning.SelectedValue = p.Planning;

                try
                {
                    lblLocatie.Text = p.Locatie_Id != null ? Klant_Locaties.GetById((int)p.Locatie_Id).Naam : string.Empty;

                    lblTechContact.Text = p.TechnischContact_Id.ToString();

                    if (p.TechnischContact_Id > 0)
                    {
                        lblTechContact.Text = String.IsNullOrEmpty(Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Tussenvoegsels) ?
                            Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Achternaam :
                            Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Tussenvoegsels +
                            " " + Klant_Contacten.ContactGetById((int)p.TechnischContact_Id).Achternaam;
                    }
                    else
                        lblTechContact.Text = "-";


                    if (p.AdministratiefContact_Id > 0)
                    {
                        lblAdminContact.Text = String.IsNullOrEmpty(Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Tussenvoegsels) ?
                            Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Achternaam :
                            Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Voornaam + " " + Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Tussenvoegsels +
                            " " + Klant_Contacten.ContactGetById((int)p.AdministratiefContact_Id).Achternaam;
                    }
                    else
                        lblAdminContact.Text = "-";

                    //lblOpzichter.Text = p.Opzichter > 0 ? p.Opzichter.ToString() : "-";
                }
                catch (Exception ex)
                {
                    Notifications.Error(ex.Message, this);
                }
                //                if (Convert.ToDateTime(p.Project_Ingedient) > DateTime.MinValue)
//                    lblProjectIngediend.Text = "Datum ingediend: " +
//                                               Convert.ToDateTime(p.Project_Ingedient).ToString("dd-MM-yyyy");
//                else
//                    lblProjectIngediend.Text = "Nog niet ingediend";

//                try
//                {
//                    rlbTechContact.Enabled = true;
//                    rlbTechContact.EmptyMessage = "Selecteer contact...";
//                    rlbAdminContact.Enabled = true;
//                    rlbAdminContact.EmptyMessage = "Selecteer contact...";
//                    rcbOpzichter.Enabled = true;
//                    rcbOpzichter.EmptyMessage = "Selecteer contact...";

//                    rlbLocatie.DataSource = Klant_Locaties.GetAllByKlantId(p.Klant_Id);
//                    rlbLocatie.DataBind();
//                    rlbLocatie.SelectedValue = p.Locatie_Id.ToString();

//                    rlbTechContact.DataSource = Klant_Contacten.GetAllByLocatieId((int)p.Locatie_Id);
//                    rlbTechContact.DataBind();

//                    rlbAdminContact.DataSource = Klant_Contacten.GetAllByLocatieId((int)p.Locatie_Id);
//                    rlbAdminContact.DataBind();

//                    rcbOpzichter.DataSource = Klant_Contacten.GetAllByLocatieId((int)p.Locatie_Id);
//                    rcbOpzichter.DataBind();

//                    rlbTechContact.SelectedValue = p.TechnischContact_Id.ToString();
//                    rlbAdminContact.SelectedValue = p.AdministratiefContact_Id.ToString();
//                    rcbOpzichter.SelectedValue = p.Opzichter.ToString();
//                }
//                catch (Exception ex)
//                {
//                    Notifications.Error(ex.Message, this);
//                }

            }
            else
            {
                rtsKlant.Tabs.Where(t => t.Text == "Items").First().Visible = false;
                rpvItems.Visible = false;
                rtsKlant.Tabs.Where(t => t.Text == "Calculatie").First().Visible = false;
                rpvCalculatie.Visible = false;
                rtsKlant.Tabs.Where(t => t.Text == "Facturatie").First().Visible = false;
                rpvFacturatie.Visible = false;
                rtsKlant.Tabs.Where(t => t.Text == "Documenten").First().Visible = false;
                rpvDocumenten.Visible = false;


                Data.Entiteiten.Projecten dbproj = Data.Functies.Projecten.GetLatest();

                int iProjectnr = 6001;

                if (dbproj != null)
                {
                    iProjectnr = Convert.ToInt32(dbproj.ProjectNr.Substring(5, 4));
                    iProjectnr += 1;
                }


                lblProjectNummer.Text = "1534" + DateTime.Now.ToString("yyyy").Substring(3, 1) + iProjectnr.ToString();


                if (Request.QueryString["i"] != null)
                {
                    string sItems = "";
                    string[] arrItems = Request.QueryString["i"].Split(';');
                  //  bool LoadItemData = false;


                    foreach (string item in arrItems)
                    {
                        Data.Entiteiten.Items i = Data.Functies.Items.GetById(Convert.ToInt32(item));

                        if (i != null)
                        {
                            //if (!LoadItemData)
                            //{
                            //    IEnumerable<Klanten> inumK = MakeMeEnumerable(Data.Functies.Klanten.GetById(i.Klant_Id));
                            //    rlbKlant.DataSource = inumK;
                            //    rlbKlant.DataBind();

                            //    rlbKlant.SelectedValue = i.Klant_Id.ToString();

                            //    rlbLocatie.DataSource = Klant_Locaties.GetAllByKlantId(i.Klant_Id);
                            //    rlbLocatie.DataBind();

                            //    LoadItemData = true;
                            //}
                            //
                            if (string.IsNullOrEmpty(i.Project_Id.ToString()))
                            {
                                if (string.IsNullOrEmpty(sItems))
                                    sItems += i.Item_Identifier;
                                else
                                    sItems += ", " + i.Item_Identifier;
                            }
                        }
                    }
                }
            }
        }
/*
        protected void rlbKlant_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rlbLocatie.ClearSelection();
            rlbTechContact.ClearSelection();
            rlbAdminContact.ClearSelection();
            rcbOpzichter.ClearSelection();

            rlbTechContact.Enabled = false;
            rlbTechContact.EmptyMessage = "Seleteer eerst een locatie.";
            rlbAdminContact.Enabled = false;
            rlbAdminContact.EmptyMessage = "Seleteer eerst een locatie.";
            rcbOpzichter.Enabled = false;
            rcbOpzichter.EmptyMessage = "Seleteer eerst een locatie.";

            string sKlantId = e.Value;
            if (!string.IsNullOrEmpty(sKlantId) && Convert.ToInt32(sKlantId) > 0)
            {
                rlbLocatie.DataSource = Klant_Locaties.GetAllByKlantId(Convert.ToInt32(sKlantId));
                rlbLocatie.DataBind();
            }
        }

        protected void rlbLocatie_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rlbTechContact.ClearSelection();
            rlbAdminContact.ClearSelection();
            rcbOpzichter.ClearSelection();

            rlbTechContact.Enabled = true;
            rlbTechContact.EmptyMessage = "Selecteer contact...";
            rlbAdminContact.Enabled = true;
            rlbAdminContact.EmptyMessage = "Selecteer contact...";
            rcbOpzichter.Enabled = true;
            rcbOpzichter.EmptyMessage = "Selecteer contact...";

            string sLocatieId = e.Value;
            if (!string.IsNullOrEmpty(sLocatieId) && Convert.ToInt32(sLocatieId) > 0)
            {
                List<Klant_Contacten.ContactCombobox> contacten =
                    Klant_Contacten.GetAllByLocatieId(Convert.ToInt32(sLocatieId));

                rlbTechContact.DataSource = contacten;
                rlbTechContact.DataBind();

                rlbAdminContact.DataSource = contacten;
                rlbAdminContact.DataBind();

                rcbOpzichter.DataSource = contacten;
                rcbOpzichter.DataBind();
            }
        }

        protected void SaveProject(bool Redirect)
        {
            try
            {
                ProjectId = 0;

                try
                {
                    ProjectId = Convert.ToInt32(Request.QueryString["p"]);
                }
                catch
                {
                }

                int KlantId = 0;

                try
                {
                    KlantId = Convert.ToInt32(rlbKlant.SelectedValue);
                }
                catch
                {
                    Notifications.Error("Selecteer een klant", this);
                    return;
                }

                int LocatieId = 0;

                try
                {
                    LocatieId = Convert.ToInt32(rlbLocatie.SelectedValue);
                }
                catch
                {
                    Notifications.Error("Selecteer een locatie", this);
                    return;
                }

                int TechContactId = 0;

                try
                {
                    TechContactId = Convert.ToInt32(rlbTechContact.SelectedValue);
                }
                catch
                {
                    Notifications.Error("Selecteer een technisch contactpersoon", this);
                    return;
                }

                int AdminContactId = 0;

                try
                {
                    AdminContactId = Convert.ToInt32(rlbAdminContact.SelectedValue);
                }
                catch
                {
                    Notifications.Error("Selecteer een administratief contactpersoon", this);
                    return;
                }

                int StatusId = 0;

                try
                {
                    StatusId = Convert.ToInt32(rlbStatus.SelectedValue);
                }
                catch
                {
                    Notifications.Error("Selecteer een status", this);
                    return;
                }

                if (ProjectId > 0)
                {
                    int iStatus = -1;
                    int.TryParse(rlbStatus.SelectedValue, out iStatus);
                    int iUitvoerder = -1;
                    int.TryParse(rcbUitvoerder.SelectedValue, out iUitvoerder);
                    int iProjectLeider = -1;
                    int.TryParse(rcbProjectleider.SelectedValue, out iProjectLeider);
                    int iWerkVoorbereider = -1;
                    int.TryParse(rcbWerkvoorbereider.SelectedValue, out iWerkVoorbereider);
                    int iOpzichter = -1;
                    int.TryParse(rcbOpzichter.SelectedValue, out iOpzichter);

                    if (iStatus == 2)
                    {
                        //Kijken of alle items op voltooid staan
                        IEnumerable<Data.Entiteiten.Items> li = Data.Functies.Items.GetByProject(ProjectId);
                        foreach (Data.Entiteiten.Items i in li)
                        {
                            int PlanId = Plannen.GetCurrentStapByItem(i.Item_Id);
                            if (PlanId != (int) ApplicationDefinitions.Stappen.Compleet)
                            {
                                Notifications.Error("Item " + i.Item_Identifier + " is nog niet voltooid", this);
                                return;
                            }
                        }
                    }

                    //Klant wijzigen
                    Data.Functies.Projecten.Update(ProjectId,
                                                   KlantId,
                                                   LocatieId,
                                                   lblProjectNummer.Text,
                                                   txtOpdrachtnummer.Text,
                                                   txtOmschrijving.Text, StatusId,
                                                   TechContactId,
                                                   AdminContactId,
                                                   txtKopierProject.Text,
                                                   txtTrajectofferte.Text,
                                                   txtCalculatienummer.Text,
                                                   txtOverzettencalculatie.Text,
                                                   ddlWerkbegroting.SelectedValue,
                                                   txtCalculator.Text,
                                                   iUitvoerder,
                                                   iProjectLeider,
                                                   iWerkVoorbereider,
                                                   txtInkoper.Text,
                                                   iOpzichter,
                                                   ddlVerrekenmethode.SelectedValue,
                                                   ddlContractsoort.SelectedValue,
                                                   ddlBoekenOpElementVerplicht.SelectedValue,
                                                   txtRegieDebiteur.Text,
                                                   (DateTime) rdpBeginDatum.SelectedDate,
                                                   (DateTime) rdpEindDatum.SelectedDate,
                                                   ddlOntvangst.SelectedValue,
                                                   txtBedragLoondeelIn.Text,
                                                   txtGrekening.Text,
                                                   txtOpdrachtgeverklantnummer.Text,
                                                   txtAanneemsom.Text,
                                                   string.Format("{0:dd-MM-yyyy}", rdpOpdrachtdatum.SelectedDate),
                                                   txtWerkadrescode.Text,
                                                   ddlProjecttype.SelectedValue,
                                                   ddlRegio.SelectedValue,
                                                   ddlSoort.SelectedValue,
                                                   ddlKostenplaatscode.SelectedValue,
                                                   ddlEHD.SelectedValue,
                                                   txtDeelprojectBehorendeBij.Text,
                                                   ddlBranche.SelectedValue,
                                                   ddlCompetenties.SelectedValue,
                                                   ddlSoortWerk.SelectedValue,
                                                   ddlMarktsector.SelectedValue,
                                                   ddlFunctie.SelectedValue,
                                                   ddlTechnicalDiscipline.SelectedValue,
                                                   ddlBusinessSector.SelectedValue,
                                                   ddlPlanning.SelectedValue);

                   

                    if (Redirect)
                        Response.Redirect(OverzichtUrl + "?NotSu=project%20succesvol%20aangepast&p=" + ProjectId.ToString() +
                                          "&PD=" + ProjectId.ToString());
                    //else
                    //     Response.Redirect(GetUrl("Page_Beheer_Project_Bewerken") + "?NotSu=project%20succesvol%20aangepast&p=" + ProjectId.ToString() +
                    //                      "&PD=" + ProjectId.ToString());
                }
                else
                {
                    Data.Entiteiten.Projecten dbproj = Data.Functies.Projecten.GetLatest();

                    int iProjectnr = 6001;

                    if (dbproj != null)
                    {
                        iProjectnr = Convert.ToInt32(dbproj.ProjectNr.Substring(5, 4));
                        iProjectnr += 1;
                    }


                    string sProjectNumber = "1534" + DateTime.Now.ToString("yyyy").Substring(3, 1) +
                                            iProjectnr.ToString();

                    int iUitvoerder = -1;
                    int.TryParse(rcbUitvoerder.SelectedValue, out iUitvoerder);
                    int iProjectLeider = -1;
                    int.TryParse(rcbProjectleider.SelectedValue, out iProjectLeider);
                    int iWerkVoorbereider = -1;
                    int.TryParse(rcbWerkvoorbereider.SelectedValue, out iWerkVoorbereider);
                    int iOpzichter = -1;
                    int.TryParse(rcbOpzichter.SelectedValue, out iOpzichter);

                    //Project toevoegen
                    Data.Entiteiten.Projecten p = Data.Functies.Projecten.Add(KlantId,
                                                                              LocatieId,
                                                                              sProjectNumber,
                                                                              txtOpdrachtnummer.Text,
                                                                              txtOmschrijving.Text,
                                                                              StatusId,
                                                                              TechContactId,
                                                                              AdminContactId,
                                                                              txtKopierProject.Text,
                                                                              txtTrajectofferte.Text,
                                                                              txtCalculatienummer.Text,
                                                                              txtOverzettencalculatie.Text,
                                                                              ddlWerkbegroting.SelectedValue,
                                                                              txtCalculator.Text,
                                                                              iUitvoerder,
                                                                              iProjectLeider,
                                                                              iWerkVoorbereider,
                                                                              txtInkoper.Text,
                                                                              iOpzichter,
                                                                              ddlVerrekenmethode.SelectedValue,
                                                                              ddlContractsoort.SelectedValue,
                                                                              ddlBoekenOpElementVerplicht.SelectedValue,
                                                                              txtRegieDebiteur.Text,
                                                                              (DateTime) rdpBeginDatum.SelectedDate,
                                                                              (DateTime) rdpEindDatum.SelectedDate,
                                                                              ddlOntvangst.SelectedValue,
                                                                              txtBedragLoondeelIn.Text,
                                                                              txtGrekening.Text,
                                                                              txtOpdrachtgeverklantnummer.Text,
                                                                              txtAanneemsom.Text,
                                                                              string.Format("{0:dd-MM-yyyy}",
                                                                                            rdpOpdrachtdatum.
                                                                                                SelectedDate),
                                                                              txtWerkadrescode.Text,
                                                                              ddlProjecttype.SelectedValue,
                                                                              ddlRegio.SelectedValue,
                                                                              ddlSoort.SelectedValue,
                                                                              ddlKostenplaatscode.SelectedValue,
                                                                              ddlEHD.SelectedValue,
                                                                              txtDeelprojectBehorendeBij.Text,
                                                                              ddlBranche.SelectedValue,
                                                                              ddlCompetenties.SelectedValue,
                                                                              ddlSoortWerk.SelectedValue,
                                                                              ddlMarktsector.SelectedValue,
                                                                              ddlFunctie.SelectedValue,
                                                                              ddlTechnicalDiscipline.SelectedValue,
                                                                              ddlBusinessSector.SelectedValue,
                                                                              ddlPlanning.SelectedValue);

                    if (p != null && Request.QueryString["i"] != null)
                    {
                        ProjectId = p.Project_Id;
                        string[] arrItems = Request.QueryString["i"].Split(';');

                        foreach (string item in arrItems)
                        {
                            Data.Entiteiten.Items i = Data.Functies.Items.GetById(Convert.ToInt32(item));

                            if (i != null)
                            {
                                int Klant_Id = -1;
                                if (!string.IsNullOrEmpty(p.Klant_Id.ToString()) &&
                                    string.IsNullOrEmpty(i.Klant_Id.ToString()))
                                    int.TryParse(p.Klant_Id.ToString(), out Klant_Id);
                                else
                                    int.TryParse(i.Klant_Id.ToString(), out Klant_Id);

                                int Locatie_Id = -1;
                                if (!string.IsNullOrEmpty(p.Locatie_Id.ToString()) &&
                                    string.IsNullOrEmpty(i.Locatie_Id.ToString()))
                                    int.TryParse(p.Locatie_Id.ToString(), out Locatie_Id);
                                else
                                    int.TryParse(i.Locatie_Id.ToString(), out Locatie_Id);

                                int Contact_Id = -1;
                                if (!string.IsNullOrEmpty(p.TechnischContact_Id.ToString()) &&
                                    string.IsNullOrEmpty(i.Contact_Id.ToString()))
                                    int.TryParse(p.TechnischContact_Id.ToString(), out Contact_Id);
                                else
                                    int.TryParse(i.Contact_Id.ToString(), out Contact_Id);

                                int Wvb_Id = -1;
                                if (!string.IsNullOrEmpty(p.Werkvoorbereider.ToString()) &&
                                    string.IsNullOrEmpty(i.WvB_Id.ToString()))
                                    int.TryParse(p.Werkvoorbereider.ToString(), out Wvb_Id);
                                else
                                    int.TryParse(i.WvB_Id.ToString(), out Wvb_Id);

                                if (string.IsNullOrEmpty(i.Klachtomschrijving))
                                    i.Klachtomschrijving =
                                        Enum.GetName(typeof (ApplicationDefinitions.Statussen), p.Status).Replace("_",
                                                                                                                  " ") +
                                        " " + p.Omschrijving;

                                if (string.IsNullOrEmpty(i.Project_Id.ToString()))
                                    Data.Functies.Items.Update(i.Item_Id, i.Item_Soort_Id, Klant_Id, Locatie_Id,
                                                               Contact_Id, Wvb_Id, i.Klachtomschrijving, p.Project_Id);
                            }
                        }

                        Data.Functies.Projecten.Update(p.Project_Id, DateTime.Now);
                        Response.Redirect(GetUrl("Page_Beheer_Project_Bewerken") + "?p=" + p.Project_Id.ToString() +
                                          "&PD=" + p.Project_Id.ToString() + "&se=true");

                        //if (Redirect)
                        //    Response.Redirect(GetUrl("Page_Beheer_Item") + "?NotSu=project%20succesvol%20aangemaakt");
                        //else
                        //    Response.Redirect(GetUrl("Page_Beheer_Item") + "?NotSu=project%20succesvol%20aangemaakt&PD=" + p.Project_Id);
                    }
                    else if (p != null)
                    {
                        Data.Functies.Projecten.Update(p.Project_Id, DateTime.Now);
                        Response.Redirect(GetUrl("Page_Beheer_Project_Bewerken") + "?p=" + p.Project_Id.ToString() +
                                          "&PD=" + p.Project_Id.ToString() + "&se=true");

                        //if (Redirect)
                        //    Response.Redirect(OverzichtUrl + "?NotSu=project%20succesvol%20aangemaakt");
                        //else
                        //    Response.Redirect(OverzichtUrl + "?NotSu=project%20succesvol%20aangemaakt&PD=" + p.Project_Id);
                    }
                    else
                    {
                        Notifications.Error("Project niet aangemaakt.", this);
                    }
                }
            }
            catch (Exception ex)
            {
                Notifications.Error(ex.Message, this);
            }
        }

        protected void btnOpslaan_Click(object sender, EventArgs e)
        {
            SaveProject(true);
        }
*/
        protected void btnAnnuleren_Click(object sender, EventArgs e)
        {
            Response.Redirect(OverzichtUrl);
        }
        
        public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
        {
            if (Request.IsSecureConnection)
                return string.Format("https://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

            return string.Format("http://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));
        }

 /*        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ProjectId = 0;

            try
            {
                ProjectId = Convert.ToInt32(Request.QueryString["p"]);
            }
            catch
            {
            }

            SaveProject(false);

            string sDownloadUrl = ConvertRelativeUrlToAbsoluteUrl("/DesktopModules/Company/Beheer/Project/ProjectPDF.aspx");
            sDownloadUrl += "?pd=" + ProjectId + "&se=false";

            Response.Redirect(sDownloadUrl);
        }
*/

    }
}