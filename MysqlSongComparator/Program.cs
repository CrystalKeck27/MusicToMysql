// See https://aka.ms/new-console-template for more information

using MySqlConnector;
using MysqlSongComparator;

Console.WriteLine("Hello, World!");


const string connectionString = "server=100.121.41.83;user=root;password=Music;database=music";

using MySqlConnection connection = new MySqlConnection(connectionString);

connection.Open();

// Fetch all the rows from the "song_processed" table

MySqlCommand command = new MySqlCommand(@"SELECT 
    song_base_hash.id as base_id,
    coalesce(song_base_hash.title, '') as base_title,
    coalesce(song_base_hash.artist, '') as base_artist,
    coalesce(song_base_hash.album, '') as base_album,
    song_base_hash.file as base_file,
    song_processed_hash.id as processed_id,
    coalesce(song_processed_hash.title, '') as processed_title,
    coalesce(song_processed_hash.artist, '') as processed_artist,
    coalesce(song_processed_hash.album, '') as processed_album,
    song_processed_hash.file as processed_file
    FROM song_base_hash
INNER JOIN song_processed_hash ON song_base_hash.hash = song_processed_hash.hash
ORDER BY song_processed_hash.artist, song_processed_hash.album, song_processed_hash.title", connection);

MySqlDataReader reader = command.ExecuteReader();

List<SongPair> songPairs = new List<SongPair>();

while (reader.Read()) {
    Song baseSong = new Song(
        reader.GetString("base_title"),
        reader.GetString("base_artist"),
        reader.GetString("base_album"),
        reader.GetString("base_file"),
        reader.GetInt32("base_id")
    );
    Song processedSong = new Song(
        reader.GetString("processed_title"),
        reader.GetString("processed_artist"),
        reader.GetString("processed_album"),
        reader.GetString("processed_file"),
        reader.GetInt32("processed_id")
    );
    
    songPairs.Add(new SongPair(baseSong, processedSong));
}

reader.Close();

Console.WriteLine("Done reading data.");
Console.WriteLine("Total songs: " + songPairs.Count);

// Compare the two lists

foreach(SongPair songPair in songPairs) {
    Console.WriteLine("Comparing " + songPair.Song1.Title + " by " + songPair.Song1.Artist);
    Console.WriteLine("With      " + songPair.Song2.Title + " by " + songPair.Song2.Artist);
}