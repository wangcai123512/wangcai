using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    public class T_TaxSettlement
    {
        public string GUID { get; set;}

        public string Rep_date { get; set; }

        public string Rep_status { get; set; }
        /*Flag=Tax---增值税
         Flag=CT---企业所得税
         * Flag=YT---营业税金及附加
         */
        public string Flag { get; set; }
        public string State { get; set; }
        public string C_GUID { get; set; }

        /*Amount原始金额*/
        public Decimal Amount { get; set; }
        /*DisAmount未付金额*/
        public Decimal DisAmount { get; set; }

        //标志用户是通过流水，收入入口还是成本费用入口，或者是增值税录入入口
        public string Type { get; set; }
        
        //是否结转
        public string Is_end { get; set; }
        public DateTime Date{get;set;}

        public  decimal Excise{get;set;}
        public  decimal EducationFee {get;set;}
        public  decimal Sales{get;set;}
        public  decimal UrbanConstruction{get;set;}
        public  decimal Resource{get;set;}

        public  decimal LandValue{get;set;}
        public  decimal UrbanLand {get;set;}
        public  decimal Property{get;set;}
        public  decimal VehicleVessel{get;set;}
        public  decimal MineralResources{get;set;}
        public  decimal Dischargefee{get;set;}
        public string AccountID{get;set;}
        public string TaxName { get; set; }
	
    }
}
