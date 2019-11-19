using Lab2_DB.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Database
{
    class DictionaryDAO
    {
        private DBConnection dbconnection;

        public DictionaryDAO(DBConnection db)
        {
            dbconnection = db;
        }

        public List<Room_type> GetRoom_Types()
        {
            List<Room_type> r_list = new List<Room_type>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.room_type";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Room_type r = new Room_type(Convert.ToInt64(reader.GetValue(0)), 
                                  reader.GetValue(1).ToString(), Convert.ToInt32(reader.GetValue(2)),
                                  Convert.ToDecimal(reader.GetValue(3)));
                r_list.Add(r);
            }
            dbconnection.Close();
            return r_list;
        }

        public List<Guest_status> GetGuest_status()
        {
            List<Guest_status> g_list = new List<Guest_status>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.guest_status";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Guest_status g = new Guest_status(Convert.ToInt64(reader.GetValue(0)),
                                                  reader.GetValue(1).ToString(),
                                                  Convert.ToInt32(reader.GetValue(2)));
                g_list.Add(g);
            }
            dbconnection.Close();
            return g_list;
        }
    }
}
