//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DDAC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        public int ScheduleID { get; set; }
        public string Destination { get; set; }
        public Nullable<System.DateTime> DepartureDateTime { get; set; }
        public Nullable<System.DateTime> ArrivalDateTime { get; set; }
        public Nullable<int> ShipID { get; set; }
    
        public virtual Ship Ship { get; set; }
    }
}
