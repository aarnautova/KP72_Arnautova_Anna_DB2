using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Database
{

    class SearchRes
    {
        private long id;
        private string attr;
        private string ts_headline;

        public SearchRes(long id, string attr, string ts_headline)
        {
            Id = id;
            this.Attr = attr;
            this.Ts_headline = ts_headline;
        }

        public long Id { get => id; set => id = value; }
        public string Attr { get => attr; set => attr = value; }
        public string Ts_headline { get => ts_headline; set => ts_headline = value; }
    }
    class FullTextSearch
    {
        private DBConnection dbconnection;

        public FullTextSearch(DBConnection db)
        {
            dbconnection = db;
        }

        public List<SearchRes> getFullPhrase(string atr, string table, string phrase)
        {
            List<SearchRes> list = new List<SearchRes>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command =connection.CreateCommand();
            command.CommandText = $"SELECT id, {atr}, ts_headline(\"{atr}\", q) FROM {table}, phraseto_tsquery('{phrase}') AS q WHERE to_tsvector({table}.{atr}) @@ q";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                SearchRes s = new SearchRes(Convert.ToInt64(reader.GetValue(0).ToString()), reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                list.Add(s);
            }
           dbconnection.Close();
            return list;
        }
          

        public List<SearchRes> getAllWithNotIncludedWord(string atr, string table, string phrase)
        {
            List<SearchRes> list = new List<SearchRes>();
            NpgsqlConnection connection = dbconnection.Open();
            NpgsqlCommand command =connection.CreateCommand();
            command.CommandText = $"SELECT id, {atr} FROM {table} WHERE NOT(to_tsvector({table}.{atr}) @@ to_tsquery('{phrase}'))";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                SearchRes s = new SearchRes(Convert.ToInt64(reader.GetValue(0).ToString()), reader.GetValue(1).ToString(), null);
                list.Add(s);
            }
           dbconnection.Close();
            return list;
        }
    }
}
