using System.Collections.Generic;

namespace DoWorkGym.WebApp.ViewModels
{
    namespace HistoryViewModels
    {
        public class HistoryListView
        {
            public string Date { get; set; }
            public string DateMillisecs { get; set; }
            public List<HistoryItemDateRow> DateRows { get; set; }
        }
        public class HistoryItemDateRow
        {
            public string Workout { get; set; }
            public string Exercise { get; set; }
        }



        public class Result
        {
            public string id { get; set; }
            public string title { get; set; }
            public string url { get; set; }
            public string @class { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }
        public class RootObject
        {
            public int Success { get; set; }
            public List<Result> Result { get; set; }
        }
    }
}