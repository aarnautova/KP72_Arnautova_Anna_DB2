using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Model
{
    class Room
    {
        private long id;
        private long type_id;
        private string number;
        private bool ocean_view;

        private Room_type type;

        public Room(long id, long type_id,
                    string number,
                    bool ocean_view)
        {
            this.id = id;
            this.type_id = type_id;
            this.number = number;
            this.ocean_view = ocean_view;
        }

        public Room(long id, long type_id, 
                    string number,
                    bool ocean_view,
                    string type_name,
                    int bed_count,
                    decimal price)
        {
            this.id = id;
            this.type_id = type_id;
            this.number = number;
            this.ocean_view = ocean_view;
            this.type = new Room_type(type_id, type_name, bed_count, price);
        }

        public long Id { get => id; set => id = value; }
        public long Type_id { get => type_id; set => type_id = value; }
        public string Number { get => number; set => number = value; }
        public Room_type Type { get => type; set => type = value; }
        public bool Ocean_view { get => ocean_view; set => ocean_view = value; }
    }
}
