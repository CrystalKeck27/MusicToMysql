namespace MysqlSongComparator; 

public class Song {
    public readonly string Title;
    public readonly string Artist;
    public readonly string Album;
    public readonly string Filename;
    public readonly int Id;
    
    public Song(string title, string artist, string album, string filename, int id) {
        Title = title;
        Artist = artist;
        Album = album;
        Filename = filename;
        Id = id;
    }
}