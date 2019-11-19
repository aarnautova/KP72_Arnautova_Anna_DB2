using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Model
{
    class Room_type
    {
        private long id;
        private string type_name;
        private int bed_count;
        private decimal price;

        public Room_type(long id, 
                        string type_name, 
                        int bed_count, 
                        decimal price)
        {
            this.id = id;
            this.type_name = type_name;
            this.bed_count = bed_count;
            this.price = price;
        }

        public long Id { get => id; set => id = value; }
        public string Type_name { get => type_name; set => type_name = value; }
        public int Bed_count { get => bed_count; set => bed_count = value; }
        public decimal Price { get => price; set => price = value; }
    }
}
