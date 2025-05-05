using CwcExim.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;

namespace CwcExim.UtilityClasses
{
    internal class ReportingHelper : IDisposable
    {
        private bool _disposed = false;
        private bool _returnValue = false;
        private readonly Document _document;
        private bool _autoHeaderFooter = false;
        /* For Footer Only */
        private bool _autoFooter = false;

        private Document Document { get { return new Document(); } }

        private void SetPageSize(PdfPageSize size)
        {
            var _size = PageSize.A4;
            switch (size)
            {
                case PdfPageSize.A3:
                    _document.SetPageSize(PageSize.A3);
                    break;
                case PdfPageSize.A4:
                    _document.SetPageSize(PageSize.A4);
                    break;
                case PdfPageSize.A5:
                    _document.SetPageSize(PageSize.A5);
                    break;
                case PdfPageSize.B3:
                    _document.SetPageSize(PageSize.B3);
                    break;
                case PdfPageSize.B4:
                    _document.SetPageSize(PageSize.B4);
                    break;
                case PdfPageSize.B5:
                    _document.SetPageSize(PageSize.B5);
                    break;
                case PdfPageSize.Letter:
                    _document.SetPageSize(PageSize.LETTER);
                    break;
                case PdfPageSize.Legal:
                    _document.SetPageSize(PageSize.LEGAL);
                    break;
                case PdfPageSize.Note:
                    _document.SetPageSize(PageSize.NOTE);
                    break;
                case PdfPageSize.PostCard:
                    _document.SetPageSize(PageSize.POSTCARD);
                    break;
                //Added By Samrat
                case PdfPageSize.A4Landscape:
                    _document.SetPageSize(PageSize.A4.Rotate());
                    break;
                case PdfPageSize.A3Landscape:
                    _document.SetPageSize(PageSize.A3.Rotate());
                    break;
                case PdfPageSize.LegalLandscape:
                    _document.SetPageSize(PageSize.LEGAL.Rotate());
                    break;
                default:
                    _document.SetPageSize(PageSize.A4);
                    break;
            }
        }

        public ReportingHelper(bool autoHeaderFooter = false, bool autoFooter = false)
        {
            _autoHeaderFooter = autoHeaderFooter;
            _autoFooter = autoFooter;
            _document = Document;
            _document.SetPageSize(PageSize.A4);
            if (_autoHeaderFooter || _autoFooter)
                _document.SetMargins(10f, 10f, 60f, 10f);
            else
                _document.SetMargins(10f, 10f, 10f, 10f);
        }

        public ReportingHelper(PdfPageSize pageSize, bool autoHeaderFooter = false, bool autoFooter = false)
        {
            _autoHeaderFooter = autoHeaderFooter;
            _autoFooter = autoFooter;
            _document = Document;
            SetPageSize(pageSize);
            if (_autoHeaderFooter || _autoFooter)
                _document.SetMargins(10f, 10f, 60f, 10f);
            else
                _document.SetMargins(10f, 10f, 10f, 10f);
        }

        public ReportingHelper(PdfPageSize pageSize, float margin, bool autoHeaderFooter = false, bool autoFooter = false)
        {
            _autoHeaderFooter = autoHeaderFooter;
            _autoFooter = autoFooter;
            _document = Document;
            SetPageSize(pageSize);
            _document.SetMargins(margin, margin, _autoHeaderFooter ? 60f : margin, margin);
        }

        public ReportingHelper(PdfPageSize pageSize, float marginTopBottom, float marginLeftRight, bool autoHeaderFooter = false, bool autoFooter = false)
        {
            _autoHeaderFooter = autoHeaderFooter;
            _autoFooter = autoFooter;
            _document = Document;
            SetPageSize(pageSize);
            _document.SetMargins(marginLeftRight, marginLeftRight, _autoHeaderFooter ? 60f : marginTopBottom, marginTopBottom);
        }

        public ReportingHelper(PdfPageSize pageSize, float marginTop, float marginBottom, float marginLeft, float marginRight, bool autoHeaderFooter = false, bool autoFooter = false)
        {
            _autoHeaderFooter = autoHeaderFooter;
            _autoFooter = autoFooter;
            _document = Document;
            SetPageSize(pageSize);
            _document.SetMargins(marginLeft, marginRight, _autoHeaderFooter ? 60f : marginTop, marginBottom);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _document.Dispose();
                }
                _disposed = true;
            }
        }

        ~ReportingHelper()
        {
            Dispose(false);
        }

        public bool GeneratePDF(string reportPath, string htmlString, string waterMark = "")
        {
            try
            {
                var Writer = PdfWriter.GetInstance(_document, new FileStream(reportPath, FileMode.Create));
                if (!string.IsNullOrEmpty(waterMark))
                {
                    Image jpg = iTextSharp.text.Image.GetInstance(waterMark);
                    jpg.ScaleToFit(400, 400);
                    jpg.SetAbsolutePosition(125, 300);
                    Writer.PageEvent = new ImageBackgroundHelper(jpg);
                }
                _document.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer: Writer, doc: _document, inp: new StringReader(htmlString));
                _returnValue = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (_document.IsOpen())
                    _document.Close();
            }
            if (_autoHeaderFooter)
            {
                PageTemplate.HeadOffice = this.HeadOffice;
                PageTemplate.HOAddress = this.HOAddress;
                PageTemplate.ZonalOffice = this.ZonalOffice;
                PageTemplate.ZOAddress = this.ZOAddress;
                PageTemplate.AddHeader(reportPath, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/reporticon.png"));
                if (this.Effectlogofile != null && this.Effectlogofile != "")
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                }
                else
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                }

            }
            else if (_autoFooter)
            {
                if (this.Effectlogofile != null && this.Effectlogofile != "")
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                }
                else
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                }

            }
            return _returnValue;
        }

        public bool GeneratePDF(string reportPath, string[] htmlString, string waterMark = "")
        {
            try
            {
                var Writer = PdfWriter.GetInstance(_document, new FileStream(reportPath, FileMode.Create));
                if (!string.IsNullOrEmpty(waterMark))
                {
                    Image jpg = Image.GetInstance(waterMark);
                    jpg.ScaleToFit(20f, 20f);
                    jpg.SetAbsolutePosition(0, 0);
                    Writer.PageEvent = new ImageBackgroundHelper(jpg);
                }
                //Writer.PageEvent = new PageTemplate();
                _document.Open();
                for (int i = 0; i < htmlString.Length; i++)
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer: Writer, doc: _document, inp: new StringReader(htmlString[i]));
                    _document.NewPage();
                }
                _returnValue = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (_document.IsOpen())
                    _document.Close();
            }
            if (_autoHeaderFooter)
            {
                PageTemplate.HeadOffice = this.HeadOffice;
                PageTemplate.HOAddress = this.HOAddress;
                PageTemplate.ZonalOffice = this.ZonalOffice;
                PageTemplate.ZOAddress = this.ZOAddress;
                PageTemplate.AddHeader(reportPath, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/reporticon.png"));
                if (this.Effectlogofile != null && this.Effectlogofile != "")
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                }
                else
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                }

            }
            else if (_autoFooter)
            {
                if (this.Effectlogofile != null && this.Effectlogofile != "")
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                }
                else
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                }

            }
            return _returnValue;
        }

        public bool GeneratePDF(string reportPath, List<string> htmlString, string waterMark = "")
        {
            try
            {
                var Writer = PdfWriter.GetInstance(_document, new FileStream(reportPath, FileMode.Create));
                if (!string.IsNullOrEmpty(waterMark))
                {
                    Image jpg = Image.GetInstance(waterMark);
                    jpg.ScaleToFit(20f, 20f);
                    jpg.SetAbsolutePosition(0, 0);
                    Writer.PageEvent = new ImageBackgroundHelper(jpg);
                }
                //Writer.PageEvent = new PageTemplate();
                _document.Open();
                foreach (string html in htmlString)
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer: Writer, doc: _document, inp: new StringReader(html));
                    _document.NewPage();
                }
                _returnValue = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (_document.IsOpen())
                    _document.Close();
            }
            if (_autoHeaderFooter)
            {
                PageTemplate.HeadOffice = this.HeadOffice;
                PageTemplate.HOAddress = this.HOAddress;
                PageTemplate.ZonalOffice = this.ZonalOffice;
                PageTemplate.ZOAddress = this.ZOAddress;
                PageTemplate.AddHeader(reportPath, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/reporticon.png"));
                if (this.Effectlogofile != null && this.Effectlogofile != "")
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                }
                else
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                }

            }
            else if (_autoFooter)
            {
                if (this.Effectlogofile != null && this.Effectlogofile != "")
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                }
                else
                {
                    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                }

            }
            return _returnValue;
        }

       
        public bool GeneratePDFWithoutFooter(string reportPath, string[] htmlString, string waterMark = "")
        {
            try
            {
                var Writer = PdfWriter.GetInstance(_document, new FileStream(reportPath, FileMode.Create));
                if (!string.IsNullOrEmpty(waterMark))
                {
                    Image jpg = Image.GetInstance(waterMark);
                    jpg.ScaleToFit(20f, 20f);
                    jpg.SetAbsolutePosition(0, 0);
                    Writer.PageEvent = new ImageBackgroundHelper(jpg);
                }
                //Writer.PageEvent = new PageTemplate();
                _document.Open();
                for (int i = 0; i < htmlString.Length; i++)
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer: Writer, doc: _document, inp: new StringReader(htmlString[i]));
                    _document.NewPage();
                }
                _returnValue = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (_document.IsOpen())
                    _document.Close();
            }
            if (_autoHeaderFooter)
            {
                PageTemplate.HeadOffice = this.HeadOffice;
                PageTemplate.HOAddress = this.HOAddress;
                PageTemplate.ZonalOffice = this.ZonalOffice;
                PageTemplate.ZOAddress = this.ZOAddress;
                //PageTemplate.AddHeader(reportPath, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/reporticon.png"));
                //if (this.Effectlogofile != null && this.Effectlogofile != "")
                //{
                //    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                //}
                //else
                //{
                //    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                //}

            }
            else if (_autoFooter)
            {
                //if (this.Effectlogofile != null && this.Effectlogofile != "")
                //{
                //    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/" + this.Effectlogofile + ""), this.Version.ToString());
                //}
                //else
                //{
                //    PageTemplate.AddFooter(reportPath, ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Name, "", "");
                //}

            }
            return _returnValue;
        }
        public string HeadOffice { get; set; } = "H O Not Available";
        public string HOAddress { get; set; } = "Address Line 1" + Environment.NewLine + "Address Line 2" + Environment.NewLine + "Address Line 3";
        public string ZonalOffice { get; set; } = "Z O Not Available";
        public string ZOAddress { get; set; } = "Address Line 1" + Environment.NewLine + "Address Line 2" + Environment.NewLine + "Address Line 3";

        public decimal Version { get; set; }
        public string Effectlogofile { get; set; }
    }

    public enum PdfPageSize
    {
        A3,
        A3Landscape,
        A4,
        A4Landscape,
        A5,
        B3,
        B4,
        B5,
        Letter,
        Legal,
        LegalLandscape,
        Note,
        PostCard
    }

    class ImageBackgroundHelper : PdfPageEventHelper
    {
        private Image img;
        public ImageBackgroundHelper(Image img)
        {
            this.img = img;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            writer.DirectContentUnder.AddImage(img);
        }
    }

    class PageTemplate
    {
        public static string HeadOffice { get; set; } = "H O Not Available";
        public static string HOAddress { get; set; } = "Address Line 1, Address Line 2, Address Line 3";
        public static string ZonalOffice { get; set; } = "Z O Not Available";
        public static string ZOAddress { get; set; } = "Address Line 1, Address Line 2, Address Line 3";

        public static void AddHeader(string reportPath, string logo)
        {
            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(reportPath);
                iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(bytes);
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        int pages = reader.NumberOfPages;
                        for (int i = 1; i <= pages; i++)
                        {
                            Image img = Image.GetInstance(logo);
                            if (reader.GetPageRotation(i) == 0)
                            {
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(HeadOffice, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK)), 40f, 825f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(HOAddress, blackFont), 40f, 815f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(ZonalOffice, FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)), 40f, 800f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(ZOAddress, blackFont), 40f, 790f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase(new Chunk(img, 130, 45, true)), 330f, 790f, 0);
                            }
                            else if (reader.GetPageRotation(i) == 90)
                            {
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(HeadOffice, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK)), 40f, 578f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(HOAddress, blackFont), 40f, 568f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(ZonalOffice, FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)), 40f, 553f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(ZOAddress, blackFont), 40f, 543f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase(new Chunk(img, 130, 45, true)), 577f, 543f, 0);
                            }
                        }
                    }

                    bytes = stream.ToArray();

                }
                System.IO.File.WriteAllBytes(reportPath, bytes);
            }
            catch (DocumentException exe)
            {
            }
        }

        public static void AddFooter(string reportPath, string createdBy, string logo, string Version)
        {
            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(reportPath);
                iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font redFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, BaseColor.RED);

                using (MemoryStream stream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(bytes);
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        int pages = reader.NumberOfPages;

                        for (int i = 1; i <= pages; i++)
                        {
                            Image img = null;

                            if (logo != "" && logo != null)
                            {
                                img = Image.GetInstance(logo);
                            }
                            //Image img = Image.GetInstance(logo);
                            if (reader.GetPageRotation(i) == 0)
                            {
                                if (img != null)
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(new Chunk(img, 30, 20, true)), 20f, 15f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(Version, redFont), 50f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 300f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 549f, 20f, 0);
                                }
                                else
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 300f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 549f, 20f, 0);
                                }

                            }
                            else if (reader.GetPageRotation(i) == 90)
                            {
                                if (img != null)
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(new Chunk(img, 30, 20, true)), 20f, 15f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(Version, redFont), 50f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 410f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 780f, 20f, 0);
                                }
                                else
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 410f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 780f, 20f, 0);
                                }

                            }
                        }
                    }

                    bytes = stream.ToArray();

                }
                System.IO.File.WriteAllBytes(reportPath, bytes);
            }
            catch (DocumentException exe)
            {
            }
        }

        public static void AddFooter(string reportPath, string createdBy)
        {
            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(reportPath);
                iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font redFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, BaseColor.RED);

                using (MemoryStream stream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(bytes);
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        int pages = reader.NumberOfPages;

                        for (int i = 1; i <= pages; i++)
                        {
                            Image img = null;
                            string logo = "";
                            if (logo != "" && logo != null)
                            {
                                img = Image.GetInstance(logo);
                            }
                            //Image img = Image.GetInstance(logo);
                            if (reader.GetPageRotation(i) == 0)
                            {
                                if (img != null)
                                {
                                    //ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(new Chunk(img, 30, 20, true)), 20f, 15f, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(Version, redFont), 50f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 300f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 549f, 20f, 0);
                                }
                                else
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 300f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 549f, 20f, 0);
                                }

                            }
                            else if (reader.GetPageRotation(i) == 90)
                            {
                                if (img != null)
                                {
                                    //ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(new Chunk(img, 30, 20, true)), 20f, 15f, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase(Version, redFont), 50f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 410f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 780f, 20f, 0);
                                }
                                else
                                {
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_LEFT, new Phrase("Printed By : " + createdBy, blackFont), 100f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_CENTER, new Phrase("Page " + i.ToString() + " of " + pages, blackFont), 410f, 20f, 0);
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), @Element.ALIGN_RIGHT, new Phrase("" + DateTime.Now, blackFont), 780f, 20f, 0);
                                }

                            }
                        }
                    }

                    bytes = stream.ToArray();

                }
                System.IO.File.WriteAllBytes(reportPath, bytes);
            }
            catch (DocumentException exe)
            {
            }
        }
    }
}