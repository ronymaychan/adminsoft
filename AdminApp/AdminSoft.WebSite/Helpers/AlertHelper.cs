using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AdminSoft.WebSite.Helpers
{
    public static class AlertHelper
    {
        public static void BoostrapAlertSuccess(this Controller controller,string message, bool dismissable = true)
        {
            BoostrapAddAlert(controller, AlertStyles.Success, message, dismissable);
        }
        public static void BoostrapAlertInformation(this Controller controller, string message, bool dismissable = true)
        {
            BoostrapAddAlert(controller, AlertStyles.Information, message, dismissable);
        }
        public static void BoostrapAlertWarning(this Controller controller, string message, bool dismissable = true)
        {
            BoostrapAddAlert(controller, AlertStyles.Warning, message, dismissable);
        }
        public static void BoostrapAlertDanger(this Controller controller, string message, bool dismissable = true)
        {
            BoostrapAddAlert(controller, AlertStyles.Danger, message, dismissable);
        }
        private static void BoostrapAddAlert(Controller controller, string alertStyle, string message, bool dismissable)
        {
            var alerts = controller.TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)controller.TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            controller.TempData[Alert.TempDataKey] = alerts;
        }
        public static HtmlString BoostrapAlerts(this HtmlHelper html) {
            var tempData = html.ViewContext.Controller.TempData;

            var alerts = tempData.ContainsKey(Alert.TempDataKey) 
                ? (List<Alert>)tempData[Alert.TempDataKey] 
                : new List<Alert>();

            if (!alerts.Any())
                return new HtmlString(string.Empty);

            var sb = new StringBuilder();
            foreach (var alert in alerts)
                sb.Append(BoostrapAlert(alert.AlertStyle, alert.Message, alert.Dismissable));
            return new HtmlString(sb.ToString());
        }

        public static HtmlString BoostrapAlertSuccess(this HtmlHelper html, string message, bool dismissable = true) {
            
            return new MvcHtmlString(BoostrapAlert(AlertStyles.Success,message,dismissable));
        }
        public static HtmlString BoostrapAlertInformation(this HtmlHelper html, string message, bool dismissable = true)
        {

            return new MvcHtmlString(BoostrapAlert(AlertStyles.Information, message, dismissable));
        }
        public static HtmlString BoostrapAlertWarning(this HtmlHelper html, string message, bool dismissable = true)
        {

            return new MvcHtmlString(BoostrapAlert(AlertStyles.Warning, message, dismissable));
        }
        public static HtmlString BoostrapAlertDanger(this HtmlHelper html, string message, bool dismissable = true)
        {

            return new MvcHtmlString(BoostrapAlert(AlertStyles.Danger, message, dismissable));
        }
        private static string BoostrapAlert(string alertStyle, string message, bool dismissable = true)
        {
            var tagHtml = new StringBuilder();
            var dismissableClass = dismissable ? "alert-dismissable" : "";
            tagHtml.AppendFormat("<div class='alert alert-{0}", alertStyle);
            tagHtml.Append(dismissable ? " alert-dismissable'>" : "'>");
            if (dismissable) 
                tagHtml.Append("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>");
            tagHtml.Append(message);
            tagHtml.AppendFormat("</div>");
            return tagHtml.ToString();
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
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }
}