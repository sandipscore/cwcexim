using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class VRN_ListOfCustAppraisementAppr
    {
        
            public int CstmAppraiseAppId { get; set; }
            public string AppraisementNo { get; set; }
            public string BOENo { get; set; }
            public string CHA { get; set; }
            public string Importer { get; set; }
        }

        public class VRN_Menu
        {
            public int CanAdd { get; set; }
            public int CanEdit { get; set; }
            public int CanDelete { get; set; }
            public int CanView { get; set; }


        }
    }
