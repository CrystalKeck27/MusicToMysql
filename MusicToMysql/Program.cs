// See https://aka.ms/new-console-template for more information

using MusicToMysql;
using MySqlConnector;

Console.WriteLine("Enter the directory of the music files you want to add to the database");

string directory = Console.ReadLine()!;

List<string> files =
    DirectoryTraverser.FilterFilesForMp3(DirectoryTraverser.GetFileNamesRecursive(directory));

List<Song> songs = files.Select(file => new Song(file)).ToList();

foreach (Song song in songs) {
    Console.WriteLine(song);
}

const string connectionString = "server=100.121.41.83;user=root;password=Music;database=music";

using MySqlConnection connection = new MySqlConnection(connectionString);

const string insertQuery = "INSERT INTO song_base (title, artist, album, filename) VALUES (@title, @artist, @album, @filename)";

connection.Open();

foreach(Song song in songs) {
    using MySqlCommand command = new MySqlCommand(insertQuery, connection);
    command.Parameters.AddWithValue("@title", song.Title);
    command.Parameters.AddWithValue("@artist", song.Artist);
    command.Parameters.AddWithValue("@album", song.Album);
    command.Parameters.AddWithValue("@filename", song.Filename);
    command.ExecuteNonQuery();
}

connection.Close();