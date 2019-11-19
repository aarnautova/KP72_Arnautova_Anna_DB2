using Lab2_DB.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Database
{
    class BookingDAO : DAO<Booking>
    {

        public BookingDAO(DBConnection db) : base(db) { }

        private bool dateCheck(Booking entity, bool selfCheck)
        {
            if (entity.Arrival >= entity.Departure) throw new Exception("Arrival can't be bigger than departure.");
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = selfCheck ? 
                                "SELECT arrival, departure from public.booking WHERE room_id = :r_id" 
                                : "SELECT arrival, departure from public.booking WHERE room_id = :r_id";
            command.Parameters.Add(new NpgsqlParameter("r_id", entity.Room_id));
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DateTime arrival = Convert.ToDateTime(reader.GetValue(0));
                DateTime departure = Convert.ToDateTime(reader.GetValue(1));
                if (entity.Arrival >= arrival && entity.Arrival < departure
                    || entity.Departure <= departure && entity.Departure > arrival)
                    return false;
            }
            dbconnection.Close();
            return true;
        }
        public override void Create(Booking entity)
        {
            if(!dateCheck(entity, false))  throw new Exception("Wrong date");
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO public.booking (guest_id, room_id, arrival, departure) VALUES (:guest_id, :room_id, :arrival, :departure)";
            command.Parameters.Add(new NpgsqlParameter("guest_id", entity.Guest_id));
            command.Parameters.Add(new NpgsqlParameter("room_id", entity.Room_id));
            command.Parameters.Add(new NpgsqlParameter("arrival", entity.Arrival));
            command.Parameters.Add(new NpgsqlParameter("departure", entity.Departure));
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
            }
            catch (PostgresException)
            {
                throw new Exception("Unable to create new booking");
            }
            dbconnection.Close();
        }

        public override void Delete(long id)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM public.booking WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();
            dbconnection.Close();
        }

        public override Booking Get(long id)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from public.booking b inner join public.room rm on b.room_id = rm.id inner join public.room_type rt on rm.type_id = rt.id " +
                                   "inner join public.guest g on b.guest_id = g.id  inner join public.guest_status gs on g.status_id = gs.id WHERE b.id = :booking_id";
            command.Parameters.Add(new NpgsqlParameter("booking_id", id));
            NpgsqlDataReader reader = command.ExecuteReader();
            Booking b = null;
            while (reader.Read())
            {
                b = new Booking(Convert.ToInt64(reader.GetValue(0)), Convert.ToInt64(reader.GetValue(1)),
                                        Convert.ToInt64(reader.GetValue(2)), Convert.ToDateTime(reader.GetValue(3)),
                                        Convert.ToDateTime(reader.GetValue(4)),
                                        Convert.ToInt64(reader.GetValue(6)), reader.GetValue(7).ToString(), Convert.ToBoolean(reader.GetValue(8)),
                                        reader.GetValue(10).ToString(), Convert.ToInt32(reader.GetValue(11)), Convert.ToDecimal(reader.GetValue(12)),
                                        reader.GetValue(14).ToString(), reader.GetValue(15).ToString(), Convert.ToInt64(reader.GetValue(16)),
                                        reader.GetValue(18).ToString(), Convert.ToInt32(reader.GetValue(19)));
            }
            dbconnection.Close();
            return b;
        }

        public override List<Booking> Get(int page)
        {
            List<Booking> b_list = new List<Booking>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from public.booking b inner join public.room rm on b.room_id = rm.id inner join public.room_type rt on rm.type_id = rt.id " +
                                   "inner join public.guest g on b.guest_id = g.id  inner join public.guest_status gs on g.status_id = gs.id" +
                " LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page*10));
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Booking b = new Booking(Convert.ToInt64(reader.GetValue(0)), Convert.ToInt64(reader.GetValue(1)),
                                        Convert.ToInt64(reader.GetValue(2)), Convert.ToDateTime(reader.GetValue(3)),
                                        Convert.ToDateTime(reader.GetValue(4)),
                                        Convert.ToInt64(reader.GetValue(6)), reader.GetValue(7).ToString(), Convert.ToBoolean(reader.GetValue(8)),
                                        reader.GetValue(10).ToString(), Convert.ToInt32(reader.GetValue(11)), Convert.ToDecimal(reader.GetValue(12)),
                                        reader.GetValue(14).ToString(), reader.GetValue(15).ToString(), Convert.ToInt64(reader.GetValue(16)),
                                        reader.GetValue(18).ToString(), Convert.ToInt32(reader.GetValue(19)));

                b_list.Add(b);
            }
            dbconnection.Close();
            return b_list;
        }

        public override void Update(Booking entity)
        {
            if (!dateCheck(entity, true)) throw new Exception("Wrong date");
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE public.booking SET arrival = :arrival, departure = :departure WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", entity.Id));
            command.Parameters.Add(new NpgsqlParameter("arrival", entity.Arrival));
            command.Parameters.Add(new NpgsqlParameter("departure", entity.Departure));
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
            }
            catch (PostgresException)
            {
                throw;
            }
            dbconnection.Close();
        }

        public List<Booking> StaticSearch(DateTime d1, DateTime d2, bool expected_ocean_view)
        {
            if (d1 >= d2) throw new Exception("Wrong date diapason");
            List<Booking> b_list = new List<Booking>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from public.booking b inner join public.room rm on b.room_id = rm.id inner join public.room_type rt on rm.type_id = rt.id " +
                                   "inner join public.guest g on b.guest_id = g.id  inner join public.guest_status gs on g.status_id = gs.id " +
                                   "where b.arrival >= :d1 and b.departure <= :d2 and rm.ocean_view = :ocean_view";
            command.Parameters.Add(new NpgsqlParameter("d1", d1));
            command.Parameters.Add(new NpgsqlParameter("d2", d2));
            command.Parameters.Add(new NpgsqlParameter("ocean_view", expected_ocean_view));
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Booking b = new Booking(Convert.ToInt64(reader.GetValue(0)), Convert.ToInt64(reader.GetValue(1)),
                                        Convert.ToInt64(reader.GetValue(2)), Convert.ToDateTime(reader.GetValue(3)),
                                        Convert.ToDateTime(reader.GetValue(4)),
                                        Convert.ToInt64(reader.GetValue(6)), reader.GetValue(7).ToString(), Convert.ToBoolean(reader.GetValue(8)),
                                        reader.GetValue(10).ToString(), Convert.ToInt32(reader.GetValue(11)), Convert.ToDecimal(reader.GetValue(12)),
                                        reader.GetValue(14).ToString(), reader.GetValue(15).ToString(), Convert.ToInt64(reader.GetValue(16)),
                                        reader.GetValue(18).ToString(), Convert.ToInt32(reader.GetValue(19)));
                b_list.Add(b);
            }
            connection.Close();
            return b_list;
        }
    }
}
