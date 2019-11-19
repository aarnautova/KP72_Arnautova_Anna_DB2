using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Lab2_DB.Database
{
    class RandomPackage
    {
        public int Previous { get; set; } = 0;

        public int[] RandomNumber(int min, int max, int count)
        {
            Random random = new Random(DateTime.Now.Ticks.GetHashCode() + Previous);
            int[] res = new int[count];
            for (int i = 0; i < count; i++)
            {
                int number = random.Next(min, max);
                if (!res.Contains(number)) 
                    res[i] = number;
                else
                    i--;
                Previous = number;
            }
            return res;
        }

        class NameList
        {
            public string[] boys { get; set; }
            public string[] girls { get; set; }
            public string[] last { get; set; }

            public NameList()
            {
                boys = new string[] { };
                girls = new string[] { };
                last = new string[] { };
            }
        }
        Random rand;
        List<string> Male;
        List<string> Female;
        List<string> Last;

        public RandomPackage()
        {
            rand = new Random(DateTime.Now.Ticks.GetHashCode());
            NameList l = new NameList();

            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader reader = new StreamReader("names.json"))
            using (JsonReader jreader = new JsonTextReader(reader))
            {
                l = serializer.Deserialize<NameList>(jreader);
            }

            Male = new List<string>(l.boys);
            Female = new List<string>(l.girls);
            Last = new List<string>(l.last);
        }

        public List<string> getSurnames()
        {
            return Last;
        }

        public List<string> getMalesNames()
        {
            return Male;
        }

        public List<string> getFemalesNames()
        {
            return Female;
        }

        public long getRandomNumberFromList(List<long> l)
        {
            Random random = new Random();
            int val = random.Next(0, l.Count);
            return l.ElementAt(val);
        }

        public string getRandomStringFromList(List<string> l)
        {
            Random random = new Random();
            int val = random.Next(0, l.Count);
            return l.ElementAt(val);
        }


        public bool getRandomBoolean()
        {
            int[] arr = RandomNumber(0, 3, 1);
            Console.WriteLine(arr[0]);
            if (arr[0] == 1) return true;
            return false;
        }

        public DateTime getRandomFutureDate()
        {
            Random gen = new Random();
            int range = 2 * 365;
            DateTime randomDate = DateTime.Today.AddDays(gen.Next(range));
            return randomDate;
        }

        public DateTime getRandomPastDate()
        {
            Random gen = new Random();
            int range = 1 * 365;
            DateTime randomDate = DateTime.Today.AddDays(-gen.Next(range));
            return randomDate;
        }

        public string getRandomChar()
        {
            Random random = new Random(DateTime.Now.Ticks.GetHashCode() + Previous++);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
