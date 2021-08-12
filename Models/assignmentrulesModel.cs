using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollbarAutoAssignment.Models
{
    public class assignmentModel
    {

        public int project_id { get; set; }

        public string event_name { get; set; }

        public List<assignmentrulesModel> rules { get; set; }

        public string assign_to_username { get; set; }
    }

    public class assignmentrulesModel
    {

        public string field { get; set; }
        public string matchtype { get; set; }
        public string value { get; set; }

    }


}
