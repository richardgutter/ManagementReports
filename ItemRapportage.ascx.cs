#region Directives

using System;
using Ncdo.Company.Framework;
using Ncdo.Company.Data.Entiteiten;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Ncdo.Company.Framework.Security;
using System.Web.UI.WebControls;

#endregion

namespace Ncdo.Company.WebInterface.Management_Rapportage
{
    public partial class ItemRapportage : CompanyModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            int itemId;

            double dTotaantalminuten = 0;
            double dTotaantaluren = 0;

            base.OnLoad(e);

            if (Request.QueryString["i"] != null)
            {
                Session["Item_ItemId"] = Request.QueryString["i"];
                itemId = Convert.ToInt32(Request.QueryString["i"]);
                //              rgManagementRapportage.ClientSettings.Scrolling.AllowScroll = true;

                int[] ArrayGeplandeTijden = new int[28];
                ArrayGeplandeTijden = OndersteunendeFuncties.GeefGeplandeTijdenArray(itemId);

                using (var _db = new Entities())
                {
                 var resultBasis = from ic in _db.Item_Calculaties
                                      where (ic.Item_Id == itemId) &&
                                      (!ic.Meerwerk)
                                      group ic by ic.Totaalbedrag into grp
                                      select new
                                      {
                                          TotBasis = grp.Sum(ic => ic.Totaalbedrag)
                                      };

                if (resultBasis.Count() > 0)
                 TextBasis.Text = resultBasis.First().TotBasis.ToString();

                 var resultMeerwerk = from ic in _db.Item_Calculaties
                             where (ic.Item_Id == itemId) &&
                             (ic.Meerwerk)
                             group ic by ic.Totaalbedrag into grp
                             select new
                             {
                                 TotMeerwerk = grp.Sum(ic => ic.Totaalbedrag)
                };

                 if (resultMeerwerk.Count() > 0)
                     TextMeerwerk.Text = resultMeerwerk.First().TotMeerwerk.ToString();

                 var resultMateriaal = from m in _db.Materialen
                             where (m.Item_Id == itemId)
                             group m by (m.Aantal * m.Prijs)* (1 + m.Opslag / 100) into grp
                                       select new 
                             {
                                 TotMateriaal = grp.Sum(p => (p.Aantal * p.Prijs)* (1 + p.Opslag / 100))
                             };

                 if (resultMateriaal.Count() > 0)
                     TextMateriaalOpbrengsten.Text = resultMateriaal.First().TotMateriaal.ToString();

                 double dTextTotaleOpbrengsten = Convert.ToDouble(resultBasis.First().TotBasis) + Convert.ToDouble(resultMeerwerk.First().TotMeerwerk) + Convert.ToDouble(resultMateriaal.First().TotMateriaal);
                 TextTotaleOpbrengsten.Text = dTextTotaleOpbrengsten.ToString();

                    var resultKosten = from m in _db.ManagementInfo
                                    join p in _db.Projecten
                                    on m.Project_id equals p.Project_Id
                                    join i in _db.Items
                                    on p.Project_Id equals i.Project_Id
                                    where (i.Item_Id == itemId)
                                    select new
                                   {
                                       Uren = m.AantalUren,
                                       Basis = m.Arbeid,
                                       Materiaal = m.Materiaal,
                                       Materieel = m.Materieel,
                                       Onderaannemer = m.Onderaanneming,
                                       Overige = m.OverigeKosten
                                   };

                    double dTextTotaleKosten;
                    if (resultKosten.Count() > 0)
                    {
                        dTextTotaleKosten = Convert.ToDouble(resultKosten.First().Basis) + Convert.ToDouble(resultKosten.First().Materiaal) + Convert.ToDouble(resultKosten.First().Materieel) + Convert.ToDouble(resultKosten.First().Onderaannemer) + Convert.ToDouble(resultKosten.First().Overige);
                        TextUrenNavis.Text = resultKosten.First().Uren.ToString();
                    }
                    else
                        dTextTotaleKosten = 0;

                     TextTotaleKosten.Text = dTextTotaleKosten.ToString();
                 
                 var resultMinuten = from u in _db.Uitvoeringen
                                 join m in _db.Medewerkers
                                 on u.Medewerker_Id equals m.Medewerker_Id
                                 where u.Item_Id == itemId
                                 group u by
                                 new
                                 {
                                     m.GebruikersNaam,
                                     u.TotaleMinuten
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

                    tcg.Text = "";
                    trg.Cells.Add(tcg);

                    var tc2g = new TableCell();

                    tc2g.Text = ArrayGeplandeTijden[24].ToString();
                    trg.Cells.Add(tc2g);

                    var tc3g = new TableCell();

                    double dsubTotaantalminuteng = ArrayGeplandeTijden[24];
                    double dsubTotaantalureng = dsubTotaantalminuteng / 60.00;

                    tc3g.Text = dsubTotaantalureng.ToString();
                    trg.Cells.Add(tc3g);

                    var tc4g = new TableCell();

                    tc4g.Text = "";
                    trg.Cells.Add(tc4g);

                    ItemOverzicht.Rows.Add(trg);

                    foreach (var element in resultMinuten)
                    {
                        var tr = new TableRow();

                        dt.Rows.Add(tr);

                        var tc = new TableCell();

                        tc.Text = element.Medewerker;
                        tr.Cells.Add(tc);

                        var tc2 = new TableCell();

                        tc2.Text = element.TotaalAantalMinuten.ToString();
                        tr.Cells.Add(tc2);

                        var tc3 = new TableCell();

                        double dsubTotaantalminuten = element.TotaalAantalMinuten;
                        double dsubTotaantaluren = dsubTotaantalminuten / 60.00;

                        dTotaantalminuten += dsubTotaantalminuten;
                        dTotaantaluren += dTotaantaluren;

                        tc3.Text = dsubTotaantaluren.ToString();
                        tr.Cells.Add(tc3);

                        var tc4 = new TableCell();

                        tc4.Text = "";
                        tr.Cells.Add(tc4);

                        ItemOverzicht.Rows.Add(tr);
                    }
                    var trt = new TableRow();

                    dt.Rows.Add(trt);

                    var tc1t = new TableCell();

                    tc1t.Text = "";
                    trt.Cells.Add(tc1t);

                    var tc2t = new TableCell();

                    tc2t.Text = dTotaantalminuten.ToString();
                    trt.Cells.Add(tc2t);

                    var tc3t = new TableCell();

                    tc3t.Text = dTotaantaluren.ToString();
                    trt.Cells.Add(tc3t);

                    var tc4t = new TableCell();

                    tc4t.Text = "";
                    trt.Cells.Add(tc4t);

                    ItemOverzicht.Rows.Add(trt);

                    //TextUrenNavis.Text = resultKosten.First().Uren.ToString();
                    TextUrenBasis.Text = dsubTotaantalureng.ToString();
                    TextUrenWorkfl.Text = dTotaantalminuten.ToString();
                }

            }
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
 
*/

