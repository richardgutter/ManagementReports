#region Directives

using System;
using System.Collections.Generic;
using System.Linq;
using Nice2do.Imtech.Data.Entiteiten;
using Nice2do.Imtech.Framework;
using Nice2do.Imtech.Framework.Security;
using Telerik.Web.UI;
using GemBox.Spreadsheet;
using System.Data;
using System.IO;

#endregion


namespace Nice2do.Imtech.WebInterface.Management_Rapportage
{
    public partial class Overzicht : ImtechModuleBase
    {
        public static string sEditUrl
        {
            get { return GetUrl("Page_ManagementItemRapportage_Detail") + "?p="; }
        }

        public string sDownloadUrl { get; set; }
        public static bool bFilter { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int Medewerker_Id;
            int Medewerker_Role;
            if (!Authentication.IsAuthenticated(out Medewerker_Id, out Medewerker_Role))
                Response.Redirect(GetUrl("Page_Account_Login") + "?ReturnUrl=" +
                                  Server.UrlEncode(Request.Url.ToString()));

            if (
                !(Medewerker_Role == (int)ApplicationDefinitions.MedewerkersRollen.Werkvoorbereiding ||
                  Medewerker_Role == (int)ApplicationDefinitions.MedewerkersRollen.Management))
            {
                Notifications.Error("U mag deze pagina niet bekijken.", this);
                pnlOverzicht.Visible = false;
                return;
            }
            if (Request["PD"] != null && Convert.ToInt32(Request["PD"]) > 0)
            {
                sDownloadUrl = ConvertRelativeUrlToAbsoluteUrl("/DesktopModules/Imtech/Beheer/Project/ProjectPDF.aspx");
                sDownloadUrl += "?pd=" + Request["PD"] + "&se=true";
            }

            if (Session["Imtech_Project_Overview_Filter_Klant"] != null)
            {
                bFilter = true;
                var currentCustomers = new List<Klanten>();
                try
                {
                    currentCustomers.Add(
                        Data.Functies.Klanten.GetById(int.Parse(Session["Imtech_Project_Overview_Filter_Klant"].ToString())));
                }
                catch
                {
                }
                rlbKlant.DataSource = currentCustomers;
                rlbKlant.DataBind();
                rlbKlant.SelectedValue = Session["Imtech_Project_Overview_Filter_Klant"].ToString();
            }
            if (Session["Imtech_Project_Overview_Filter_Locatie"] != null)
            {
                bFilter = true;
                var currentLocations = new List<Klant_Locaties>();
                try
                {
                    currentLocations.Add(
                        Data.Functies.Klant_Locaties.GetById(
                            int.Parse(Session["Imtech_Project_Overview_Filter_Locatie"].ToString())));
                }
                catch
                {
                }
                rlbLocatie.DataSource = currentLocations;
                rlbLocatie.DataBind();
                rlbLocatie.SelectedValue = Session["Imtech_Project_Overview_Filter_Locatie"].ToString();
            }
            if (Session["Imtech_Project_Overview_Filter_Status"] != null)
            {
                bFilter = true;
                rlbStatus.SelectedValue = Session["Imtech_Project_Overview_Filter_Status"].ToString();
            }
            if (Session["Imtech_Project_Overview_Filter_Projectnummer"] != null)
            {
                txtProjectnummer.Text = Session["Imtech_Project_Overview_Filter_Projectnummer"].ToString();
                bFilter = true;
            }
            if (Session["Imtech_Project_Overview_Filter_Opdrachtnummer"] != null)
            {
                txtOpdrachtnummer.Text = Session["Imtech_Project_Overview_Filter_Opdrachtnummer"].ToString();
                bFilter = true;
            }
        }

        protected void rlbKlant_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            List<Klanten> k = Data.Functies.Klanten.GetFiltered(e.Text);


            IEnumerable<Klanten> kFiltered = k.Skip(e.NumberOfItems).Take(25);
            rlbKlant.DataSource = kFiltered;
            rlbKlant.ClearSelection();
            rlbKlant.DataBind();

            int endOffset = e.NumberOfItems + kFiltered.Count();
            int totalCount = k.Count();

            if (endOffset == totalCount)
                e.EndOfItems = true;

            e.Message = String.Format("Klanten 1-{0} van de {1}", endOffset, totalCount);
        }

        protected void rlbLocatie_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            List<Klant_Locaties> kl = Data.Functies.Klant_Locaties.GetFiltered(e.Text);

            IEnumerable<Klant_Locaties> klFiltered = kl.Skip(e.NumberOfItems).Take(25);
            rlbLocatie.DataSource = klFiltered;
            rlbLocatie.ClearSelection();
            rlbLocatie.DataBind();

            int endOffset = e.NumberOfItems + klFiltered.Count();
            int totalCount = kl.Count();

            if (endOffset == totalCount)
                e.EndOfItems = true;

            e.Message = String.Format("Locaties 1-{0} van de {1}", endOffset, totalCount);
        }

/*
        protected void btnAanmaken_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetUrl("Page_Beheer_Project_Bewerken"));
        }
*/
        public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
        {
            if (Request.IsSecureConnection)
                return string.Format("https://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

            return string.Format("http://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            Session.Remove("Imtech_Project_Overview_Filter_Klant");
            Session.Remove("Imtech_Project_Overview_Filter_Locatie");
            Session.Remove("Imtech_Project_Overview_Filter_Status");
            Session.Remove("Imtech_Project_Overview_Filter_Projectnummer");
            Session.Remove("Imtech_Project_Overview_Filter_Opdrachtnummer");

            rlbKlant.Text = "";
            rlbStatus.Text = "";
            txtProjectnummer.Text = "";
            txtOpdrachtnummer.Text = "";

            rlbKlant.ClearSelection();
            rlbStatus.ClearSelection();
            rlbLocatie.ClearSelection();
        }

        protected void btnImportNavision_Click(object sender, EventArgs e)
        {
            bool IsFileUploaded = (fileUpload.PostedFile != null) && (fileUpload.PostedFile.ContentLength > 0);
            string fileExtension = Path.GetExtension(fileUpload.PostedFile.FileName).ToLower();
            if (IsFileUploaded && fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                Notifications.Error("U kunt alleen maar Excel bestanden uploaden.", this);
                return;
            }

            if (IsFileUploaded)
            {
                var dt = new DataTable();
                try
                {
                    SpreadsheetInfo.SetLicense("EPAR-TIT0-QDQR-NCHQ");

                    ExcelFile ef = new ExcelFile();
                    ExcelWorksheet ws = default(ExcelWorksheet);

                    ef.LoadXls(fileUpload.PostedFile.InputStream);
                    ws = ef.Worksheets[0];

                    dt.Columns.Add(new DataColumn("Projectnummer", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg1", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg2", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg3", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg4", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg5", typeof(String)));
                    dt.Columns.Add(new DataColumn("AantalUren", typeof(String)));
                    dt.Columns.Add(new DataColumn("Arbeid", typeof(String)));
                    dt.Columns.Add(new DataColumn("Materiaal", typeof(String)));
                    dt.Columns.Add(new DataColumn("Materieel", typeof(String)));
                    dt.Columns.Add(new DataColumn("Onderaanneming", typeof(String)));
                    dt.Columns.Add(new DataColumn("OverigeKosten", typeof(String)));
                    dt.Columns.Add(new DataColumn("TotaalKosten", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg6", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg7", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg8", typeof(String)));
                    dt.Columns.Add(new DataColumn("TotaalKostenIncl", typeof(String)));
                    dt.Columns.Add(new DataColumn("Leeg9", typeof(String)));
                    dt.Columns.Add(new DataColumn("Opbrengsten", typeof(String)));

                    // ws.ExtractDataEvent += ExtractDataErrorHandler;

                    ws.ExtractDataEvent += (sender2, e2) =>
                    {
                        if (e2.ErrorID == ExtractDataError.WrongType)
                        {
                            e2.DataTableValue = e2.ExcelValue == null ? null : e2.ExcelValue.ToString();
                            e2.Action = ExtractDataEventAction.Continue;
                        }
                    };
                    ws.ExtractToDataTable(dt, ws.Rows.Count, ExtractDataOptions.StopAtFirstEmptyRow, ws.Rows[7],
                                          ws.Columns[0]);

                    foreach (DataRow dr in dt.Rows)
                    {
                        int aantal = dt.Rows.Count;
                        var mi = new Data.Entiteiten.ManagementInfo();
                        Projecten lProject;
                        string projectnr = dr["Projectnummer"].ToString();

                        lProject = null;
                        using (var db = new Nice2do.Imtech.Data.Entiteiten.Entities())
                        {
                            lProject = db.Projecten.FirstOrDefault(p => p.ProjectNr == projectnr && p.IsDeleted == false);
                        }
                        
                        if (lProject != null)
                        {
                            ManagementInfo lManagementInfo;
                            using (var db = new Nice2do.Imtech.Data.Entiteiten.Entities())
                            {
                                lManagementInfo = db.ManagementInfo.FirstOrDefault(m => m.Project_id == lProject.Project_Id);
                            }

                            if (lManagementInfo != null)
                                Nice2do.Imtech.Data.Functies.ManagementInfo.Delete(lProject.Project_Id);

                            mi.Project_id = lProject.Project_Id;
                            mi.AantalUren = Convert.ToDecimal(dr["AantalUren"].ToString().Replace(",", "."));
                            try
                            {
                                mi.Arbeid = Convert.ToDecimal(dr["Arbeid"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.Arbeid = 0;
                            }
                            try
                            {
                                mi.Materiaal = Convert.ToDecimal(dr["Materiaal"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.Materiaal = 0;
                            }
                            try
                            {
                                mi.Materieel = Convert.ToDecimal(dr["Materieel"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.Materieel = 0;
                            }
                            try
                            {
                                mi.Onderaanneming = Convert.ToDecimal(dr["Onderaanneming"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.Onderaanneming = 0;
                            }
                            try
                            {
                                mi.OverigeKosten = Convert.ToDecimal(dr["OverigeKosten"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.OverigeKosten = 0;
                            }
                            try
                            {
                                mi.TotaalKosten = Convert.ToDecimal(dr["TotaalKosten"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.TotaalKosten = 0;
                            }
                            try
                            {
                                mi.TotaalKostenIncl = Convert.ToDecimal(dr["TotaalKostenIncl"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.TotaalKostenIncl = 0;
                            }
                            try
                            {
                                mi.Opbrengsten = Convert.ToDecimal(dr["Opbrengsten"].ToString().Replace(",", "."));
                            }
                            catch
                            {
                                mi.Opbrengsten = 0;
                            }

                            Nice2do.Imtech.Data.Functies.ManagementInfo.Add(mi);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}

 