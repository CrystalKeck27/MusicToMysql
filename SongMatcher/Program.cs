// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using MusicToMysql;
using MySqlConnector;
using SongMatcher;

const string directory = @"E:\Music\";

Console.WriteLine("Reading from directory: " + directory);

List<string> files =
    DirectoryTraverser.FilterFilesForMp3(DirectoryTraverser.GetFileNamesRecursive(directory));

Console.WriteLine("Found " + files.Count + " files");

ConcurrentQueue<Song?> songsQueue = new ConcurrentQueue<Song?>();

Parallel.ForEach(files, file => {
    Song song = new Song(file);
    songsQueue.Enqueue(song);
    int progress = songsQueue.Count * 100 / files.Count;
    Console.WriteLine("Progress: " + progress + "%");
});

Console.WriteLine("Songs processed: " + songsQueue.Count);

const string connectionString = "server=100.121.41.83;user=root;password=Music;database=music";

using MySqlConnection connection = new MySqlConnection(connectionString);

const string insertQuery = "INSERT INTO song_processed_hash (hash, file, title, artist, album) VALUES (@hash, @file, @title, @artist, @album)";

connection.Open();

while (songsQueue.TryDequeue(out Song? song)) {
    using MySqlCommand command = new MySqlCommand(insertQuery, connection);
    command.Parameters.AddWithValue("@hash", song!.Hash);
    command.Parameters.AddWithValue("@title", song.Title);
    command.Parameters.AddWithValue("@artist", song.Artist);
    command.Parameters.AddWithValue("@album", song.Album);
    command.Parameters.AddWithValue("@file", song.Filename);
    command.ExecuteNonQuery();
}

connection.Close();

Console.WriteLine("Done");