using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Services.Description;

namespace StockPortal.ViewModel
{
    public class SecurityUserData
    {
        public int WorkerId { get; set; }

        public string UserId { get; set; }

        [DisplayName("إسم الموظف")]
        public string WorkerName { get; set; }

        [DisplayName("إسم الدخول")]
        public string UserName { get; set; }

        [DisplayName("البريد الإلكترونى")]
        public string Email { get; set; }

        [DisplayName("حالة الحساب")]
        public string AccountStatus { get; set; }

        [DisplayName("نوع المستخدم")]
        public string UserTypeName { get; set; }

         [DisplayName("كلمة المرور القديمة")]
        [Required(ErrorMessage = "* مطلوب")]
        public string OldPassword { get; set; }

         [DisplayName("كلمة المرور الجديدة")]
        [Required(ErrorMessage = "* مطلوب")]
        public string NewPassword { get; set; }

         [DisplayName("تأكيد كلمة المرور")]
         [Required(ErrorMessage = "* مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة غير مطابقة")]
        public string NewPasswordConfirm { get; set; }
    }

    public class objPass {
        public string UserId { get; set; }


        [DisplayName("كلمة المرور القديمة")]
        [Required(ErrorMessage = "* مطلوب")]
        public string OldPassword { get; set; }

        [DisplayName("كلمة المرور الجديدة")]
        [Required(ErrorMessage = "* مطلوب")]
        public string NewPassword { get; set; }

        [DisplayName("تأكيد كلمة المرور")]
        [Required(ErrorMessage = "* مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة غير مطابقة")]
        public string NewPasswordConfirm { get; set; }
    }
}