namespace MysqlSongComparator; 

public class SongPair {
    public readonly Song Song1;
    public readonly Song Song2;
    
    public SongPair(Song song1, Song song2) {
        Song1 = song1;
        Song2 = song2;
    }
}