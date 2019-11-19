using Lab2_DB.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Database
{
    class DBConnection
    {
        private NpgsqlConnection connection
            = new NpgsqlConnection("Server=127.0.0.1; Port=5432; User Id=postgres; Password=123; Database=Hotel;");

        public NpgsqlConnection Open()
        {
            connection.Open();
            return connection;
        }

        public void Close()
        {
            connection.Close();
        }

        public void RandomDB(RoomDAO rd, GuestDAO gd, BookingDAO bd, DictionaryDAO dict)
        {
            RandomPackage randomPackage = new RandomPackage();

            List<long> roomTypesIndexes = new List<long>();
            List<Room_type> roomTypes = dict.GetRoom_Types();

            List<long> guestStatusesIndexes = new List<long>();
            List<Guest_status> guest_Statuses = dict.GetGuest_status();

            foreach (Room_type rt in roomTypes)
            {
                roomTypesIndexes.Add(rt.Id);
            }

            foreach (Guest_status gs in guest_Statuses)
            {
                guestStatusesIndexes.Add(gs.Id);
            }

            List<string> names = randomPackage.getFemalesNames();
            names.AddRange(randomPackage.getMalesNames());
            List<string> surnames = randomPackage.getSurnames();

            List<long> guestsIndexes = new List<long>();
            for (int i = 0; i < 20; i++)
            {
                string name = randomPackage.getRandomStringFromList(names);
                string surname = randomPackage.getRandomStringFromList(surnames);
                long status_id = randomPackage.getRandomNumberFromList(guestStatusesIndexes);
                Guest g = new Guest(-1, name, surname, status_id);
                gd.Create(g);
            }

            List<Guest> guests = gd.Get(0);
            guests.AddRange(gd.Get(1));

            foreach (Guest g in guests)
            {
                guestsIndexes.Add(g.Id);
            }

            for(int i = 0; i < 10000; i++)
            {
                int n = i;
                if (n > 1000) n /= 10;
                string number = n.ToString() + randomPackage.getRandomChar();
                long type_id = randomPackage.getRandomNumberFromList(roomTypesIndexes);
                Room r = new Room(-1, type_id, number, randomPackage.getRandomBoolean());
                rd.Create(r);
            }

            List<Room> rooms = rd.Get(0);
            foreach(Room r in rooms)
            {
                long g_id = randomPackage.getRandomNumberFromList(guestsIndexes);
                Booking b = new Booking(-1, g_id, r.Id, randomPackage.getRandomPastDate(), randomPackage.getRandomFutureDate());
                bd.Create(b);
            }
        }
    }
}
