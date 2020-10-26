using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlasticStatic
{
    public class Enums
    {
        public class EnumValue : Attribute
        {
            private readonly string _value;
            public EnumValue(string value)
            {
                _value = value;
            }
            public string Value
            {
                get { return _value; }
            }
        }
        public static class EnumString
        {
            public static string GetStringValue(Enum value)
            {
                string output = null;
                Type type = value.GetType();
                FieldInfo fi = type.GetField(value.ToString());
                EnumValue[] attrs = fi.GetCustomAttributes(typeof(EnumValue), false) as EnumValue[];
                if (attrs != null && attrs.Length > 0)
                {
                    output = attrs[0].Value;
                }
                return output;
            }
        }
        public enum AspNetUserType
        {
            [EnumValue("1")]
            SuperAdmin = 1,
            [EnumValue("2")]
            Admin = 2,
            [EnumValue("3")]
            NormalUser = 3,
        }
        public enum RedirectPages
        {
            [EnumValue("~/Account/Login.aspx")]
            Login = 1,

        }
        public enum example
        {
            [EnumValue("1")] // 
            example1 = 1,

        }
        public enum demondOrderStatus
        {
            [EnumValue("1")]
            NotFinished = 0,
            [EnumValue("2")]
            current = 1,
            [EnumValue("3")]
            Finished = 2,
        }

        public enum job
        {
            [EnumValue("2")]
            Technician = 2, //فني

            [EnumValue ("3")]
            Storekeeper = 3 ,  // أمين مخزن

           [EnumValue("4")]
            Safekeeper = 4   // أمين خزينة

        }
        public enum jobDescription
        {
            [EnumValue("1")]
            CutingTechnician = 1, //فني تقطيع

            [EnumValue("2")]
            PrintingTechnician = 2,  //فني طباعه


        }

        public enum ToStockTypeId
        {
            [EnumValue("1")]
            PurchaseInvoice = 1, //فاتورة شراء
            [EnumValue("2")]
            SalesReturns = 2, // مرتجع مبيعات
            [EnumValue("3")]
            OperationReturns = 3, // مرتجع تشغيل
            [EnumValue("4")]
            FinishedItem = 4 ,//  تام
            [EnumValue("5")]
            ExpiredItem = 5 //تالف
        }

        public enum expiredtype
        {
            [EnumValue("1")]
            [Display(Name ="تالف طباعه")]
            printexpired = 1, // طباعه
            [EnumValue("2")]
            [Display(Name = "تالف تقطيع")]
            
            cutexpired = 2,// تثطيع
            [EnumValue("3")]
            [Display(Name = "تالف فيلم")]
            filmexpired = 3 // فيلم
        }
        public enum FromStockTypeId
        {
            [EnumValue("1")]
            OperationOrder = 1,// امر تشغيل
            [EnumValue("2")]
            sales = 2 ,//  sales
            [EnumValue("3")]
            [Display(Name = "مرتجع مشتريات")]
            purchesesreturn = 3 //  
        }
        public enum StockOperationType
        {
            [EnumValue("+")]
            Add = 1, // جمع الاصناف في المخرن
            [EnumValue("-")]
            Subtruct = 2, // طرح الاصناف في المخزن
        }
        public enum StoreType
        {
            [Display(Name = "خام")]

            [EnumValue("1")]
            Raw = 1, // الخام
            [EnumValue("2")]
            [Display(Name = "تام")]

            finished = 2 ,// تام
                  [EnumValue("3")]
            [Display(Name = "تالف")]

            expired = 3 // تالف
        }
        public enum ErrorMessageEnum
        {
            [EnumValue("1")]
            success = 1,

            [EnumValue("2")]
            danger = 2
        }
        public enum demondorderdetailstatus
        {
            [EnumValue("1")]
            Underconstruction = 1,

            [EnumValue("2")]
            Completed = 2,
            [EnumValue("3")]
            Delivery= 3,
            [EnumValue("4")]

            NotStarted=4,
            [EnumValue("5")]
            SaleingProcess=5,
            [EnumValue("6")]
            Duringdelivery= 6

        }

        public enum InvoiceStatus
        {
            [EnumValue("1")]
            [Display(Name = "تحت الانشاء")]
            NotFinish = 1,

            [Display(Name = "تم الانتهاء")]
            [EnumValue("2")]
            Finish = 2
        } 
        public enum InvoiceSSAlestatus
        {
            [EnumValue("1")]
            [Display(Name = "يتم التسجيل في حاسب العميل")]
            NotFinish = 1,

            [Display(Name = "يتم الصرف من المخزن")]
            [EnumValue("2")]
            Finish = 2
        }
        public enum ExpensDetailsStatus
        {
            [EnumValue("0")]
            [Display(Name = "تحت الانشاء")]
            NotFinish = 0,

            [Display(Name = "تم الانتهاء")]
            [EnumValue("1")]
            Finish = 1
        }


        public enum ExpensStatus
        {
            [EnumValue("0")]
            [Display(Name = "مصروفات ارباح وخسائر")]
            
            WinLoseExpens = 0,

            [Display(Name = "مصروفات اخري")]
            [EnumValue("1")]
            OtherExpinse = 1,
            [Display(Name = "لاتحسب")]
            [EnumValue("2")]
            NotCount = 2

        }



        public enum IncomeStatus
        {
            [Display(Name = "ايرادات اخري")]
            [EnumValue("0")]
            OtherIncome = 0,
            [Display(Name = "لاتحسب")]
            [EnumValue("1")]
            NotCount = 1

        }

        public enum IncomeDetailsStatus
        {
            [EnumValue("0")]
            [Display(Name = "تحت الانشاء")]
            NotFinish = 0,

            [Display(Name = "تم الانتهاء")]
            [EnumValue("1")]
            Finish = 1
        }  public enum cheque_status
        {
            [EnumValue("0")]
            [Display(Name = "مقبول")]
            accepted = 0,

            [Display(Name = "مرفوض")]
            [EnumValue("1")]
            Refused = 1 ,      [Display(Name = "قيد  التنفيذ")]
            [EnumValue("2")]
            under_execution = 2
        }

    }

    public static class Extensions
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
