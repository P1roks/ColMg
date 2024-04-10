using ColMg.Models;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace ColMg
{
    static public class CollectionRepository
    {
        static private readonly string dataDir = FileSystem.AppDataDirectory;
        static private readonly string imageDir = Path.Combine(dataDir, "images");

        static CollectionRepository()
        {
            if(!Directory.Exists(imageDir))
            {
                Directory.CreateDirectory(imageDir);
            }

            Trace.WriteLine(dataDir);
        }

        public static List<string> GetAllCollections() =>
            Directory.GetFiles(dataDir, "*.txt")
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        public static bool CollectionExists(string collectionName)
        {
            string path = Path.ChangeExtension(collectionName, ".txt");
            path = Path.Combine(dataDir, path);
            return File.Exists(path);
        }

        public static bool ExportCollection(string collectionName)
        {
            string collection = Path.ChangeExtension(collectionName, ".txt");
            string destinationPath = Path.Combine(Microsoft.VisualBasic.FileIO.SpecialDirectories.MyDocuments, collection);
            string sourcePath = Path.Combine(dataDir, collection);

            try
            {
                File.Copy(sourcePath, destinationPath, true);
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error: {e}");
                return false;
            }
        }

        public static bool ImportCollection(string externalPath)
        {
            string importedName = Path.GetFileName(externalPath);
            string collection = Path.ChangeExtension(importedName, ".txt");
            string internalPath = Path.Combine(dataDir, collection);

            if (!File.Exists(internalPath))
            {
                File.Copy(externalPath, internalPath);
                return true;
            }

            var externalLines = File.ReadLines(externalPath);
            var internalLines = File.ReadLines(internalPath);

            // Collections have the same columns
            if (externalLines.First().Equals(internalLines.First()))
            {
                using StreamWriter writer = File.AppendText(internalPath);
                foreach (var line in externalLines.Skip(1))
                {
                    writer.WriteLine(line);
                }
            }
            // Collections have different columns
            else
            {
                List<string> getColumns(string line) => (from column in line.Split(';') select column).ToList();

                List<Dictionary<string, string>> getColumnsFieldsMappings(IEnumerable<string> lines, List<string> columns)
                {
                    List<Dictionary<string, string>> mappings = new();
                    foreach(var singleItem in lines)
                    {
                        var field = singleItem.Split(";");
                        Dictionary<string, string> item = new();
                        for(int i = 0; i < field.Length; ++i)
                        {
                            item.Add(columns[i], field[i]);
                        }
                        mappings.Add(item);
                    }
                    return mappings;
                } 

                List<string> externalColumns = getColumns(externalLines.First());
                List<Dictionary<string, string>> externalItems = getColumnsFieldsMappings(externalLines.Skip(1), externalColumns);

                List<string> internalColumns = getColumns(internalLines.First());
                List<Dictionary<string, string>> internalItems = getColumnsFieldsMappings(internalLines.Skip(1), internalColumns);

                //Ensure that special columns are last
                List<string> outputColumns = internalColumns.SkipLast(2).Union(externalColumns.SkipLast(2)).ToList();
                outputColumns.Add("Status");
                List<string> outputItems = new();
                foreach(var item in internalItems.Concat(externalItems))
                {
                    outputItems.Add(
                        string.Join(";", outputColumns.Select(column => item.ContainsKey(column) ? item[column] : ""))
                    );
                }

                using StreamWriter writer = File.CreateText(internalPath);
                // This Column always has no data
                outputColumns.Add("Actions");
                writer.WriteLine(string.Join(";", outputColumns));
                foreach(var line in outputItems)
                {
                    writer.WriteLine(line);
                }
            }
            return false;
        }

        public static (IEnumerable<string>, IEnumerable<CollectionItem>) GetCollection(string collectionName)
        {
            string fileName = Path.ChangeExtension(collectionName, ".txt");
            string path = Path.Combine(dataDir, fileName);
            if (File.Exists(path))
            {
                var lines = File.ReadLines(path);
                var columns = lines.First().Split(';');
                var items = from line in lines.Skip(1) select CollectionItem.FromLine(line);
                return (columns, items);
            }
            else
            {
                return (null, null);
            }
        }

        public static void SaveCollection(CollectionHandler collection, string collectionName)
        {
            string fileName = Path.ChangeExtension(collectionName, ".txt");
            string path = Path.Combine(dataDir, fileName);
            using StreamWriter writer = new(path);

            writer.WriteLine(string.Join(';', collection.Columns));
            foreach (var item in collection.Items)
            {
                writer.WriteLine(item.ToLine());
            }
        }

        public static void CreateCollection(string collectionName)
        {
            string path = Path.ChangeExtension(collectionName, ".txt");
            path = Path.Combine(dataDir, path);
            File.WriteAllText(path, "Image;Name;Status;Actions");
        }

        public static string ImportImage(string imagePath)
        {
            string extension = Path.GetExtension(imagePath);
            string imageName = $"{Guid.NewGuid()}{extension}";
            string copyPath = Path.Combine(imageDir, imageName);
            File.Copy(imagePath, copyPath);
            return imageName;
        }

        public static string GetImagePath(string imageName)
        {
            string maybePath = Path.Combine(imageDir, imageName);
            return File.Exists(maybePath) ? maybePath : null;
        }
    }
}
