using System.Web;
using System.Web.Optimization;

namespace CwcExim
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Clear();
            bundles.ResetAll();
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                          //"~/Scripts/jquery-{version}.js", "~/Scripts/jquery-ui-1.10.4.js",
                          "~/Scripts/jquery-3.6.1.min.js", "~/Scripts/jquery-ui.js", //For Audit Change
                       "~/Scripts/jquery.marquee.min.js"));


            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
"~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.min.js"
));
            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
            "~/Scripts/jquery.unobtrusive-ajax.min.js"
           ));
            //bundles.Add(new ScriptBundle("~/bundles/AjaxOption").Include(
            //           "~/ScriptsAjax/jquery-1.7.1.min.js", "~/ScriptsAjax/jquery.unobtrusive-ajax.min.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/Design.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.bootstrap.min.js",
                      "~/Scripts/dataTables.fixedHeader.min.js",
                      "~/Scripts/dataTables.responsive.min.js",
                      "~/Scripts/responsive.bootstrap.min.js",
                      "~/Scripts/ColReorderWithResize.js",
                      "~/Scripts/bootstrap-clockpicker.min.js",
                      "~/Scripts/DynamicYear.js")); //,"~/Scripts/timepicker_slider.js"


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/EximStyle.css",
                      "~/Content/Landing_page.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css",
                      "~/Content/dataTables.bootstrap.min.css",
                      "~/Content/fixedHeader.bootstrap.min.css",
                       "~/Content/responsive.bootstrap.min.css",
                       "~/Content/jquery-ui.css",
                        "~/Content/bootstrap-clockpicker.min.css"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/Audit").Include(
                           "~/Scripts/Audit/Main.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/AuditParaEntry").Include(
               "~/Scripts/Audit/AddAuditPara.js",
               "~/Scripts/Audit/ViewAuditPara.js",
                "~/Scripts/Audit/AuditParaList.js",
                 "~/Scripts/Audit/AttachmentList.js",
                 "~/Scripts/Audit/EditAuditPara.js",
               "~/Scripts/Audit/EditAttachmentList.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/AuditGnrlDiscuss").Include(
            "~/Scripts/Audit/AddAuditDiscuss.js",
            "~/Scripts/Audit/ViewAuditDiscuss.js",
             "~/Scripts/Audit/AuditDiscussList.js",
              "~/Scripts/Audit/EditAuditDiscuss.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/AuditInfo").Include(
             "~/Scripts/Audit/AddAuditInfo.js",
             "~/Scripts/Audit/ViewAuditInfo.js",
              "~/Scripts/Audit/AuditInfoList.js",
               "~/Scripts/Audit/EditAuditInfo.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/AuditBroadSheet").Include(
            "~/Scripts/Audit/AddBroadSheet.js",
            "~/Scripts/Audit/ViewBroadSheet.js",
             "~/Scripts/Audit/BroadSheetList.js",
              "~/Scripts/Audit/EditBroadSheet.js",
              "~/Scripts/Audit/BSAttachmentList.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/AuditBroadSheetApproval").Include(
          "~/Scripts/Audit/AddAuditApproval.js",
          "~/Scripts/Audit/ViewAuditApproval.js",
           "~/Scripts/Audit/AuditApprovalList.js",
            "~/Scripts/Audit/AuditApprovalFileList.js",
            "~/Scripts/Audit/EditAuditApproval.js"
          ));

            bundles.Add(new ScriptBundle("~/bundles/AuditBroadSheetMail").Include(
        "~/Scripts/Audit/AddAuditMail.js",
        "~/Scripts/Audit/ViewAuditMail.js",
         "~/Scripts/Audit/AuditMailList.js",
                "~/Scripts/Audit/AuditMailFileList.js",
          "~/Scripts/Audit/EditAuditMail.js"
        ));

            bundles.Add(new ScriptBundle("~/bundles/CourtCase").Include(
                              "~/Scripts/CourtCase/Main.js"
                           ));

            bundles.Add(new ScriptBundle("~/bundles/CCWritPetition").Include(
                "~/Scripts/CourtCase/AddWritPetition.js",
                "~/Scripts/CourtCase/ViewWritPetition.js",
                 "~/Scripts/CourtCase/WritPetitionList.js",
                 "~/Scripts/CourtCase/AttachmentList.js",
                  "~/Scripts/CourtCase/EditWritPetition.js",
                "~/Scripts/CourtCase/EditAttachmentList.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/AdvctAppoIntimation").Include(
             "~/Scripts/CourtCase/AddIntimation.js",
             "~/Scripts/CourtCase/ViewIntimation.js",
              "~/Scripts/CourtCase/IntimationList.js",
               "~/Scripts/CourtCase/EditIntimation.js"
             ));


            bundles.Add(new ScriptBundle("~/bundles/AdvctAppoDtl").Include(
            "~/Scripts/CourtCase/AddAppointment.js",
            "~/Scripts/CourtCase/ViewAppointment.js",
             "~/Scripts/CourtCase/AppointmentList.js",
              "~/Scripts/CourtCase/EditAppointment.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/CertificateCopyApp").Include(
            "~/Scripts/CourtCase/AddCertificate.js",
            "~/Scripts/CourtCase/ViewCertificate.js",
             "~/Scripts/CourtCase/CertificateList.js",
              "~/Scripts/CourtCase/EditCertificate.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/CertificateCopyRcvd").Include(
           "~/Scripts/CourtCase/AddCertificateRec.js",
           "~/Scripts/CourtCase/ViewCertificateRec.js",
            "~/Scripts/CourtCase/CertificateRecList.js",
             "~/Scripts/CourtCase/EditCertificateRec.js",
             "~/Scripts/CourtCase/CertificateRecAttachment.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/IntimationToFinDept").Include(
          "~/Scripts/CourtCase/AddIntimationToFinDept.js",
          "~/Scripts/CourtCase/ViewIntimationToFinDept.js",
           "~/Scripts/CourtCase/IntimationToFinDeptList.js",
            "~/Scripts/CourtCase/EditIntimationToFinDept.js"
          ));


            bundles.Add(new ScriptBundle("~/bundles/PetitionAppeal").Include(
          "~/Scripts/CourtCase/AddPetitionAppeal.js",
          "~/Scripts/CourtCase/ViewPetitionAppeal.js",
           "~/Scripts/CourtCase/PetitionAppealList.js",
            "~/Scripts/CourtCase/EditPetitionAppeal.js"
          ));

            bundles.Add(new ScriptBundle("~/bundles/Faq").Include(
                 "~/Scripts/Faq/AddFaq.js",
                 "~/Scripts/Faq/EditFaq.js",
                  "~/Scripts/Faq/FaqList.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/AdminGrievance").Include(
                 "~/Scripts/Grievance/EditGrievance.js",
                  "~/Scripts/Grievance/GrievanceList.js",
                   "~/Scripts/Grievance/ViewGrievance.js"
                 ));


            bundles.Add(new ScriptBundle("~/bundles/UserGrievance").Include(
                 "~/Scripts/Grievance/AddRegisterComplaint.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/NoDueCertificate").Include(
                          "~/Scripts/NoDueCertificate/NoDueCertificateApp.js",
                          "~/Scripts/NoDueCertificate/EditNoDueCertificateApp.js",
                          "~/Scripts/NoDueCertificate/GetWbdedPendingNodueCert.js",
                          "~/Scripts/NoDueCertificate/SaveNoDueCertPhyVerifyRpt.js",
                          "~/Scripts/NoDueCertificate/NotificationForNoDuePayment.js",
                          "~/Scripts/NoDueCertificate/IssueNoDueCert.js"
                       ));
        }
    }
}
