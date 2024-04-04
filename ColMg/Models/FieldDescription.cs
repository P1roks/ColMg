namespace ColMg.Models
{
    class FieldDescription
    {
        public string Column { get; set; }
        public string Value { get; set; }

        public FieldDescription(string columnName, string value)
        {
            Column = columnName;
            Value = value;
        }
    }
}
