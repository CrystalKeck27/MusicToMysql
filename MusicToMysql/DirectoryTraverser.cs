namespace MusicToMysql; 

public static class DirectoryTraverser {
    public static List<string> GetFileNamesRecursive(string path) {
        List<string> files = new List<string>();
        List<string> directories = new List<string>();

        try {
            files.AddRange(Directory.GetFiles(path));
            directories.AddRange(Directory.GetDirectories(path));
        } catch (UnauthorizedAccessException) {
            // Ignore
        }

        foreach (var directory in directories) {
            files.AddRange(GetFileNamesRecursive(directory));
        }

        return files;
    }
    
    public static List<string> FilterFilesForMp3(IEnumerable<string> files) {
        return files.Where(file => file.EndsWith(".mp3")).ToList();
    }
}