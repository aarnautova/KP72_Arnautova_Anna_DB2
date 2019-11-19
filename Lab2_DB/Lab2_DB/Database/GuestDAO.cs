using Lab2_DB.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Database
{
    class GuestDAO : DAO<Guest>
    {
        public GuestDAO(DBConnection db) : base(db) { }

        public override void Create(Guest entity)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO public.guest (status_id, name, surname) VALUES (:status_id, :name, :surname)";
            command.Parameters.Add(new NpgsqlParameter("status_id", entity.Status_id));
            command.Parameters.Add(new NpgsqlParameter("name", entity.Name));
            command.Parameters.Add(new NpgsqlParameter("surname", entity.Surname));
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
            }
            catch (PostgresException)
            {
                throw new Exception("Unable to create new guest");
            }
            dbconnection.Close();
        }

        public override void Delete(long id)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM public.guest WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();
            dbconnection.Close();
        }

        public override Guest Get(long id)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.guest g INNER JOIN public.guest_status s ON g.status_id = s.id WHERE g.id = :guest_id";
            command.Parameters.Add(new NpgsqlParameter("guest_id", id));
            NpgsqlDataReader reader = command.ExecuteReader();
            Guest g = null;
            while (reader.Read())
            {
                g = new Guest(Convert.ToInt64(reader.GetValue(0)), reader.GetValue(1).ToString(),
                                    reader.GetValue(2).ToString(), Convert.ToInt64(reader.GetValue(3)),
                                    reader.GetValue(5).ToString(), Convert.ToInt32(reader.GetValue(6)));
            }
            dbconnection.Close();
            return g;
        }

        public override List<Guest> Get(int page)
        {
            List<Guest> g_list = new List<Guest>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.guest g INNER JOIN public.guest_status s ON g.status_id = s.id" +
                " LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Guest g = new Guest(Convert.ToInt64(reader.GetValue(0)), reader.GetValue(1).ToString(),
                                    reader.GetValue(2).ToString(), Convert.ToInt64(reader.GetValue(3)),
                                    reader.GetValue(5).ToString(), Convert.ToInt32(reader.GetValue(6)));
                g_list.Add(g);
            }
            dbconnection.Close();
            return g_list;
        }

        public override void Update(Guest entity)
        {
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE public.guest SET status_id = :status_id, name = :name, surname = :surname WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", entity.Id));
            command.Parameters.Add(new NpgsqlParameter("status_id", entity.Status_id));
            command.Parameters.Add(new NpgsqlParameter("name", entity.Name));
            command.Parameters.Add(new NpgsqlParameter("surname", entity.Surname));
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
            }
            catch (PostgresException)
            {
                throw new Exception("Unable to edit guest");
            }
            dbconnection.Close();
        }
    }
}
