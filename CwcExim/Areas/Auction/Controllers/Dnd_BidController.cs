using CwcExim.Areas.Auction.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Auction.Controllers
{
    public class Dnd_BidController : BaseController
    {
        // GET: Auction/Auction

        #region Auction Notice
        public ActionResult BidFinalization()
        {
            Dnd_AuctionRepository _Ar = new Dnd_AuctionRepository();
            List<Dnd_PartyDetails> lstPartyDetails = new List<Dnd_PartyDetails>();
            _Ar.GetBidPartyAndDetails();
            List<Dnd_AucBidFinalizationDtl> lstRefNo = new List<Dnd_AucBidFinalizationDtl>();

            if (_Ar.DBResponse.Data != null)
            {
                lstPartyDetails = (List<Dnd_PartyDetails>)_Ar.DBResponse.Data;
            }
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            _Ar.GetListShippBillOBLForBid(2);
            if (_Ar.DBResponse.Data != null)
            {
                lstRefNo = (List<Dnd_AucBidFinalizationDtl>)_Ar.DBResponse.Data;
            }

            ViewBag.PartyDetails = Newtonsoft.Json.JsonConvert.SerializeObject(lstPartyDetails);
            ViewBag.RefNoList= Newtonsoft.Json.JsonConvert.SerializeObject(lstRefNo);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BidFinalizationSave(Dnd_AucBidFinalizationHdr obj)
        {
            if (ModelState.IsValid)
            {
                int Uid = Convert.ToInt32(((Login)(Session["LoginUser"])).Uid);
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (obj.Type == "OBL")
                {
                    obj.RefFlag = 1;
                }
                else if(obj.Type == "Shipbill")
                {
                    obj.RefFlag = 2;
                }
                else if(obj.Type == "CONTAINER")
                {
                    obj.RefFlag = 3;
                }

                //var formated= Newtonsoft.Json.JsonConvert.DeserializeObject<IList<AucBidFinalizationDtl>>(obj.StrTableDtls);
                //string XML = Utility.CreateXML(formated);
                //obj.StrTableDtls = XML;
                Dnd_AuctionRepository _Ar = new Dnd_AuctionRepository();
                _Ar.SaveBid(obj, Uid, BranchId);
                return Json(_Ar.DBResponse);
            }
            return Json("error");
        }


        [HttpGet]
        public ActionResult BidFinalizationList(int id)
        {
           

            Dnd_AuctionRepository _ar = new Dnd_AuctionRepository();
            List<Dnd_AucBidFinalizationHdr> LstAucBidFinalizationHdr = new List<Dnd_AucBidFinalizationHdr>();
            _ar.GetBidLIst(id);
            if (_ar.DBResponse.Data != null)
            {
                LstAucBidFinalizationHdr = (List<Dnd_AucBidFinalizationHdr>)_ar.DBResponse.Data;
            }
            return PartialView("BidFinalizationList", LstAucBidFinalizationHdr);
        }


        public JsonResult GetDataByFlag(int Flag)//,int PartyID =0
        {
            Dnd_AuctionRepository obj = new Dnd_AuctionRepository();
            obj.GetListShippBillOBLForBid(Flag);
            List<Dnd_AucBidFinalizationDtl> lstRefNo = new List<Dnd_AucBidFinalizationDtl>();
            if (obj.DBResponse.Data != null)
            {
                lstRefNo = (List<Dnd_AucBidFinalizationDtl>)obj.DBResponse.Data;
            }
            return Json(lstRefNo, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetBidDataByID(int id)
        {
            Dnd_AuctionRepository obj = new Dnd_AuctionRepository();
            obj.GetBidLIst(id);
            List<Dnd_AucBidFinalizationHdr> LstAucBidFinalizationHdr = new List<Dnd_AucBidFinalizationHdr>();
            if (obj.DBResponse.Data != null)
            {
                LstAucBidFinalizationHdr = (List<Dnd_AucBidFinalizationHdr>)obj.DBResponse.Data;
            }
            return Json(LstAucBidFinalizationHdr, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteBidDataByID(int id)
        {
            Dnd_AuctionRepository obj = new Dnd_AuctionRepository();
            obj.DeleteBid(id);
          
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }



        #endregion


    }
}