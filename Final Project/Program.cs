using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp

{

    class Program

    {

        static void Main(string[] args)

        {

            Music crimes = new Music(args);

        }

    }

}



namespace ConsoleApp

{

    public class Music

    {

        private static List<Musicdata> SampleMusicPlaylist = new List<Musicdata>();
        private static object minSong;

        public Music(string[] args)

        {

            string csvFilePath = string.Empty;

            string reportFilePath = string.Empty;

            string startupPath = Directory.GetCurrentDirectory();


            if (Debugger.IsAttached)

            {

                csvFilePath = Path.Combine(startupPath, "SampleMusicPlaylist.csv");

                reportFilePath = Path.Combine(startupPath, "SampleMusicPlaylist.txt");

            }



            else

            {

                if (args.Length != 2)

                {

                    Console.WriteLine("Invalid call.\n Valid call example : Music Analyzer <crime_csv_file_path> <report_file_path>");

                    Console.ReadLine();

                    return;

                }

                else

                {

                    csvFilePath = args[0];

                    reportFilePath = args[1];

                    if (!csvFilePath.Contains("\\"))

                    {

                        csvFilePath = Path.Combine(startupPath, csvFilePath);

                    }

                    if (!reportFilePath.Contains("\\"))

                    {

                        reportFilePath = Path.Combine(startupPath, reportFilePath);

                    }

                }

            }

            if (File.Exists(csvFilePath))

            {

                if (ReadData(csvFilePath))

                {

                    try

                    {

                        var file = File.Create(reportFilePath);

                        file.Close();

                    }

                    catch (Exception fe)

                    {

                        Console.WriteLine($"Unable to create report file at : {reportFilePath}");

                    }

                    WriteReport(reportFilePath);

                }

            }

            else

            {

                Console.Write($"Music Data file does not exist at path: {csvFilePath}");

            }

            Console.ReadLine();

        }

        private static bool ReadData(string filePath)

        {

            Console.WriteLine($"Reading data from file : {filePath}");

            try

            {

                int columns = 0;

                string[] musicdata = File.ReadAllLines(filePath);

                for (int index = 0; index < musicdata.Length; index++)

                {

                    string musicDatalines = musicdata[index];

                    string[] data = musicDatalines.Split(',');

                    if (index == 0) 

                    {

                        columns = data.Length;

                    }

                    else

                    {

                        if (columns != data.Length)

                        {

                            Console.WriteLine($"Row {index} contains {data.Length} values. It should contain {columns}.");

                            return false;

                        }

                        else

                        {

                            try

                            {

                                Musicdata musicData = new Musicdata();

                                musicData.Name = Convert.ToInt32(data[0]);

                                musicData.Artist = Convert.ToInt32(data[1]);

                                musicData.Album = Convert.ToInt32(data[2]);

                                musicData.Genre = Convert.ToInt32(data[3]);

                                musicData.Size = Convert.ToInt32(data[4]);

                                musicData.Time = Convert.ToInt32(data[5]);

                                musicData.Year = Convert.ToInt32(data[6]);

                                musicData.Plays = Convert.ToInt32(data[7]);

                                SampleMusicPlaylist.Add(musicData);

                            }

                            catch (InvalidCastException e)

                            {

                                Console.WriteLine($"Row {index} contains invalid value.");

                                return false;

                            }

                        }

                    }

                }

                Console.WriteLine($"Data read completed successfully.");

                return true;

            }

            catch (Exception ex)

            {

                Console.WriteLine("Error in reading data from csv file.");

                throw ex;

            }

        }

        private static void WriteReport(string filePath)

        {

            try

            {

                if (SampleMusicPlaylist != null && SampleMusicPlaylist.Any())

                {

                    Console.WriteLine($"Calculating the desired data and writing it to report file : {filePath}");

                    StringBuilder sb = new StringBuilder();

                    sb.Append("SampleMusicPlaylist");

                    sb.Append(Environment.NewLine);





                    int minYear = SampleMusicPlaylist.Min(x => x.Year);

                    int maxYear = SampleMusicPlaylist.Max(x => x.Year);



                    int years = maxYear - minYear + 1;

                    sb.Append($"Period: {minYear}-{maxYear} ({years} years)");

                    sb.Append(Environment.NewLine);



                    var mYears = from musicData in SampleMusicPlaylist

                                 where musicData.Album < 15000

                                 select musicData.Year;

                    string mYearsStr = string.Empty;

                    for (int i = 0; i < mYears.Count(); i++)

                    {

                        mYearsStr += mYears.ElementAt(i).ToString();



                        if (i < mYears.Count() - 1) mYearsStr += ", ";

                    }

                    sb.Append($"songs recieved 200 or more plays < 15000: {mYearsStr}");

                    sb.Append(Environment.NewLine);



                    var rYears = from musicData in SampleMusicPlaylist

                                 where musicData.Size > 500000

                                 select musicData;

                    string rYearsStr = string.Empty;

                    for (int i = 0; i < rYears.Count(); i++)

                    {

                        Musicdata crimeData = rYears.ElementAt(i);

                        rYearsStr += $"{crimeData.Year} = {crimeData.Size}";



                        if (i < rYears.Count() - 1) rYearsStr += ", ";

                    }

                    sb.Append($"How many songs are in the playlist with the Genre of “Alternative > 500000: {rYearsStr}");

                    sb.Append(Environment.NewLine);



                    var vCrime = from crimeData in SampleMusicPlaylist

                                 where crimeData.Year == 2010

                                 select crimeData;

                    Musicdata vCrimeData = vCrime.First();

                    double vhipHop = (double)vCrimeData.Time / (double)vCrimeData.Artist;

                    sb.Append($"How many songs are in the playlist with the Genre of “Hip-Hop/Rap: {vhipHop}");

                    sb.Append(Environment.NewLine);


                    double songFishbowl = SampleMusicPlaylist.Sum(x => x.Album) / SampleMusicPlaylist.Count;

                    sb.Append($"What songs are in the playlist from the album “Welcome to the Fishbowl?: {songFishbowl}");

                    sb.Append(Environment.NewLine);



                    int murders1 = SampleMusicPlaylist

                    .Where(x => x.Year >= 1994 && x.Year <= 1997)

                    .Sum(y => y.Album);

                    double playlistT = murders1 / SampleMusicPlaylist.Count;

                    sb.Append($"What are the songs in the playlist from before 1970: {playlistT}");

                    sb.Append(Environment.NewLine);



                    int long2 = SampleMusicPlaylist

                    .Where(x => x.Year >= 2010 && x.Year <= 2014)

                    .Sum(y => y.Album);

                    double longCharacters = long2 / SampleMusicPlaylist.Count;

                    sb.Append($"What are the song names that are more than 85 characters long?: {longCharacters}");

                    sb.Append(Environment.NewLine);



                    int minTheft = SampleMusicPlaylist

                    .Where(x => x.Year >= 1999 && x.Year <= 2004)

                    .Min(x => x.Year);

                    sb.Append($"What is the shortest song? (longest in Time): {minSong}");

                    sb.Append(Environment.NewLine);



                    int maxSong = SampleMusicPlaylist

                    .Where(x => x.Year >= 1999 && x.Year <= 2004)

                    .Max(x => x.Year);

                    _ = sb.Append(value: $"What is the longest song? (longest in Time): {maxSong}");

                    sb.Append(Environment.NewLine);
                    NewMethod();

                    sb.Append($"What is the longest song? (longest in Time)s: {maxSong}");

                    sb.Append(Environment.NewLine);

                    using (var stream = new StreamWriter(filePath))

                    {

                        stream.Write(sb.ToString());

                    }

                    Console.WriteLine();

                    Console.WriteLine(sb.ToString());

                    Console.WriteLine();

                    Console.WriteLine($"Written report file successfully at : {filePath}");

                }

                else

                {

                    Console.WriteLine($"No data to write.");

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine("Error in writing report file.");

                throw ex;

            }

        }

        private static void NewMethod()
        {
            int maxSong = SampleMusicPlaylist.OrderByDescending(x => x.Plays).First().Year;
        }
    }

}



namespace ConsoleApp

{

    public class Musicdata

    {

        public int Name { get; set; }

        public int Artist { get; set; }

        public int Album { get; set; }

        public int Genre { get; set; }

        public int Size { get; set; }

        public int Time { get; set; }

        public int Year { get; set; }

        public int Plays { get; set; }

    }

}