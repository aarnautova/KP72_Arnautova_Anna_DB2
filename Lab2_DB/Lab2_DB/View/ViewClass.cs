using ConsoleTableExt;
using Lab2_DB.Database;
using Lab2_DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Lab2_DB.View
{
    public enum Entity
    {
        Null,
        Room,
        Guest,
        Booking,
        Exception
    }

    class ViewClass
    {
        public Entity entity;
        private List<Room_type> r_t;
        private List<Guest_status> g_t;
        private bool searchmode;

        public ViewClass(List<Room_type> r_t, List<Guest_status> g_t)
        {
            this.r_t = r_t;
            this.g_t = g_t;
            entity = Entity.Null;
        }

        public void PrintTable(DataTable datatable)
        {
            ConsoleTableBuilder.From(datatable).WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();
        }

        public int Page()
        {
            while (true)
            {
                Console.WriteLine("Push '<' '>'\n\nPress 0 to exit");
                string arrow = Console.ReadLine();
                if (arrow == ">") return 1;
                if (arrow == "<") return -1;
                if (arrow == "0") return 0;
            }
        }

        private DataTable Room_types_table()
        {
            DataTable dataTable = new DataTable("Room types");
           
            dataTable.Columns.Add(new DataColumn("Id", Type.GetType("System.Int64")));
            dataTable.Columns.Add(new DataColumn("Type", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Beds", Type.GetType("System.Int32")));
            dataTable.Columns.Add(new DataColumn("Price", Type.GetType("System.String")));
            if (r_t.Count == 0)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = new object[] {-1, "Empty", -1, "Empty" };
                dataTable.Rows.Add(row);
            } else {
                foreach (Room_type r in r_t)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = new object[] { r.Id, r.Type_name, r.Bed_count, $"${r.Price.ToString()}" };
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        private DataTable Guest_status_table()
        {
            DataTable dataTable = new DataTable("Guest statuses");

            dataTable.Columns.Add(new DataColumn("Id", Type.GetType("System.Int64")));
            dataTable.Columns.Add(new DataColumn("Status name", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Required visits", Type.GetType("System.Int32")));
            if (g_t.Count == 0)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", -1};
                dataTable.Rows.Add(row);
            }
            else
            {
                foreach (Guest_status g in g_t)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = new object[] { g.Id, g.Status_name, g.Required_visits };
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }

        public DataTable Rooms_table(List<Room> r)
        {
            DataTable dataTable = new DataTable("Rooms");
            dataTable.Columns.Add(new DataColumn("Id", Type.GetType("System.Int64")));
            dataTable.Columns.Add(new DataColumn("Number", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Ocean view", Type.GetType("System.Boolean")));
            dataTable.Columns.Add(new DataColumn("Type", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Beds", Type.GetType("System.Int32")));
            dataTable.Columns.Add(new DataColumn("Price", Type.GetType("System.String")));
            if (r.Count == 0 || r[0] == null)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", false, -1, -1, "Empty" };
                dataTable.Rows.Add(row);
            }
            else
            {
                foreach (Room rm in r)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = new object[] { rm.Id, rm.Number, rm.Ocean_view, rm.Type.Type_name, rm.Type.Bed_count, $"${rm.Type.Price.ToString()}" };
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        public DataTable Guests_table(List<Guest> g)
        {
            DataTable dataTable = new DataTable("Guest statuses");

            dataTable.Columns.Add(new DataColumn("Id", Type.GetType("System.Int64")));
            dataTable.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Surname", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Status name", Type.GetType("System.String")));
            if (g.Count == 0 || g[0] == null)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", "Empty", "Empty" };
                dataTable.Rows.Add(row);
            }
            else
            {
                foreach (Guest gt in g)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = new object[] { gt.Id, gt.Name, gt.Surname, gt.Status.Status_name };
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }

        public DataTable Booking_table(List<Booking> b)
        {
            DataTable dataTable = new DataTable("Bookings");

            dataTable.Columns.Add(new DataColumn("Id", Type.GetType("System.Int64")));
            dataTable.Columns.Add(new DataColumn("Arrival", Type.GetType("System.DateTime")));
            dataTable.Columns.Add(new DataColumn("Departure", Type.GetType("System.DateTime")));
            dataTable.Columns.Add(new DataColumn("Room number", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Ocean view", Type.GetType("System.Boolean")));
            dataTable.Columns.Add(new DataColumn("Type", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Beds", Type.GetType("System.Int32")));
            dataTable.Columns.Add(new DataColumn("Guest name", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Guest surname", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Status name", Type.GetType("System.String")));
            if (b.Count == 0 || b[0] == null)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, null, null, "Empty", false, "Empty", -1, "Empty", "Empty", "Empty"};
                dataTable.Rows.Add(row);
            }
            else
            {
                foreach (Booking bk in b)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = new object[] { bk.Id, bk.Arrival, bk.Departure, bk.Room.Number,
                                                  bk.Room.Ocean_view, bk.Room.Type.Type_name,
                                                 bk.Room.Type.Bed_count, bk.Guest.Name, bk.Guest.Surname, bk.Guest.Status.Status_name };
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }


        public DataTable FullTextSearch_FullPhrase_table(List<SearchRes> res)
        {
            DataTable dataTable = new DataTable("Guest statuses");

            dataTable.Columns.Add(new DataColumn("Id", Type.GetType("System.Int64")));
            dataTable.Columns.Add(new DataColumn("Attr", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("ts_headline", Type.GetType("System.String")));
            if (res.Count == 0 || res[0] == null)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", "Empty" };
                dataTable.Rows.Add(row);
            }
            else
            {
                foreach (SearchRes s in res)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = new object[] { s.Id, s.Attr, s.Ts_headline };
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }

        public DataTable FullTextSearch_NotIncludedWord_table(List<SearchRes> res)
        {
            DataTable dataTable = new DataTable("Guest statuses");

            dataTable.Columns.Add(new DataColumn("Id", Type.GetType("System.Int64")));
            dataTable.Columns.Add(new DataColumn("Attr", Type.GetType("System.String")));
            if (res.Count == 0 || res[0] == null)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty" };
                dataTable.Rows.Add(row);
            }
            else
            {
                foreach (SearchRes s in res)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = new object[] { s.Id, s.Attr};
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }

        public int MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose task:\n1. Entities\n2. Full text search\n\nPress 0 to exit");
            try
            {
                int key = Convert.ToInt32(Console.ReadLine());
                if (key == 2) searchmode = true;
                else if (key == 1) searchmode = false;
                if (key != 1 && key != 2) return -1;
                return key;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public Entity EntitiesMenu()
        {
            Console.Clear();
            Console.Write("ENTITIES\n1. Rooms\n2. Guests\n");
            if (!searchmode) Console.Write("3. Bookings\n");
            Console.WriteLine("\nPress 0 to exit");
            Console.WriteLine("Choose entity:");
            try
            {
                int key = Convert.ToInt32(Console.ReadLine());
                switch (key)
                {
                    case 0:
                        entity = Entity.Null;
                        break;
                    case 1:
                        entity = Entity.Room;
                        break;
                    case 2:
                        entity = Entity.Guest;
                        break;
                    case 3:
                        if (!searchmode)
                            entity = Entity.Booking;
                        else entity = Entity.Exception;
                        break;
                    default: return Entity.Exception;
                }
            }
            catch (Exception)
            {
                entity = Entity.Exception;
            }
            return entity;
        }

        public int OperationsMenu()
        {
            Console.Clear();
            Console.WriteLine("OPERATIONS:\n1. Get by id\n2. Get all\n3. Add new\n4. Update\n5. Delete");
            if (entity == Entity.Booking) Console.WriteLine("6. Search through bookings");
            Console.WriteLine("\n\nPress 0 to exit");
            Console.WriteLine("Choose operation:");
            try
            {
                int key = Convert.ToInt32(Console.ReadLine());
                if (key >= 0 && key < 6) return key;
                if (entity == Entity.Booking && key == 6) return key;
                else return -1;

            }
            catch (Exception)
            {
                return -1;
            }
           
        }

        public long EnterId()
        {
            Console.WriteLine("Enter id:");
            try
            {
                return Convert.ToInt64(Console.ReadLine());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public Room RoomAddOrUpdateEnter()
        {
            Console.WriteLine("Enter number of room:");
            string number = Console.ReadLine();
            Console.WriteLine("Does it have an ocean view?");
            bool ocean_view = Convert.ToBoolean(Console.ReadLine());
            Console.WriteLine("Choose room type:");
            ConsoleTableBuilder.From(Room_types_table()).WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();
            long type_id = Convert.ToInt64(Console.ReadLine());
            return new Room(-1, type_id, number, ocean_view);
        }

        public Guest GuestAddOrUpdateEnter()
        {
            Console.WriteLine("Enter name of guest:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter surname of guest:");
            string surname = Console.ReadLine();
            Console.WriteLine("Choose guest status:");
            ConsoleTableBuilder.From(Guest_status_table()).WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();
            long status_id = Convert.ToInt64(Console.ReadLine());
            return new Guest(-1, name, surname, status_id);
        }

        public Booking BookingAddOrUpdate()
        {
            Console.WriteLine("Enter guest id");
            long guest_id = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Enter room id");
            long room_id = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Enter arrival date");
            DateTime arrival = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Enter departure date");
            DateTime departure = Convert.ToDateTime(Console.ReadLine());
            return new Booking(-1, guest_id, room_id, arrival, departure);
        }

        public Booking StaticSearch()
        {
            Console.WriteLine("Enter arrival date");
            DateTime arrival = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Enter departure date");
            DateTime departure = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Does it have an ocean view?");
            bool ocean_view = Convert.ToBoolean(Console.ReadLine());
            return new Booking(-1, -1, -1, arrival, departure, -1, null, ocean_view, "", 0, 0, "", "", -1, "", 0);
        }

        public int GuestAtr()
        {
            Console.WriteLine("CHOOSE ATRIBUTE");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Surname");
            int key = 0;
            while(key != 1 && key != 2)
            {
                key = Convert.ToInt32(Console.ReadLine());
            }
            return key;

        }

        public int FullText()
        {
            Console.WriteLine("CHOOSE SEARCH");
            Console.WriteLine("1. Full phrase");
            Console.WriteLine("2. Not included word");
            int key = 0;
            while (key != 1 && key != 2)
            {
                key = Convert.ToInt32(Console.ReadLine());
            }
            return key;

        }

        public string SearchQuery()
        {
            Console.WriteLine("Enter query");
            return Console.ReadLine();
        }

        public void Success()
        {
            Console.WriteLine("Operation succeed");
        }

        public void Error(string message)
        {
            Console.WriteLine($"Error occured: {message}");
        }

        public void Wait()
        {
            Console.Write("Press any key to get back: ");
            Console.ReadKey();
        }
    }
}
