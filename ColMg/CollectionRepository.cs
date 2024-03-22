using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg
{
    public class CollectionRepository
    {
        static private readonly string dataDir = FileSystem.AppDataDirectory;

        public CollectionRepository()
        {
            Trace.WriteLine(dataDir);
        }

        public List<string> getCollections() =>
            Directory.GetFiles(dataDir, "*.txt")
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        public bool collectionExists(string collectionName)
        {
            string path = Path.ChangeExtension(collectionName, ".txt");
            path = Path.Combine(dataDir, path);
            return File.Exists(path);
        }

        public OrderedDictionary getCollection(string collectionName)
        {
            throw new NotImplementedException();
        }
    }
}
