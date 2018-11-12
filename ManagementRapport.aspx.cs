#region Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EvoPdf.HtmlToPdf;
using EvoPdf.HtmlToPdf.PdfDocument;
using Ncdo.Company.Data.Entiteiten;
using Ncdo.Company.Framework;
using Ncdo.Company.Framework.Security;
using Plannen = Ncdo.Company.Data.Functies.Plannen;
using System.Globalization;
using System.Web.UI.HtmlControls;

#endregion

namespace Ncdo.Company.WebInterface.Management_Rapportage
{
    public partial class ManagementRapport : Page
    {
        int customerId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Encription.SchrijfNaarLogFile("page load", "");
            int Itemnummer = 0;
            int ProjectNummer = 0;

            if (Request["pdf"] != null)
            {
                customerId = Convert.ToInt32(Request["cid"]);
                pnlButtons.Visible = false;
                pnlImage.Visible = true;
            }
            else
            {
                int Medewerker_Id;
                int Medewerker_Role;
                if (!Authentication.IsAuthenticated(out Medewerker_Id, out Medewerker_Role))
                    Response.Redirect(CompanyModuleBase.GetUrl("Page_Account_Login") + "?ReturnUrl=" +
                                      Server.UrlEncode(Request.Url.ToString()));
                //                pnlImage.Visible = false;
                pnlImage.Visible = true;
            }

            try
            {
                Itemnummer = Convert.ToInt32(Request["i"]);
            }
            catch
            {
                Itemnummer = 0;
            }

            try
            {
                ProjectNummer = Convert.ToInt32(Request["p"]);
            }
            catch
            {
                ProjectNummer = 0;
            }

            var _db = new Ncdo.Company.Data.Entiteiten.Entities();

            Ncdo.Company.Data.Entiteiten.Items Item;
            Ncdo.Company.Data.Entiteiten.Projecten Project;
            Ncdo.Company.Data.Entiteiten.Item_Soorten Soort;
            Ncdo.Company.Data.Entiteiten.Item_Soort_Types Type;
            Ncdo.Company.Data.Entiteiten.Klanten Klant;
            Ncdo.Company.Data.Entiteiten.Klant_Locaties Locatie;

            TextKlant.Text = "";
            TextLocatie.Text = "";
            TextItemSoort.Text = "";
            TextItemType.Text = "";

            Item = Ncdo.Company.Data.Functies.Items.GetById(Itemnummer);
            Project = Ncdo.Company.Data.Functies.Projecten.GetById(ProjectNummer);
            TextItemNummer.Text = Item.Item_Identifier;
            TextProjectNummer.Text = Project.ProjectNr;
            TextOmschrijving.Text = Project.Omschrijving;
            TextBegindatum.Text = string.Format("{0:dd-MM-yyyy}", Project.Begindatum);
            TextEinddatum.Text = string.Format("{0:dd-MM-yyyy}", Project.Einddatum);
            TextResultaat.Text = "";

            if (Item.Item_Soort_Id != 0)
            {
                Soort = Ncdo.Company.Data.Functies.Item_Soorten.GetById(Item.Item_Soort_Id);
                TextItemSoort.Text = Soort.Naam;
            }
            if (Item.Item_Type_Id != null)
            {
                Type = Ncdo.Company.Data.Functies.Item_Soort_Types.GetById((int)Item.Item_Type_Id);
                TextItemType.Text = Type.Naam;
            }
            if (Item.Klant_Id != 0)
            {
                Klant = Ncdo.Company.Data.Functies.Klanten.GetById((int)Item.Klant_Id);
                TextKlant.Text = Klant.Bedrijfsnaam;
            }
            if (Item.Locatie_Id != null)
            {
                Locatie = Ncdo.Company.Data.Functies.Klant_Locaties.GetById((int)Item.Locatie_Id);
                TextLocatie.Text = Locatie.Naam;
            }


            int[] ArrayGeplandeTijden = new int[28];
            ArrayGeplandeTijden = OndersteunendeFuncties.GeefGeplandeTijdenArray(Itemnummer);

            var resultBasis = from ic in _db.Item_Calculaties
                               where (ic.Item_Id == Itemnummer) &&
                               (!ic.Meerwerk)
                               group ic by ic.Item_Id into grp
                               select new
                               {
                                   TotBasis = grp.Sum(ic => ic.Totaalbedrag)
                               };

            decimal Basis = 0;

            if (resultBasis.Count() > 0)
                Basis = resultBasis.First().TotBasis;

            TextBasisOpbrengsten.Text = Basis.ToString("C", new CultureInfo("nl-NL"));

            var resultMeerwerk = from ic in _db.Item_Calculaties
                                 where (ic.Item_Id == Itemnummer) &&
                                 (ic.Meerwerk)
                                 group ic by ic.Item_Id into grp
                                 select new
                                 {
                                     TotMeerwerk = grp.Sum(ic => ic.Totaalbedrag)
                                 };
            decimal Meerwerk = 0;

            if (resultMeerwerk.Count() > 0)
                Meerwerk = resultMeerwerk.First().TotMeerwerk;

            TextMeerwerk.Text = Meerwerk.ToString("C", new CultureInfo("nl-NL"));

            var resultMateriaal = from m in _db.Materialen
                                  where (m.Item_Id == Itemnummer)
                                  group m by m.Item_Id into grp
                                  select new
                                  {
                                      TotMateriaal = grp.Sum(p => (p.Aantal * p.Prijs) * (1 + p.Opslag / 100))
                                  };

            decimal MateriaalOpbrengsten = 0;

            if (resultMateriaal.Count() > 0)
                MateriaalOpbrengsten = Convert.ToDecimal(resultMateriaal.First().TotMateriaal);

            TextMateriaalOpbrengsten.Text = MateriaalOpbrengsten.ToString("C", new CultureInfo("nl-NL"));

            decimal dTextTotaleOpbrengsten = Basis + Meerwerk + MateriaalOpbrengsten;
            TextTotaleOpbrengsten.Text = dTextTotaleOpbrengsten.ToString("C", new CultureInfo("nl-NL"));

            var resultKosten = from m in _db.ManagementInfo
                               join p in _db.Projecten
                               on m.Project_id equals p.Project_Id
                               join i in _db.Items
                               on p.Project_Id equals i.Project_Id
                               where (p.Project_Id == ProjectNummer)
                               //where (i.Item_Id == itemId)
                               select new
                              {
                                  Uren = m.AantalUren,
                                  Basis = m.Arbeid,
                                  Materiaal = m.Materiaal,
                                  Materieel = m.Materieel,
                                  Onderaannemer = m.Onderaanneming,
                                  Overige = m.OverigeKosten
                              };

            decimal dBasisKosten = 0;
            decimal dMateriaalKosten = 0;
            decimal dMaterieelKosten = 0;
            decimal dOnderaannemerKosten = 0;
            decimal dOverigeKosten = 0;

            decimal dTotaleKosten = 0;
            decimal dUrenNavis = 0;

            if (resultKosten.Count() > 0)
            {
                dBasisKosten = Convert.ToDecimal(resultKosten.First().Basis);
                dMateriaalKosten = Convert.ToDecimal(resultKosten.First().Materiaal);
                dMaterieelKosten = Convert.ToDecimal(resultKosten.First().Materieel);
                dOnderaannemerKosten = Convert.ToDecimal(resultKosten.First().Onderaannemer);
                dOverigeKosten = Convert.ToDecimal(resultKosten.First().Overige);
                dTotaleKosten = dBasisKosten + dMateriaalKosten + dMaterieelKosten + dOnderaannemerKosten + dOverigeKosten;
                dUrenNavis = Convert.ToDecimal(resultKosten.First().Uren);
            }

            TextBasisKosten.Text = dBasisKosten.ToString("C", new CultureInfo("nl-NL"));
            TextMateriaalKosten.Text = dMateriaalKosten.ToString("C", new CultureInfo("nl-NL"));
            TextMaterieel.Text = dMaterieelKosten.ToString("C", new CultureInfo("nl-NL"));
            TextOnderaannemer.Text = dOnderaannemerKosten.ToString("C", new CultureInfo("nl-NL"));
            TextOverige.Text = dOverigeKosten.ToString("C", new CultureInfo("nl-NL"));

            TextTotaleKosten.Text = dTotaleKosten.ToString("C", new CultureInfo("nl-NL"));
            TextMinutenNavis.Text = (dUrenNavis * 60).ToString();

            var resultMedewerkers = from u in _db.Uitvoeringen
                                    join m in _db.Medewerkers
                                    on u.Medewerker_Id equals m.Medewerker_Id
                                    where u.Item_Id == Itemnummer
                                    group u by
                                    new
                                    {
                                        m.GebruikersNaam,
                                        u.Medewerker_Id
                                        // u.TotaleMinuten
                                    } into grp
                                    select new
                                    {
                                        Medewerker = grp.Key.GebruikersNaam,
                                        MedewerkerId = grp.Key.Medewerker_Id
                                    };

            var tr = new TableRow();

            tr.CssClass = "oneven";
            ItemOverzicht.Rows.Add(tr);

            var tc = new TableCell();
            tc = new TableCell();
            tc.Text = "Gepland";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[0].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[2].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[1].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[3].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[4].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[5].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[6].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[7].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[8].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[9].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[10].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[16].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[17].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[11].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[19].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[13].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[14].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[20].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ArrayGeplandeTijden[24].ToString();
            tr.Cells.Add(tc);

            double dTotGepland = (double)ArrayGeplandeTijden[24] / 60.00;

            tc = new TableCell();
            tc.Text = dTotGepland.ToString("N1");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            bool isOneven = false;
            double dSubTotaalMinMedewerker = 0;
            double dSubTotaalUrenMedewerker = 0;
            double dEindTotaalMinMedewerker = 0;
            double dEindTotaalUrenMedewerker = 0;
            double[] ArraySubTotaalStap = new double[18];

            Encription.SchrijfNaarLogFile("begin", "");
            foreach (var element in resultMedewerkers)
            {
                dSubTotaalMinMedewerker = 0;
                tr = new TableRow();
                if (isOneven)
                {
                    tr.CssClass = "oneven";
                    isOneven = false;
                }
                else
                {
                    tr.CssClass = "even";
                    isOneven = true;
                }
                ItemOverzicht.Rows.Add(tr);

                tc = new TableCell();
                tc.Text = element.Medewerker;
                tr.Cells.Add(tc);

                var resultStap1 = from u in _db.Uitvoeringen
                                  join ug in _db.Uitvoering_Gegevens
                                  on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                                  where (u.Item_Id == Itemnummer) &&
                                  (u.Medewerker_Id == element.MedewerkerId) &&
                                  (ug.Stap_Id == 1)
                                  select new
                                  {
                                      Tijd = u.TotaleMinuten
                                  };

                int TijdStap1;
                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[0] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";
                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 3)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };


                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[1] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 2)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[2] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 4)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[3] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 5)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[4] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 6)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[5] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 7)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[6] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 8)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[7] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 9)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[8] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 10)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[9] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 11)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[10] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 17)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[11] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 18)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[12] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 12)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[13] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 20)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[14] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 14)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };


                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[15] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 13)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[16] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                resultStap1 = from u in _db.Uitvoeringen
                              join ug in _db.Uitvoering_Gegevens
                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                              where (u.Item_Id == Itemnummer) &&
                              (u.Medewerker_Id == element.MedewerkerId) &&
                              (ug.Stap_Id == 21)
                              select new
                              {
                                  Tijd = u.TotaleMinuten
                              };

                tc = new TableCell();

                if (resultStap1.Count() > 0)
                {
                    TijdStap1 = resultStap1.First().Tijd;
                    dSubTotaalMinMedewerker += TijdStap1;
                    ArraySubTotaalStap[17] += TijdStap1;
                    tc.Text = TijdStap1.ToString();
                }
                else
                    tc.Text = "";

                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dSubTotaalMinMedewerker.ToString();
                tr.Cells.Add(tc);

                dSubTotaalUrenMedewerker = dSubTotaalMinMedewerker / 60.00;
                tc = new TableCell();
                tc.Text = dSubTotaalUrenMedewerker.ToString("N1");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "";
                tr.Cells.Add(tc);

                dEindTotaalMinMedewerker += dSubTotaalMinMedewerker;
                Encription.SchrijfNaarLogFile("stap", "");
            }

            Encription.SchrijfNaarLogFile("totalen", "");

            tr = new TableRow();

            if (isOneven)
            {
                tr.CssClass = "oneven";
            }
            else
            {
                tr.CssClass = "even";
            }

            ItemOverzicht.Rows.Add(tr);

            tc = new TableCell();
            tc.Text = "Totaal";
            tr.Cells.Add(tc);

            for (int i = 0; i < 18; i++)
            {
                tc = new TableCell();
                tc.Text = ArraySubTotaalStap[i].ToString();
                tr.Cells.Add(tc);
            }

            tc = new TableCell();
            tc.Text = dEindTotaalMinMedewerker.ToString();
            tr.Cells.Add(tc);

            dEindTotaalUrenMedewerker = dEindTotaalMinMedewerker / 60.00;

            tc = new TableCell();
            tc.Text = dEindTotaalUrenMedewerker.ToString("N1");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = dUrenNavis.ToString("N1");
            tr.Cells.Add(tc);

            TextMinutenBasis.Text = ArrayGeplandeTijden[24].ToString();
            TextMinutenWorkfl.Text = dEindTotaalMinMedewerker.ToString();

            double TotToezichtBasis = ArrayGeplandeTijden[25];
            double TotWvbBasis = ArrayGeplandeTijden[26];
            double TotDirectenBasis = ArrayGeplandeTijden[27];

            TextToezichtBasis.Text = TotToezichtBasis.ToString();
            TextWvbBasis.Text = TotWvbBasis.ToString();
            TextDirectenBasis.Text = TotDirectenBasis.ToString();

            var resultToezichtWorkf = from u in _db.Uitvoeringen
                                      join ug in _db.Uitvoering_Gegevens
                                      on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                                      where ((u.Item_Id == Itemnummer) &&
                                      ((ug.Stap_Id == 3) || (ug.Stap_Id == 7)))
                                      group u by u.TotaleMinuten into grp
                                      select new
                                      {
                                          ToezichtWorkf = grp.Sum(u => u.TotaleMinuten)
                                      };

            var resultWvbWorkf = from u in _db.Uitvoeringen
                                 join ug in _db.Uitvoering_Gegevens
                                 on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                                 where ((u.Item_Id == Itemnummer) &&
                                 ((ug.Stap_Id == 1) || (ug.Stap_Id == 2) || (ug.Stap_Id == 13) || (ug.Stap_Id == 15) || (ug.Stap_Id == 21)))
                                 group u by u.TotaleMinuten into grp
                                 select new
                                 {
                                     WvbWorkf = grp.Sum(u => u.TotaleMinuten)
                                 };

            double TotToezichtWorkfl = 0;
            double TotWvbWorkfl = 0;
            double TotDirectenWorkfl = 0;

            if (resultToezichtWorkf.Count() > 0)
                TotToezichtWorkfl = Convert.ToDouble(resultToezichtWorkf.First().ToezichtWorkf);

            if (resultWvbWorkf.Count() > 0)
                TotWvbWorkfl = Convert.ToDouble(resultWvbWorkf.First().WvbWorkf);

            TotDirectenWorkfl = dEindTotaalMinMedewerker - TotWvbWorkfl - TotToezichtWorkfl;

            TextToezichtWorkfl.Text = TotToezichtWorkfl.ToString();
            TextWvbWorkfl.Text = TotWvbWorkfl.ToString();
            TextDirectenWorkfl.Text = TotDirectenWorkfl.ToString();

            IEnumerable<Item_Fotos> lCurrentFotos = Data.Functies.Item_Fotos.GetByItemId(Itemnummer);
                var fotoDiv = new HtmlGenericControl("div");
                fotoDiv.Attributes.Add("class", "Company-foto-thumb");

                var fotoA = new HtmlGenericControl("a");
                fotoA.Attributes.Add("href", ResolveClientUrl(ConfigurationManager.AppSettings.Get(ApplicationSettings.UploadUrl) + lCurrentFotos.First().Item_Id + "//" + lCurrentFotos.First().GUID + lCurrentFotos.First().Type));
                fotoDiv.Controls.Add(fotoA);

                var img = new Image
                {
                    ImageUrl = ResolveClientUrl(ConfigurationManager.AppSettings.Get(ApplicationSettings.UploadUrl) + lCurrentFotos.First().Item_Id + "//" + lCurrentFotos.First().GUID + lCurrentFotos.First().Type),
                    Width = 100,
                    Height = 150
                };
                fotoA.Controls.Add(img);

                imtectdetailfotoslider.Controls.Add(fotoDiv);
        }

        protected void btnToPdf_Click(object sender, EventArgs e)
        {
            var pdfConverter = new PdfConverter();
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;
            pdfConverter.HtmlViewerWidth = 1280;
            pdfConverter.LicenseKey = "MxgBEwAAEwcEARMBHQMTAAIdAgEdCgoKCg==";
            string sHtml = "";
            if (Request.Url.ToString().IndexOf("?") > 0)
                sHtml = getHtml(Request.Url + "&pdf=true&cid=" + customerId);
            else
                sHtml = getHtml(Request.Url + "?pdf=true&cid=" + customerId);
            Document mergedDoc = pdfConverter.GetPdfDocumentObjectFromHtmlString(sHtml);

            byte[] pdfBytes = null;
            try
            {
                pdfBytes = mergedDoc.Save();
            }
            finally
            {
                mergedDoc.Close();
            }

            if (pdfBytes != null)
            {
                //Save document in documents.
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition",
                                   string.Format("attachment; filename=Itemrapport_{0:ddMMyyyy}.pdf", DateTime.Now));
                Response.BinaryWrite(pdfBytes);
                Response.End();
            }
        }

        protected string getHtml(string url)
        {
            var myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myWebRequest.Method = "GET";
            var myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            var myWebSource = new StreamReader(myWebResponse.GetResponseStream());
            string myPageSource = string.Empty;
            myPageSource = myWebSource.ReadToEnd();
            myWebResponse.Close();
            return myPageSource;
        }
    }
}
