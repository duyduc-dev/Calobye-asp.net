//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calobye.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ORDER_DETAILS
    {
        public string ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public int AMOUNT { get; set; }
        public decimal TOTAL { get; set; }
    
        public virtual PRODUCT PRODUCT { get; set; }
        public virtual THE_ORDER THE_ORDER { get; set; }
    }
}