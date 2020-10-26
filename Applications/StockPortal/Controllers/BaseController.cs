using System.Collections.Generic;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class BaseController : Controller
    {
        public void Notification(NotificationType msgType, bool dismissable = false)
        {
            var message = "";
            var alertStyle = "";
            switch (msgType)
            {
                case NotificationType.SaveSuccess:
                    message = "تم الحفظ بنجاح";
                    alertStyle = AlertStyles.Success;
                    break;
                case NotificationType.UpdateSuccess:
                    message = "تم التعديل بنجاح";
                    alertStyle = AlertStyles.Success;
                    break;
                case NotificationType.DeleteSuccess:
                    message = "تم الحذف بنجاح";
                    alertStyle = AlertStyles.Success;
                    break;
                case NotificationType.ResetSuccess:
                    message = "تم إعادة كلمة المرور بنجاح";
                    alertStyle = AlertStyles.Success;
                    break;
                case NotificationType.SearchSuccess:
                    message = "تم البحث بنجاح";
                    alertStyle = AlertStyles.Success;
                    break;
                case NotificationType.SaveError:
                    message = "توجد مشكلة أثناء تنفيذ الحفظ";
                    alertStyle = AlertStyles.Danger;
                    break;
                case NotificationType.UpdateError:
                    message = "توجد مشكلة أثناء تنفيذ التعديل";
                    alertStyle = AlertStyles.Danger;
                    break;
                case NotificationType.DeleteError:
                    message = "توجد مشكلة أثناء تنفيذ الحذف";
                    alertStyle = AlertStyles.Danger;
                    break;
                case NotificationType.ResetError:
                    message = "توجد مشكلة أثناء تنفيذ إعادة كلمة المرور";
                    alertStyle = AlertStyles.Danger;
                    break;
                case NotificationType.SearchError:
                    message = "توجد مشكلة أثناء تنفيذ البحث";
                    alertStyle = AlertStyles.Danger;
                    break;
                case NotificationType.SaveInformation:
                    message = "البيان غير متوفر لا يمكن تنفيذ عملية الحفظ";
                    alertStyle = AlertStyles.Information;
                    break;
                case NotificationType.UpdateInformation:
                    message = "البيان غير متوفر لا يمكن تنفيذ عملية التعديل";
                    alertStyle = AlertStyles.Information;
                    break;
                case NotificationType.DeleteInformation:
                    message = "البيان غير متوفر لا يمكن تنفيذ عملية الحذف";
                    alertStyle = AlertStyles.Information;
                    break;
                case NotificationType.ResetInformation:
                    message = "البيان غير متوفر لا يمكن تنفيذ عملية إعادة كلمة المرور";
                    alertStyle = AlertStyles.Information;
                    break;
                case NotificationType.SearchInformation:
                    message = "البيان غير متوفر لا يمكن تنفيذ عملية البحث";
                    alertStyle = AlertStyles.Information;
                    break;
                case NotificationType.DeletePurchaseInvoice:
                    message = "لايمكن حذف هذه الفاتورة";
                    alertStyle = AlertStyles.Danger;
                    break;
            }

            AddAlert(alertStyle, message, dismissable);
        }
        public void NotificationWithMessage(string inputMessage, bool dismissable = false)
        {
            var message = inputMessage;
            var alertStyle = AlertStyles.Information;

            if (message.Contains("Incorrect password"))
            {
                message = "كلمة المرور القديمة غير صحيحة";
                alertStyle = AlertStyles.Danger;
            }

            if (message.Contains("Passwords must be at least 6 characters"))
            {
                message = "كلمة المرور لابد أن تكون 6 احرف على الأقل وتحتوى على حروف وارقام والحروف تكون كبيرة وصغيرة";
                alertStyle = AlertStyles.Danger;
            }
            AddAlert(alertStyle, message, dismissable);
        }
        public void NotificationWithMessage(string alertStyle, string inputMessage, bool dismissable = false)
        {
            var message = inputMessage;


            if (message.Contains("Incorrect password"))
            {
                message = "كلمة المرور القديمة غير صحيحة";
                alertStyle = AlertStyles.Danger;
            }

            if (message.Contains("Passwords must be at least 6 characters"))
            {
                message = "كلمة المرور لابد أن تكون 6 احرف على الأقل وتحتوى على حروف وارقام والحروف تكون كبيرة وصغيرة";
                alertStyle = AlertStyles.Danger;
            }
            AddAlert(alertStyle, message, dismissable);
        }


        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }
    }

    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Error = "error";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }

    public enum NotificationType
    {
        SaveSuccess,
        UpdateSuccess,
        DeleteSuccess,
        ResetSuccess,
        SearchSuccess,
        SaveError,
        UpdateError,
        DeleteError,
        ResetError,
        SearchError,
        SaveInformation,
        UpdateInformation,
        DeleteInformation,
        ResetInformation,
        SearchInformation,
        DeletePurchaseInvoice

    }

}
