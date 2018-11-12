#region Directives

using System;
using System.Web;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Ncdo.Company.Data.Entiteiten;
using Ncdo.Company.Framework.Security;

#endregion

namespace Ncdo.Company.WebInterface.Management_Rapportage
{
    public class OndersteunendeFuncties
    {
        public static int[] GeefGeplandeTijdenArray(int itemId)
        {
            int[] ArrayGeplandeTijden = new int[28];

            using (var _db = new Entities())
            {
                var result = from kcr in _db.Klant_Contract_Regels
                             join ic in _db.Item_Calculaties
                             on kcr.IndexNr equals ic.IndexNr
                             join i in _db.Items
                             on ic.Item_Id equals i.Item_Id
                             join kc in _db.Klant_Contracten
                             on i.Klant_Id equals kc.Klant_Id
                             where (kc.Contract_Id == kcr.Contract_Id) &&
                             (!ic.Meerwerk) &&
                             (i.Item_Id == itemId)
                             select new
                             {
                                 Afmeting = kcr.Afmeting,
                                 Drukklasse = kcr.Drukklasse,
                                 Inschrijven = kcr.Inschrijven,
                                 Werkvoorbereiding = kcr.Werkvoorbereiding,
                                 EersteInspectie = kcr.EersteInspectie,
                                 PreTesten = kcr.PreTesten,
                                 Demontage = kcr.Demontage,
                                 Stralen = kcr.Stralen,
                                 TweedeInspectie = kcr.TweedeInspectie,
                                 ConserverenEersteLaag = kcr.ConserverenEersteLaag,
                                 Machinaal = kcr.Machinaal,
                                 Montage = kcr.Montage,
                                 Testen = kcr.Testen,
                                 ConserverenTweedeLaag = kcr.ConserverenTweedeLaag,
                                 QualityControl = kcr.QualityControl,
                                 TransportGereedMaken = kcr.TransportGereedMaken,
                                 E_I = kcr.E_I,
                                 O2Clean = kcr.O2Clean,
                                 Afmonteren = kcr.Afmonteren,
                                 Calculatie = kcr.Calculatie,
                             };

                foreach (var element in result)
                {
                    ArrayGeplandeTijden[0] += element.Inschrijven;
                    ArrayGeplandeTijden[1] += element.Werkvoorbereiding;
                    ArrayGeplandeTijden[2] += element.EersteInspectie;
                    ArrayGeplandeTijden[3] += element.PreTesten;
                    ArrayGeplandeTijden[4] += element.Demontage;
                    ArrayGeplandeTijden[5] += element.Stralen;
                    ArrayGeplandeTijden[6] += element.TweedeInspectie;
                    ArrayGeplandeTijden[7] += element.ConserverenEersteLaag;
                    ArrayGeplandeTijden[8] += element.Machinaal;
                    ArrayGeplandeTijden[9] += element.Montage;
                    ArrayGeplandeTijden[10] += element.Testen;
                    ArrayGeplandeTijden[11] += element.ConserverenTweedeLaag;
                    ArrayGeplandeTijden[12] += 0;
                    ArrayGeplandeTijden[13] += element.QualityControl;
                    ArrayGeplandeTijden[14] += element.TransportGereedMaken;
                    ArrayGeplandeTijden[15] += 0;
                    ArrayGeplandeTijden[16] += element.E_I;
                    ArrayGeplandeTijden[17] += element.O2Clean;
                    ArrayGeplandeTijden[18] += 0;
                    ArrayGeplandeTijden[19] += element.Afmonteren;
                    ArrayGeplandeTijden[20] += element.Calculatie;
                    ArrayGeplandeTijden[21] += 0;
                    ArrayGeplandeTijden[22] += 0;
                    ArrayGeplandeTijden[23] += 0;

                    for (int i = 0; i < 24; i++)
                        ArrayGeplandeTijden[24] += ArrayGeplandeTijden[i];

                    ArrayGeplandeTijden[25] += ArrayGeplandeTijden[2] + ArrayGeplandeTijden[6];
                    ArrayGeplandeTijden[26] += ArrayGeplandeTijden[0] + ArrayGeplandeTijden[1] + ArrayGeplandeTijden[14] + ArrayGeplandeTijden[20];
                    ArrayGeplandeTijden[27] += ArrayGeplandeTijden[24] - ArrayGeplandeTijden[25] - ArrayGeplandeTijden[26];
                }
            }
            return ArrayGeplandeTijden;
        }
    }
}


/*            
            using (var _db = new Entities())
           {
               BasicLines = _db.Klant_Contract_Regels
                   .Join(_db.Item_Calculaties, kcr => kcr.IndexNr, ic => ic.IndexNr, (krc, ic) => new { krc, ic })
                   .Join(_db.Items, itca => itca.ic.Item_Id, i => i.Item_Id, (itca, i) => new { itca, i })
                   .Join(_db.Klant_Contracten, i => i.i.Klant_Id, klco => klco.Klant_Id, (i, klco) => new { i, klco })
                   .Where(item => item.klco.Contact_Id == item.
                       
                       (Itemnummer == null || item.i.Item_Identifier.IndexOf(Itemnummer) >= 0) &&
                                  (Tagnr == null || item.i.TagNr.IndexOf(Tagnr) >= 0) &&
                                  (ProjectNummer <= 0 || item.i.Project_Id == ProjectNummer) &&
                                  (Klant <= 0 || item.i.Klant_Id == Klant) &&
                                  (Soort <= 0 || item.i.Item_Soort_Id == Soort) &&
                                  item.i.IsDeleted == false
                   )
                   .Select(
                       item =>
                       new BasicLines
                       {
                           ItemId = item.i.Item_Id,
                           Itemnummer = item.i.Item_Identifier,
                           TagNr = item.i.TagNr,
                           Opleverdatum = item.i.Verwachteopleverdatum,
                           Klant = item.k.Bedrijfsnaam,
                           Soort = item.i.Item_Soorten.Naam,
                           Project = item.p.ProjectNr,
                           OpmerkingenWvb = item.i.Klachtomschrijving
                       })
                   .OrderBy(i => i.ItemId)
                   .ToList();
           }


            Inschrijven = 1,
            Werkvoorbereiding = 2,
            EersteInspectie = 3,
            PreTesten = 4,
            Demontage = 5,
            Stralen = 6,
            TweedeInspectie = 7,
            ConserverenEersteLaag = 8,
            Machinaal = 9,
            Montage = 10,
            Testen = 11,
            ConserverenTweedeLaag = 12,
            // Transport_gereed_maken = 13,
            QualityControl = 14,
            Transport = 15,
            // Technisch_Afgerond = 16,
            E_I = 17,
            O2Clean = 18,
            // Voorman = 19,
            Afmonteren = 20,
            Calculatie = 21,
            // Compleet = 22,
            // Planning = 23,
            // Afgekeurd = 24
*/
