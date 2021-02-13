using CsvHelper.Configuration.Attributes;

namespace productConsolidater.model
{
    public class Supplier
    {
        [Name("ID")]
        public int Id { get; set; }
        [Name("Name")]
        public string Name { get; set; }
    }
}