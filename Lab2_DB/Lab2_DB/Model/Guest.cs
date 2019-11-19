using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Model
{
    class Guest
    {
        private long id;
        private string name;
        private string surname;
        private long status_id;

        private Guest_status status;

        public Guest(long id,
                     string name,
                     string surname,
                     long status_id)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.status_id = status_id;
        }
        public Guest(long id, 
                     string name, 
                     string surname, 
                     long status_id, 
                     string status_name,
                     int required_visits)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.status_id = status_id;
            this.status = new Guest_status(status_id, status_name, required_visits);
        }

        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public long Status_id { get => status_id; set => status_id = value; }
        internal Guest_status Status { get => status; set => status = value; }
    }
}
