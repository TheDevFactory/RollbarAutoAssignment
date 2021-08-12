using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollbarAutoAssignment.Models
{
    public class rollbarProjectAccessTokensModel
    {

        public int err { get; set; }
        public List<rollbarProjectTokensModel> result { get; set; }

    }

    public class rollbarProjectTokensModel 
    {
    
        public int project_id { get; set; }
        public string access_token { get; set; }
        public string status { get; set; }
        public string[] scopes { get; set; }
    
    }

    public class rollbarErrorModel 
    {

        public int err { get;  set; }
        public string message { get; set; }

    }


    public class rollbarUsers {

        public int err { get; set; }
        public rollbarUsersResults result { get; set; }

    }

    public class rollbarUsersResults {

        public List<rollbarUser> users { get; set; }

    }

    public class rollbarUser
    {

        public string username { get; set; }
        public int id { get; set; }
        public string email { get; set; }


    }



    public class rollbarEventModel
    {

        public string event_name { get; set; }
        public rollbarEventDataModel data { get; set; }

    }

    public class rollbarEventDataModel {

        public string url { get; set; }
        public rollbarEventDataItemModel item { get; set; }

    }

    public class rollbarEventDataItemModel {

        public int id { get; set; }
        public int project_id { get; set; }

        public string title { get; set; }
        public Nullable<int> assigned_user_id { get; set; }

        public rollbarEvenetDataItemOccurenceModel last_occurrence { get; set; }

    }

    public class rollbarEvenetDataItemOccurenceModel {

        public string root { get; set; }
        public string environment { get; set; }
        public string framework { get; set; }
        public string host { get; set; }
        public string language { get; set; }
        public string level { get; set; }
        public string context { get; set; }
        public rollbarEvenetDataItemOccurenceRequestModel request { get; set; }

    }

    public class rollbarEvenetDataItemOccurenceRequestModel
    {

        public string url { get; set; }
        public string query_string { get; set; }
        public string user_ip { get; set; }

    }

}
