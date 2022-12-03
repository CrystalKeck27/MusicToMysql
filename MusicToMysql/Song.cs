using File = TagLib.File;

namespace MusicToMysql; 

public class Song {
    public readonly string Filename;
    public readonly string Title;
    public readonly string Artist;
    public readonly string Album;

    public Song(string filename) {
        Filename = filename;
        
        File? file = File.Create(Filename);
        Title = file.Tag.Title;
        Artist = file.Tag.FirstPerformer;
        Album = file.Tag.Album;
    }
    
    public override string ToString() {
        return $"{Title} by {Artist}";
    }
}