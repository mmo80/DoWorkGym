using System.Web.Optimization;

namespace DoWorkGym.WebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/misc").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-*",
                        "~/Scripts/knockout.mapping*",
                        "~/Scripts/bootstrap*",
                        "~/Scripts/underscore*",
                        "~/Scripts/views/dowork.base.js",
                        "~/Scripts/jquery.bootstrap-growl.custom.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/plugins/jquery*",
                        "~/Scripts/plugins/calendar/language/sv-SE.js",
                        "~/Scripts/plugins/calendar/calendar*"
                        ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/account").Include("~/Scripts/views/dowork.view.account.js"));
            bundles.Add(new ScriptBundle("~/bundles/training").Include("~/Scripts/views/dowork.view.training.js"));
            bundles.Add(new ScriptBundle("~/bundles/exercise").Include("~/Scripts/views/dowork.view.exercise.js"));
            bundles.Add(new ScriptBundle("~/bundles/workout").Include("~/Scripts/views/dowork.view.workout.js"));
            bundles.Add(new ScriptBundle("~/bundles/workoutdetail").Include("~/Scripts/views/dowork.view.workoutdetail.js"));
            bundles.Add(new ScriptBundle("~/bundles/history").Include("~/Scripts/views/dowork.view.history.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/headjs").Include(
                        "~/Scripts/html5shiv.js",
                        "~/Scripts/responde*"
                        ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        //"~/Content/bootstrap*",
                        "~/Content/theme/yeti/bootstrap*",
                        "~/Content/theme/google-plus/gplus-bootstrap.css",
                        "~/Scripts/plugins/calendar/css/calendar*",
                        "~/Content/dowork.css"
                        ));
        }
    }
}