using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SongFinder
{
    public class Header
    {
        public int status_code { get; set; }
        public double execute_time { get; set; }
        public int available { get; set; }
    }

    public class MusicGenre
    {
        public int music_genre_id { get; set; }
        public int music_genre_parent_id { get; set; }
        public string music_genre_name { get; set; }
        public string music_genre_name_extended { get; set; }
        public string music_genre_vanity { get; set; }
    }

    public class MusicGenreList
    {
        public MusicGenre music_genre { get; set; }
    }

    public class PrimaryGenres
    {
        public List<MusicGenreList> music_genre_list { get; set; }
    }

    public class Track
    {
        public int track_id { get; set; }
        public string track_name { get; set; }
        public List<object> track_name_translation_list { get; set; }
        public int track_rating { get; set; }
        public int commontrack_id { get; set; }
        public int instrumental { get; set; }
        public int @explicit { get; set; }
        public int has_lyrics { get; set; }
        public int has_subtitles { get; set; }
        public int has_richsync { get; set; }
        public int num_favourite { get; set; }
        public int album_id { get; set; }
        public string album_name { get; set; }
        public int artist_id { get; set; }
        public string artist_name { get; set; }
        public string track_share_url { get; set; }
        public string track_edit_url { get; set; }
        public int restricted { get; set; }
        public DateTime updated_time { get; set; }
        public PrimaryGenres primary_genres { get; set; }
    }

    public class TrackList
    {
        public Track track { get; set; }
    }

    public class Body
    {
        [JsonProperty("track_list")]
        public List<TrackList> TrackList { get; set; }
    }

    public class Message
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class Root
    {
        public Message message { get; set; }
    }


    public class Program
    {
        public static async Task Main(string[] args)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            int i = 1;
            Console.WriteLine("Welcome to my song finder");
            Console.WriteLine("Type part of a song you want to search for\n");
            string query = Console.ReadLine();
            var json = new WebClient().DownloadString("https://api.musixmatch.com/ws/1.1/track.search?q_lyrics=searchlyrics&apikey=apikey&".Replace("searchlyrics",query));
            var root = JsonConvert.DeserializeObject<Root>(json);
            foreach (var item in root.message.body.TrackList)
            {
                dict.Add(i, item.track.track_name + " " + item.track.artist_name);
                Console.WriteLine(i.ToString() + ". " +item.track.track_name + " - " + item.track.artist_name);
                i++;
            }

            SongChoose(dict);

            //OpenBrowser("https://www.google.com/search?q=searchitem".Replace("searchitem", ValidCode(dict).ToString().Replace(" ", "+")));
        }
        private static void SongChoose(Dictionary<int, string> dict)
        {
            Console.WriteLine("Press 1 to search for a song in browser or other key to leave..\n");
            string select = Console.ReadLine();
            switch (select)
            {
                case "1":
                    OpenBrowser("https://www.google.com/search?q=searchitem".Replace("searchitem", ValidCode(dict).ToString().Replace(" ", "+")));
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }
        public static string ValidCode(Dictionary<int, string> list)
        {
            int selection;
            int mycode;
            if (list.Count > 1)
            {
                Console.Write("Choose a song\n");
                while (!int.TryParse(Console.ReadLine(), out selection) || selection > list.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Wrong value, try again!\n(Choose from the list above)\n");
                    Console.ResetColor();
                }
                bool isInList = list.Keys.ElementAt(selection - 1) == -1;
                if (isInList)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Theres no song with this code! Try again...");
                    Console.ResetColor();
                    ValidCode(list);
                }
                return list.Values.ElementAt(selection - 1);
            }
            return list.Values.ElementAt(0);
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }


}