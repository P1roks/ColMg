using ColMg.Utils;

namespace ColMg.Models
{
    public class CollectionItem
    {
        public List<object> DisplayFields
        {
            get
            {
                List<object> fields = new()
                {
                    ImageLocation
                };
                fields.AddRange(ExtraFields);
                fields.Add(Status);

                return fields;
            }
        }

        public List<string> ExtraFields { get; set; }

        public float Opacity { get => Status == ItemStatus.Sold ? 0.5f : 1.0f; }
        public ItemStatus Status { get; set; }
        public string StatusString {
            get => Status.ToString().SplitCamelCase();
        }

        public string ImageLocation { get; set; }

        public CollectionItem(List<string> fields)
        {
            ExtraFields = fields;
        }

        public CollectionItem() {}

        public static CollectionItem FromLine(string line)
        {
            var split = line.Split(';');
            return new CollectionItem()
            {
                ImageLocation = split.First(),
                ExtraFields = split.Skip(1).SkipLast(1).ToList(),
                Status = (ItemStatus)Enum.ToObject(typeof(ItemStatus), Convert.ToInt32(split.Last()))
            };
        }

        public string ToLine()
        {
            List<object> fields = new()
            {
                ImageLocation
            };
            fields.AddRange(ExtraFields);
            fields.Add((int)Status);

            return string.Join(';', fields);
        }
    }
}
