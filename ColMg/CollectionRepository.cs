using ColMg.Models;
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

        public static (IEnumerable<string>, IEnumerable<CollectionItem>) getCollection(string collectionName)
        {
            string fileName = Path.ChangeExtension(collectionName, ".txt");
            string path = Path.Combine(dataDir, fileName);
            if(File.Exists(path))
            {
                var lines = File.ReadLines(path);
                var columns = lines.First().Split(' ');
                var items = from line in lines.Skip(1) select CollectionItem.FromLine(line);
                return (columns, items);
            }
            else
            {
                return (null, null);
            }
        }

        public static void saveCollection(CollectionHandler collection, string collectionName)
        {
            string fileName = Path.ChangeExtension(collectionName, ".txt");
            string path = Path.Combine(dataDir, fileName);
            using StreamWriter writer = new(path);

            writer.WriteLine(string.Join(' ', collection.Columns));
            foreach(var item in collection.Items)
            {
                writer.WriteLine(item.ToLine());
            }
        }

        public static void createCollection(string collectionName)
        {
            string path = Path.ChangeExtension(collectionName, ".txt");
            path = Path.Combine(dataDir, path);
            File.WriteAllText(path, "Grafika Nazwa Status Opis");
        }
    }
}
