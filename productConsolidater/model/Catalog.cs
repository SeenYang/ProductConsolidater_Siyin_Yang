using CsvHelper.Configuration.Attributes;

namespace productConsolidater.model
{
    public class Catalog
    {
        [Name("SKU")]
        public string Sku { get; set; }
        
        [Name("Description")]
        public string Description { get; set; }
    }
}