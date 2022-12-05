using System.Security.Cryptography;
using NAudio.Wave;
using File = TagLib.File;

namespace SongMatcher; 

public class Song {
    public readonly string Filename;
    public readonly string Title;
    public readonly string Artist;
    public readonly string Album;
    public readonly int Hash;
    
    public Song(string filename) {
        Filename = filename;
        
        File file = File.Create(filename);
        Title = file.Tag.Title;
        Artist = file.Tag.FirstPerformer;
        Album = file.Tag.Album;
        AudioFileReader reader = new AudioFileReader(filename);
        MD5 md5 = MD5.Create();
        byte[] rawHash = md5.ComputeHash(reader);
        // convert first 4 bytes of hash to int
        Hash = BitConverter.ToInt32(rawHash, 0);
    }
}