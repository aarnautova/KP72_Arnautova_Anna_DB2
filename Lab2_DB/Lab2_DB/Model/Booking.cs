using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Model
{
    class Booking
    {
        private long id;
        private long guest_id;
        private long room_id;
        private DateTime arrival;
        private DateTime departure;

        private Guest guest;
        private Room room;

        public Booking(long id, long guest_id,
                       long room_id,
                       DateTime arrival,
                       DateTime departure)
        {
            this.id = id;
            this.guest_id = guest_id;
            this.room_id = room_id;
            this.arrival = arrival;
            this.departure = departure;
        }

        public Booking(long id, long guest_id, 
                       long room_id, 
                       DateTime arrival, 
                       DateTime departure, 
                       long type_id,
                       string number,
                       bool ocean_view,
                       string type_name,
                       int bed_count,
                       decimal price,
                       string name,
                       string surname,
                       long status_id,
                       string status_name,
                       int required_visits)
        {
            this.id = id;
            this.guest_id = guest_id;
            this.room_id = room_id;
            this.arrival = arrival;
            this.departure = departure;
            this.guest = new Guest(guest_id, name, surname, status_id, status_name, required_visits);
            this.room = new Room(room_id, type_id, number, ocean_view, type_name, bed_count, price);
        }

        public long Id { get => id; set => id = value; }
        public long Guest_id { get => guest_id; set => guest_id = value; }
        public long Room_id { get => room_id; set => room_id = value; }
        public DateTime Arrival { get => arrival; set => arrival = value; }
        public DateTime Departure { get => departure; set => departure = value; }
        internal Guest Guest { get => guest; set => guest = value; }
        internal Room Room { get => room; set => room = value; }
    }
}
