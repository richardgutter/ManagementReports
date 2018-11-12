#region Directives

using System;
using System.Globalization;
using Ncdo.Company.Framework;
using EvoPdf.HtmlToPdf;
using EvoPdf.HtmlToPdf.PdfDocument;
using System.Net;
using System.IO;
using Ncdo.Company.Framework.Security;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
//using Medewerkers = Ncdo.Company.Data.Functies.Medewerkers;

#endregion

namespace Ncdo.Company.WebInterface.Management_Rapportage
{
    public partial class ProjectRapportage : CompanyModuleBase
    {
        public static string sOpenUrl { get; set; }
        protected string SRedirectUrl { get; set; }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int itemId;

            rgDocument.Height = 600;

            double dTotaantalminuten = 0;
            double dTotaantaluren = 0;

            if (Request.QueryString["p"] != null)
            {
                Session["ProjectDetail_Project_ProjectId"] = Request.QueryString["p"];
                sOpenUrl = GetUrl("Page_ManagementItemRapportage_ProjectRapportage") + "?p=" + Request.QueryString["p"] + "&i=";
            }

            int projectId = Convert.ToInt32(Request.QueryString["p"]);
            decimal dTotaalbedragOpbrengsten = Data.Functies.Projecten.GetTotal(Convert.ToInt32(Request.QueryString["p"]));

            var _db2 = new Ncdo.Company.Data.Entiteiten.Entities();
            var resultKosten2 = from m in _db2.ManagementInfo
                               join p in _db2.Projecten
                               on m.Project_id equals p.Project_Id
                               join i in _db2.Items
                               on p.Project_Id equals i.Project_Id
                               where (p.Project_Id == projectId)
                               select new
                               {
                                   Uren = m.AantalUren,
                                   Basis = m.Arbeid,
                                   Materiaal = m.Materiaal,
                                   Materieel = m.Materieel,
                                   Onderaannemer = m.Onderaanneming,
                                   Overige = m.OverigeKosten
                               };

            decimal dBasisKosten2 = 0;
            decimal dMateriaalKosten2 = 0;
            decimal dMaterieelKosten2 = 0;
            decimal dOnderaannemerKosten2 = 0;
            decimal dOverigeKosten2 = 0;

            decimal dTotaleKosten2 = 0;

            if (resultKosten2.Count() > 0)
            {
                dBasisKosten2 = Convert.ToDecimal(resultKosten2.First().Basis);
                dMateriaalKosten2 = Convert.ToDecimal(resultKosten2.First().Materiaal);
                dMaterieelKosten2 = Convert.ToDecimal(resultKosten2.First().Materieel);
                dOnderaannemerKosten2 = Convert.ToDecimal(resultKosten2.First().Onderaannemer);
                dOverigeKosten2 = Convert.ToDecimal(resultKosten2.First().Overige);
            }
            dTotaleKosten2 = dBasisKosten2 + dMateriaalKosten2 + dMaterieelKosten2 + dOnderaannemerKosten2 + dOverigeKosten2;
            TextTotaleKosten.Text = dTotaleKosten2.ToString("C", new CultureInfo("nl-NL"));

           decimal dTotaalVerschil = 0;

            dTotaalVerschil = dTotaalbedragOpbrengsten - dTotaleKosten2;

            lblTotaalbedragOpbrengsten.Text = dTotaalbedragOpbrengsten.ToString("C", new CultureInfo("nl-NL"));
            lblTotaalbedragKosten.Text = dTotaleKosten2.ToString("C", new CultureInfo("nl-NL"));
            lblTotaalVerschil.Text = dTotaalVerschil.ToString("C", new CultureInfo("nl-NL"));

            if (Request.QueryString["i"] != null)
            {
                btnShowRapport.Visible = true;
                FilterPanel2.Visible = true;
                ItemOverzicht.Visible = true;
                Session["Item_ItemId"] = Request.QueryString["i"];
                itemId = Convert.ToInt32(Request.QueryString["i"]);
                //              rgManagementRapportage.ClientSettings.Scrolling.AllowScroll = true;

                int[] ArrayGeplandeTijden = new int[28];
                ArrayGeplandeTijden = OndersteunendeFuncties.GeefGeplandeTijdenArray(itemId);

                using (var _db = new Ncdo.Company.Data.Entiteiten.Entities())
                {
                    var resultBasis = from ic in _db.Item_Calculaties
                                      where (ic.Item_Id == itemId) &&
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
                                         where (ic.Item_Id == itemId) &&
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
                                          where (m.Item_Id == itemId)
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
                                       where (p.Project_Id == projectId)
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
                    TextMinutenNavis.Text = (dUrenNavis*60).ToString();

                    var resultMinuten = from u in _db.Uitvoeringen
                                        join m in _db.Medewerkers
                                        on u.Medewerker_Id equals m.Medewerker_Id
                                        where u.Item_Id == itemId
                                        group u by
                                        new
                                        {
                                            m.GebruikersNaam//,
                                        } into grp
                                        select new
                                        {
                                            Medewerker = grp.Key.GebruikersNaam,
                                            TotaalAantalMinuten = grp.Sum(u => u.TotaleMinuten)
                                        };

                    var dt = new DataTable();

                    dt.Columns.Add(new DataColumn("Medewerkers", typeof(String)));
                    dt.Columns.Add(new DataColumn("TotMinuten", typeof(String)));
                    dt.Columns.Add(new DataColumn("TotUren", typeof(String)));
                    dt.Columns.Add(new DataColumn("Navision", typeof(String)));

                    var trg = new TableRow();

                    ItemOverzicht.Rows.Add(trg);

                    var tcg = new TableCell();

                    tcg.Text = "Geplande tijden";
                    trg.Cells.Add(tcg);

                    var tc2g = new TableCell();

                    tc2g.Text = ArrayGeplandeTijden[24].ToString();
                    trg.Cells.Add(tc2g);

                    var tc3g = new TableCell();

                    double dsubTotaantalminuteng = ArrayGeplandeTijden[24];
                    double dsubTotaantalureng = dsubTotaantalminuteng / 60.00;

                    tc3g.Text = dsubTotaantalureng.ToString("N1", new CultureInfo("nl-NL"));
                    trg.Cells.Add(tc3g);

                    var tc4g = new TableCell();

                    tc4g.Text = "";
                    trg.Cells.Add(tc4g);

                    ItemOverzicht.Rows.Add(trg);

                    foreach (var element in resultMinuten)
                    {
/*                        Ncdo.Company.Data.Entiteiten.Medewerkers lMedewerker;
                        lMedewerker = Ncdo.Company.Data.Functies.Medewerkers.GetAnywayById(element.Medewerker);
                        var resultMedewerker = from m in _db.Medewerkers
                                               where m.Medewerker_Id == element.Medewerker
                                               select new
                                               {
                                                   Medewerkersnaam = m.GebruikersNaam
                                               };
                        */
                        var tr = new TableRow();

                        dt.Rows.Add(tr);

                        var tc = new TableCell();

                        tc.Text = element.Medewerker;               //resultMedewerker.First().Medewerkersnaam; //element.Medewerker;
                        tr.Cells.Add(tc);

                        var tc2 = new TableCell();

                        tc2.Text = element.TotaalAantalMinuten.ToString();
                        tr.Cells.Add(tc2);

                        var tc3 = new TableCell();

                        double dsubTotaantalminuten = element.TotaalAantalMinuten;
                        double dsubTotaantaluren = dsubTotaantalminuten / 60.00;

                        dTotaantalminuten += dsubTotaantalminuten;
                        dTotaantaluren += dsubTotaantaluren;

                        tc3.Text = dsubTotaantaluren.ToString("N1", new CultureInfo("nl-NL"));
                        tr.Cells.Add(tc3);

                        var tc4 = new TableCell();

                        tc4.Text = "";
                        tr.Cells.Add(tc4);

                        ItemOverzicht.Rows.Add(tr);
                    }
                    var trt = new TableRow();

                    dt.Rows.Add(trt);

                    var tc1t = new TableCell();

                    tc1t.Text = "Totaal uitvoering";
                    trt.Cells.Add(tc1t);

                    var tc2t = new TableCell();

                    tc2t.Text = dTotaantalminuten.ToString();
                    trt.Cells.Add(tc2t);

                    var tc3t = new TableCell();

                    tc3t.Text = dTotaantaluren.ToString("N1", new CultureInfo("nl-NL"));
                    trt.Cells.Add(tc3t);

                    var tc4t = new TableCell();

                    tc4t.Text = dUrenNavis.ToString();
                    trt.Cells.Add(tc4t);

                    ItemOverzicht.Rows.Add(trt);

                    //TextUrenNavis.Text = resultKosten.First().Uren.ToString();
                    TextMinutenBasis.Text = dsubTotaantalminuteng.ToString();
                    TextMinutenWorkfl.Text = dTotaantalminuten.ToString();


                    double TotToezichtBasis = ArrayGeplandeTijden[25];
                    double TotWvbBasis = ArrayGeplandeTijden[26];
                    double TotDirectenBasis = ArrayGeplandeTijden[27];

                    TextToezichtBasis.Text = TotToezichtBasis.ToString();
                    TextWvbBasis.Text = TotWvbBasis.ToString();
                    TextDirectenBasis.Text = TotDirectenBasis.ToString();

                    var resultToezichtWorkf = from u in _db.Uitvoeringen
                                              join ug in _db.Uitvoering_Gegevens
                                              on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                                              where ((u.Item_Id == itemId) &&
                                              ((ug.Stap_Id == 3) || (ug.Stap_Id == 7)))
                                              group u by u.TotaleMinuten into grp
                                              select new
                                              {
                                                  ToezichtWorkf = grp.Sum(u => u.TotaleMinuten)
                                              };

                    var resultWvbWorkf = from u in _db.Uitvoeringen
                                         join ug in _db.Uitvoering_Gegevens
                                         on u.Stap_Gegevens_Id equals ug.Stap_Gegevens_Id
                                         where ((u.Item_Id == itemId) &&
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

                    TotDirectenWorkfl = dTotaantalminuten - TotWvbWorkfl - TotToezichtWorkfl;

                    TextToezichtWorkfl.Text = TotToezichtWorkfl.ToString();
                    TextWvbWorkfl.Text = TotWvbWorkfl.ToString();
                    TextDirectenWorkfl.Text = TotDirectenWorkfl.ToString();
                }
            }
            else
            {
                btnShowRapport.Visible = false;
                FilterPanel2.Visible = false;
                ItemOverzicht.Visible = false;
            }
        }

        private string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
        {
            if (Request.IsSecureConnection)
                return string.Format("https://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

            return string.Format("http://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));
        }

        private static string GetHtml(string url)
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

        protected void btnShowRapport_Click(object sender, EventArgs e)
        {
            SRedirectUrl = ResolveClientUrl("~/DesktopModules/Company/Management Rapportage/ManagementRapport.aspx");
            SRedirectUrl += "?p=" + Request.QueryString["p"];
            SRedirectUrl += "&i=" + Request.QueryString["i"];
        }
    }
}


/*
  var result2 = from u in _db.Uitvoeringen
                             from m in _db.Medewerkers
                             where u.Item_Id == itemId && u.Medewerker_Id == m.Medewerker_Id
                             group u by
                             new{m.GebruikersNaam,
                                 u.TotaleMinuten
                             } into grp
                             select new{
                                 grp.Key.GebruikersNaam,
                                 tot = grp.Sum(u => u.TotaleMinuten)
                             };
 
 * 
 * 
 * 
 * materiaal:
 * 
  * 
 * select sum(((Aantal * Prijs)* (1 + Opslag / 100))) as totaalprijs
  from [Company.Dev].[dbo].[Materialen]
  where Item_Id = 2044
  
 * 
 * arbeid en meerwerk:
  
  select SUM(totaalbedrag)
  from [Company.Dev].[dbo].[Item_Calculaties]
  where  Item_Id = 2044 and Meerwerk = 0
   
  select SUM(totaalbedrag)
  from [Company.Dev].[dbo].[Item_Calculaties]
  where  Item_Id = 2044 and Meerwerk = 1
 * 
 * 
  Select SUM(TotaleMinuten)
  from Uitvoeringen
  inner join Uitvoering_Gegevens
  on Uitvoeringen.Stap_Gegevens_Id = Uitvoering_Gegevens.Stap_Gegevens_Id
where Uitvoeringen.Item_Id = 53 and (Stap_Id = 3 or Stap_Id = 7)

   
  Select SUM(TotaleMinuten)
  from Uitvoeringen
  inner join Uitvoering_Gegevens
  on Uitvoeringen.Stap_Gegevens_Id = Uitvoering_Gegevens.Stap_Gegevens_Id
where Uitvoeringen.Item_Id = 53 and (Stap_Id = 1 or Stap_Id = 2 or Stap_Id = 13 or Stap_Id = 15 or Stap_Id = 21)

*/