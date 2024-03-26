using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg
{
    static public class CollectionRepository
    {
        static private readonly string dataDir = FileSystem.AppDataDirectory;

        static CollectionRepository()
        {
            Trace.WriteLine(dataDir);
        }

        public static List<string> getCollections() =>
            Directory.GetFiles(dataDir, "*.txt")
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        public static bool collectionExists(string collectionName)
        {
            string path = Path.ChangeExtension(collectionName, ".txt");
            path = Path.Combine(dataDir, path);
            return File.Exists(path);
        }

        public static OrderedDictionary getCollection(string collectionName)
        {
            throw new NotImplementedException();
        }

        public static void createCollection(string collectionName)
        {
            string path = Path.ChangeExtension(collectionName, ".txt");
            path = Path.Combine(dataDir, path);
            File.Create(path);
        }
    }
}
