using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class AMD_MergeSingleDeliAppCharged
    {
           public int IsFranchise { get; set; }
           public int IsOnWheel {get;set;} 
           public int IsReworking  {get;set;}
           public int IsCargoShifting {get;set;}
           public int IsDestuffing {get;set;}
           public int IsCargoDelivery {get;set;}
           public int IsLiftOnOff {get;set;}
           public int IsSweeping  {get;set;}
           public int IsHandling  {get;set;}
    }
}