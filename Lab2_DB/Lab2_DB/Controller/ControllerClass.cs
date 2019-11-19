using Lab2_DB.Database;
using Lab2_DB.Model;
using Lab2_DB.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Controller
{
    public enum Operation
    {
        Get = 1,
        GetAll,
        Add,
        Update,
        Delete,
        Search
    }
    class ControllerClass
    {
        
        private ViewClass view;
        private RoomDAO roomDAO;
        private GuestDAO guestDAO;
        private BookingDAO bookingDAO;
        private DictionaryDAO dictionaryDAO;
        private FullTextSearch fullTextSearch;

        public ControllerClass(DBConnection db)
        {
            roomDAO = new RoomDAO(db);
            guestDAO = new GuestDAO(db);
            bookingDAO = new BookingDAO(db);
            dictionaryDAO = new DictionaryDAO(db);
            fullTextSearch = new FullTextSearch(db);
            this.view = new ViewClass(dictionaryDAO.GetRoom_Types(), dictionaryDAO.GetGuest_status());
        }

        public void Start()
        {
            while (true)
            {
                int mode = view.MainMenu();
                if (mode == 0) break;
                if(mode > 0)
                {
                    while (true)
                    {
                        Entity entity = view.EntitiesMenu();
                        if (entity == Entity.Null) break;
                        else if(entity != Entity.Exception)
                        {
                            if (mode == 1)
                            {
                                while (true)
                                {
                                    int operation = view.OperationsMenu();
                                    if (operation == 0) break;
                                    try
                                    {
                                        switch ((Operation)operation)
                                        {
                                            case Operation.Add:
                                                AddOperation();
                                                break;
                                            case Operation.Get:
                                                GetOperation();
                                                break;
                                            case Operation.GetAll:
                                                GetAllOperation();
                                                break;
                                            case Operation.Update:
                                                UpdateOperation();
                                                break;
                                            case Operation.Delete:
                                                DeleteOperation();
                                                break;
                                            case Operation.Search:
                                                SearchBookingOperation();
                                                break;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        view.Error(e.Message.ToString());
                                    }
                                    view.Wait();
                                }
                            }
                            else
                            {
                                FullTextSearch();
                                view.Wait();
                            }
                        }
                    }
                }
            }
        }

        private void AddOperation() {
            switch (view.entity)
            {
                case Entity.Room:
                    Room room = view.RoomAddOrUpdateEnter();
                    roomDAO.Create(room);
                    break;
                case Entity.Guest:
                    Guest guest = view.GuestAddOrUpdateEnter();
                    guestDAO.Create(guest);
                    break;
                case Entity.Booking:
                    Booking booking = view.BookingAddOrUpdate();
                    bookingDAO.Create(booking);
                    break;
            }
            view.Success();
        }
        private void GetOperation()
        {
            long id = -1;
            while(id < 0) {
            id = view.EnterId();
            }
            switch (view.entity)
            {
                case Entity.Room:
                    List<Room> r_table = new List<Room>() { roomDAO.Get(id) };
                    view.PrintTable(view.Rooms_table(r_table));
                    break;
                case Entity.Guest:
                    List<Guest> g_table = new List<Guest>() { guestDAO.Get(id) };
                    view.PrintTable(view.Guests_table(g_table));
                    break;
                case Entity.Booking:
                    List<Booking> b_table = new List<Booking>() { bookingDAO.Get(id) };
                    view.PrintTable(view.Booking_table(b_table));
                    break;
            }
            view.Success();
        }
        private void GetAllOperation()
        {
            int page = 0;
            while (true)
            {
                switch (view.entity)
                {
                    case Entity.Room:
                        List<Room> r_table = roomDAO.Get(page);
                        view.PrintTable(view.Rooms_table(r_table));
                        break;
                    case Entity.Guest:
                        List<Guest> g_table = guestDAO.Get(page);
                        view.PrintTable(view.Guests_table(g_table));
                        break;
                    case Entity.Booking:
                        List<Booking> b_table = bookingDAO.Get(page);
                        view.PrintTable(view.Booking_table(b_table));
                        break;
                }
                int arr = view.Page();
                if (arr == 0) break;
                else page += arr;
                if (page < 0) page = 0;
            }
            view.Success();
        }
        private void UpdateOperation()
        {
            long id = -1;
            while (id < 0)
            {
                id = view.EnterId();
            }
            if (id < 0) throw new Exception("Wrong id");
            switch (view.entity)
            {
                case Entity.Room:
                    Room room = view.RoomAddOrUpdateEnter();
                    room.Id = id;
                    roomDAO.Update(room);
                    break;
                case Entity.Guest:
                    Guest guest = view.GuestAddOrUpdateEnter();
                    guest.Id = id;
                    guestDAO.Update(guest);
                    break;
                case Entity.Booking:
                    Booking booking = view.BookingAddOrUpdate();
                    booking.Id = id;
                    bookingDAO.Update(booking);
                    break;
            }
            view.Success();
        }
        private void DeleteOperation()
        {
            long id = -1;
            while (id < 0)
            {
                id = view.EnterId();
            }
            if (id < 0) throw new Exception("Wrong id");
            switch (view.entity)
            {
                case Entity.Room:
                    roomDAO.Delete(id);
                    break;
                case Entity.Guest:
                    guestDAO.Delete(id);
                    break;
                case Entity.Booking:
                    bookingDAO.Delete(id);
                    break;
            }
            view.Success();
        }

        private void SearchBookingOperation()
        {
            Booking searchdata = view.StaticSearch();
            List<Booking> b = bookingDAO.StaticSearch(searchdata.Arrival, searchdata.Departure, searchdata.Room.Ocean_view);
            view.PrintTable(view.Booking_table(b));
        }

        private void FullTextSearch()
        {
            List<SearchRes> res = new List<SearchRes>();
            string table;
            string attr;
            switch (view.entity)
            {
                case Entity.Room:
                    table = "room";
                    attr = "number";
                    break;
                case Entity.Guest:
                    table = "guest";
                    int atr = view.GuestAtr();
                    if (atr == 1) attr = "name";
                    else attr = "surname";
                    break;
                default: throw new Exception("Wrong entity selected");
            }
            int search = view.FullText();
            string query = view.SearchQuery();
            switch (search)
            {
                case 1:
                    res.AddRange(fullTextSearch.getFullPhrase(attr, table, query));
                    view.PrintTable(view.FullTextSearch_FullPhrase_table(res));
                    break;
                case 2:
                    res.AddRange(fullTextSearch.getAllWithNotIncludedWord(attr, table, query));
                    view.PrintTable(view.FullTextSearch_NotIncludedWord_table(res));
                    break;
                default: throw new Exception("Wrong entity selected");
            }
        }
    }
}
