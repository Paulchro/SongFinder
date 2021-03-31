using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            int i = 0;
            Console.WriteLine("Welcome to my song finder");
            Console.WriteLine("Type part of a song you want to search for");
            string query = Console.ReadLine();
            var json = new WebClient().DownloadString("https://api.musixmatch.com/ws/1.1/track.search?q_lyrics=searchlyrics&apikey={Apikey}&".Replace("searchlyrics",query));
            var root = JsonConvert.DeserializeObject<Root>(json);
            foreach (var item in root.message.body.TrackList)
            {
                Console.WriteLine(i.ToString() + ". " +item.track.track_name + " - " + item.track.artist_name);
                i++;
            }
        }
        
    }


}