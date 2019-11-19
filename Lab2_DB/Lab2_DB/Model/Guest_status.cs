using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Model
{
    class Guest_status
    {
        private long id;
        private string status_name;
        private int required_visits;

        public Guest_status(long id, 
                            string status_name, 
                            int required_visits)
        {
            this.id = id;
            this.status_name = status_name;
            this.required_visits = required_visits;
        }

        public long Id { get => id; set => id = value; }
        public string Status_name { get => status_name; set => status_name = value; }
        public int Required_visits { get => required_visits; set => required_visits = value; }
    }
}
