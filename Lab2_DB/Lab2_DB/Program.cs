using ConsoleTableExt;
using Lab2_DB.Controller;
using Lab2_DB.Database;
using Lab2_DB.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Lab2_DB
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnection dB = new DBConnection();
            ControllerClass controller = new ControllerClass(dB);
            controller.Start();
        }
    }
}
