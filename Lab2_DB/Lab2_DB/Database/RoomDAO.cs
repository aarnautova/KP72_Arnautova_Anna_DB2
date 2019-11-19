using Lab2_DB.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Database
{
    class RoomDAO : DAO<Room>
    {

        public RoomDAO(DBConnection db) : base(db) { }

        public override void Create(Room entity)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO public.room (type_id, number, ocean_view) VALUES (:type_id, :number, :ocean_view)";
            command.Parameters.Add(new NpgsqlParameter("type_id", entity.Type_id));
            command.Parameters.Add(new NpgsqlParameter("number", entity.Number));
            command.Parameters.Add(new NpgsqlParameter("ocean_view", entity.Ocean_view));
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
            }
            catch (PostgresException)
            {
                throw new Exception("Unable to create new room");
            }
            dbconnection.Close();
        }

        public override void Delete(long id)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM public.room WHERE id = :id" ;
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();
            dbconnection.Close();
        }

        public override Room Get(long id)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.room rm INNER JOIN public.room_type rt ON rm.type_id = rt.id WHERE rm.id = :room_id";
            command.Parameters.Add(new NpgsqlParameter("room_id", id));
            NpgsqlDataReader reader = command.ExecuteReader();
            Room r = null;
            while (reader.Read())
            {
                r = new Room(Convert.ToInt64(reader.GetValue(0)), Convert.ToInt64(reader.GetValue(1)),
                                  reader.GetValue(2).ToString(), Convert.ToBoolean(reader.GetValue(3)),
                                  reader.GetValue(5).ToString(), Convert.ToInt32(reader.GetValue(6)),
                                  Convert.ToDecimal(reader.GetValue(7)));
            }
            dbconnection.Close();
            return r;
        }

        public override List<Room> Get(int page)
        {
            List<Room> r_list = new List<Room>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.room rm INNER JOIN public.room_type rt ON rm.type_id = rt.id" +
                " LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Room r = new Room(Convert.ToInt64(reader.GetValue(0)), Convert.ToInt64(reader.GetValue(1)),
                                  reader.GetValue(2).ToString(), Convert.ToBoolean(reader.GetValue(3)),
                                  reader.GetValue(5).ToString(), Convert.ToInt32(reader.GetValue(6)),
                                  Convert.ToDecimal(reader.GetValue(7)));
                r_list.Add(r);
            }
            dbconnection.Close();
            return r_list;
        }

        public override void Update(Room entity)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE public.room SET type_id = :type_id, number = :number, ocean_view = :ocean_view WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", entity.Id));
            command.Parameters.Add(new NpgsqlParameter("type_id", entity.Type_id));
            command.Parameters.Add(new NpgsqlParameter("number", entity.Number));
            command.Parameters.Add(new NpgsqlParameter("ocean_view", entity.Ocean_view));
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
            }
            catch (PostgresException)
            {
                throw new Exception("Unable to edit room");
            }
            dbconnection.Close();
        }
    }
}
