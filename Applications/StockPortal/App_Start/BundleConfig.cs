using System.Web;
using System.Web.Optimization;

namespace StockPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/jquery-ui.js"
              //new
              //"~/Scripts/jquery-ui-1.10.4.custom.js",
              //"~/Scripts/jquery-ui-1.10.4.custom.min.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/jquery").Include(
                "~/Content/jquery-ui.css"
            //new 
            // "~/Content/jquery-ui-1.10.4.custom.css"

            ));

            bundles.Add(new ScriptBundle("~/Content/chosenJavaScript").Include(
                "~/Scripts/MultipleChosen/chosen.jquery.js",
                "~/Scripts/MultipleChosen/docsupport/prism.js"
            ));
            bundles.Add(new StyleBundle("~/Content/chosenCSS").Include(
                "~/Scripts/MultipleChosen/docsupport/style.css",
                "~/Scripts/MultipleChosen/docsupport/prism.cs",
                "~/Scripts/MultipleChosen/chosen.css"
            ));

            bundles.Add(new StyleBundle("~/Content/DateTimeStyle").Include("~/Content/jquery-ui-timepicker-addon.min.css"));
            bundles.Add(new ScriptBundle("~/Content/DateTimeScript").Include("~/Scripts/jquery-ui-timepicker-addon.min.js"));

        }
    }
}
