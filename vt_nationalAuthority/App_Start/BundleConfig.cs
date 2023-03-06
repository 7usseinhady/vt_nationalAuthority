using System.Web;
using System.Web.Optimization;

namespace vt_nationalAuthority
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862


        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap-4.1.3/bootstrap.min.css",
        //              "~/Content/css/all.css",
        //              "~/Content/color101.css",
        //              "~/Content/hover.css",
        //              "~/Content/main101.css",
        //              "~/Content/Salert/sweetalert.min.css",
        //              "~/Content/pagination.css",
        //              "~/Content/font-family/Cairo-Changa-ElMessiri-Tajawal/display-swap/Cairo-Changa-ElMessiri-Tajawal.css"));

        //    bundles.Add(new StyleBundle("~/Content/cssLogin").Include(
        //            "~/Content/color101.css",
        //            "~/Content/main101.css",
        //            "~/Content/Salert/sweetalert.min.css"
        //            ));

        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                 "~/Scripts/jquery/jquery-3.3.1.js",
        //                 "~/Scripts/Salert/delete/sweetalert2@8.js",
        //                 "~/Scripts/Salert/sweetalert.min.js"
        //                 ));

        //    #region Date picker
        //    bundles.Add(new StyleBundle("~/bundles/datepicker").Include(
        //                   "~/Content/bootstrap-datepicker3/bootstrap-datepicker3.min.css"
        //                 ));
        //    bundles.Add(new ScriptBundle("~/bundles/datepicker.js").Include(
        //                "~/Scripts/bootstrap-datepicker/bootstrap-datepicker.min.js"
        //                ));
        //    #endregion

        //    #region Drop Down List 
        //    bundles.Add(new StyleBundle("~/bundles/select.css").Include(
        //                "~/Content/bootstrap-4.3.1-dist/bootstrap-select.min.css"
        //                ));
        //    bundles.Add(new ScriptBundle("~/bundles/select.js").Include(
        //                "~/Scripts/ajax/libs/popper.js/1.14.3/umd/popper.min.js",
        //                "~/Scripts/bootstrap-4.1.3/bootstrap.min.js",
        //                "~/Scripts/bootstrap-4.3.1-dist/bootstrap-select.min.js"
        //                ));
        //    #endregion

        //    #region Data Picker - Select Drop Down List
        //    bundles.Add(new ScriptBundle("~/bundles/datepickerSelect.js").Include(
        //              "~/Scripts/bootstrap-datepicker/bootstrap-datepicker.min.js",
        //              "~/Scripts/ajax/libs/popper.js/1.14.3/umd/popper.min.js",
        //              "~/Scripts/bootstrap-4.1.3/bootstrap.min.js",
        //              "~/Scripts/bootstrap-4.3.1-dist/bootstrap-select.min.js"
        //              ));
        //    bundles.Add(new StyleBundle("~/bundles/datepickerSelect.css").Include(
        //                  "~/Content/bootstrap-datepicker3/bootstrap-datepicker3.min.css",
        //                   "~/Content/bootstrap-4.3.1-dist/bootstrap-select.min.css"
        //                ));
        //    #endregion

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                  "~/Scripts/jquery.unobtrusive*")
        //    );

        //    #region chart
        //    bundles.Add(new ScriptBundle("~/bundles/chart").Include(
        //                  "~/Scripts/jquery/jquery-3.3.1.js",
        //                  "~/Scripts/Charts/Chart.bundle.min.js",
        //                  "~/Scripts/Charts/chartmain.js"
        //    ));
        //    #endregion

        //}

        public static void RegisterBundles(BundleCollection bundles)
        {

            //Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-4.1.3/bootstrap.min.css",
                      "~/Content/color101.css",
                      "~/Content/hover.css",
                      "~/Content/main101.css",
                      "~/Content/Salert/sweetalert.min.css",
                      "~/Content/pagination.css"));

            bundles.Add(new StyleBundle("~/Content/cssLogin").Include(
                    "~/Content/color101.css",
                    //"~/Content/hover.css",
                    "~/Content/main101.css",
                    "~/Content/Salert/sweetalert.min.css"
                    //,"~/Content/pagination.css"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/jquerymain").Include(

                    "~/Scripts/jquery/jquery-3.4.1.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery/jquery-3.3.1.js",
                         //"~/Scripts/jquery/jquery-3.4.1.min.js",
                         //"~/Scripts/jquery/jquery-3.3.1.js",
                         "~/Scripts/Salert/delete/sweetalert2@8.js",
                         //"~/Scripts/jquery.unobtrusive-ajax.js",
                         "~/Scripts/Salert/sweetalert.min.js"
                         ));

            // Date picker
            bundles.Add(new StyleBundle("~/bundles/datepicker").Include(
                           //"~/Content/bootstrap-datepicker3/bootstrap-datepicker.css",
                           "~/Content/bootstrap-datepicker3/bootstrap-datepicker3.min.css"
                         //"~/Content/bootstrap-datepicker3/bootstrap-datepicker3.standalone.css"
                         ));

            // Drop Down List 
            bundles.Add(new StyleBundle("~/bundles/select.css").Include(
                        "~/Content/bootstrap-4.3.1-dist/bootstrap-select.min.css"
                        //"~/Content/bootstrap-4.3.1-dist/bootstrap-select.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/select.js").Include(
                        "~/Scripts/bootstrap-4.3.1-dist/bootstrap-select.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*")
            );

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
         "~/Scripts/jquery/jquery-3.3.1.js",
         "~/Scripts/Charts/Chart.bundle.min.js",
         "~/Scripts/Charts/chartmain.js"
            ));

        }
    }
}