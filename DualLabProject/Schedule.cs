using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DualLabProject
{
    internal class Schedule
    {
        private List<string> list;
        private const string ending = "<end-of-file>";
        private const string bestBus = "Posh";

        public Schedule(string filePath)
        {
            list = new List<string>(File.ReadAllLines(filePath, Encoding.UTF8));
        }

        private TimeSpan GetTime(int i, int timeNum)
        {
            return TimeSpan.Parse(list[i].Split(' ')[timeNum]);
        }

        //Method for deleting uncomfortable entries
        private void DeleteUncomfortable()
        {
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].Split(' ')[0] != bestBus) continue;
                var timeP1 = GetTime(i, 2);
                var timeP2 = GetTime(i, 1);

                for (int j = 0; j < list.Count; j++)
                {
                    if(list[j].Split(' ')[0] != bestBus)
                    {
                        var timeG1 = GetTime(j, 2);
                        var timeG2 = GetTime(j, 1);
                        if (timeP1 == timeG1 && timeP2 == timeG2) list.RemoveAt(j);            
                    }
                }
            }
        }

        //Method for making a collection efficient according to conditions
        private void MakeEfficient()
        {
            for (int i = 0; i < list.Count; i++)
            {
                var timeP1 = GetTime(i, 1);
                var timeP2 = GetTime(i, 2);

                for (int j = 0; j < list.Count; j++)
                {
                    if (i == j) continue;
                    var timeG1 = GetTime(j, 1);
                    var timeG2 = GetTime(j, 2);

                    if (timeP1 == timeG1 && timeP2 < timeG2 || timeP1 > timeG1 && timeP2 == timeG2 ||
                        timeP1 > timeG1 && timeP2 < timeG2) list.RemoveAt(j);                 
                }
            }
        }

        public void MakeSorted()
        {
            //Removing ending
            list.Remove(ending);

            //Deleting time more than one hour
            list = list.Where(x => TimeSpan.Parse(x.Split(' ')[2]) - TimeSpan.Parse(x.Split(' ')[1]) <= new TimeSpan(1, 0, 0)).ToList();

            //Making a collection efficient according to conditions
            MakeEfficient();

            //Deleting uncomfortable entries
            DeleteUncomfortable();
            
            //Sorting a collection
            list = list.OrderBy(x => TimeSpan.Parse(x.Split(' ')[1])).ToList();
        }

        //Method for moving a data in a file
        public void MoveToFile(string path)
        {
            try
            {
                using (var stream = new StreamWriter(path))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Split(' ')[0] == bestBus)
                        {
                            stream.WriteLine(list[i]);
                        }
                    }
                    stream.WriteLine();

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Split(' ')[0] != bestBus)
                        {
                            stream.WriteLine(list[i]);
                        }
                    }
                    stream.WriteLine(ending);
                }
            }
            catch(DirectoryNotFoundException ex)
            {
                Console.WriteLine("File path is wrong. " + ex.Message);
            }
        }
    }
}
