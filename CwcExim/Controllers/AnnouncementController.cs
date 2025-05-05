using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    public class AnnouncementController : BaseController
    {
        // GET: Announcement
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAnnouncement()
        {

            //ViewBag.CurrentDate= DateTime.Now;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditAnnounce(Announcement ObjAnnounce)
        {
            ModelState.Remove("AnnounceId");
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                AnnouncementRepository ObjRR = new AnnouncementRepository();

                if (Request["Command"] == "Save")
                {
                    ObjAnnounce.IsPublished = false;
                    ObjAnnounce.Uid = ObjLogin.Uid;
                    ObjAnnounce.Title= ObjAnnounce.Title.Trim();
                    ObjRR.AddEditAnnouncement(ObjAnnounce);
                }
                else if (Request["Command"] == "Publish")
                {
                    ObjAnnounce.IsPublished = true;
                    ObjAnnounce.Uid = ObjLogin.Uid;
                    ObjAnnounce.Title = ObjAnnounce.Title.Trim();
                    ObjRR.AddEditAnnouncement(ObjAnnounce);
                }
                else if (Request["Command"] == "Update")
                {
                    ObjAnnounce.Uid = ObjLogin.Uid;
                    ObjAnnounce.IsPublished = true;
                    ObjAnnounce.Title = ObjAnnounce.Title.Trim();
                    ObjRR.AddEditAnnouncement(ObjAnnounce);
                }
                return Json(ObjRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1, Message = "Please fill all the required details" };
                return Json(Err);
            }
        }

        public ActionResult CheckExistingAnnounce()
        {
            return View("AnnouncementList");
        }

        public ActionResult FilterAnnounce(bool IsActive, bool IsPortal, string Title = null)
        {
            AnnouncementRepository ObjAR = new AnnouncementRepository();
            Announcement ObjAnnounce = new Announcement();
            if(Title=="")
            {
                Title = null;
            }
            ObjAR.SearchAnnouncement(Title, IsActive, IsPortal);
            List<Announcement> LstAnnounce = new List<Announcement>();
            if (ObjAR.DBResponse.Data != null)
            {
                ObjAnnounce.LstAnnounce = (List<Announcement>)ObjAR.DBResponse.Data;
                ViewBag.IsData = true;
            }
            return View("FilteredAnnounceList", ObjAnnounce);
            //return Json(new { List = LstAnnounce, Count = LstAnnounce.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAnnounce(int AnnounceId)
        {
            AnnouncementRepository ObjRR = new AnnouncementRepository();
            Announcement ObjAnnounce = new Announcement();
            ObjRR.GetAnnouncementDet(AnnounceId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjAnnounce = (Announcement)ObjRR.DBResponse.Data;
            }
            return View("EditAnnouncement", ObjAnnounce);
        }

        [HttpGet, ChildActionOnly, OutputCache(Duration =1)]
        public ActionResult AnnouncementOnDashBoard()
        {
            List<Announcement> objAnnc = new List<Models.Announcement>();
            AnnouncementRepository objAR = new Repositories.AnnouncementRepository();
            objAR.ActiveAnnouncement();
            if (objAR.DBResponse.Data != null)
                objAnnc = (List<Announcement>)objAR.DBResponse.Data;
            return View("ActiveAnnouncement", objAnnc);
        }
    }
}

