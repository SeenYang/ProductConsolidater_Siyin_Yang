using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace productConsolidater.model
{
    public class Catalog
    {
        public string Sku { get; set; }
        public string Description { get; set; }
        
        public int DataSourceId { get; set; }
    }
    
    public sealed class CatalogMap : ClassMap<Catalog>
    {
        public CatalogMap()
        {
            // Ignore DataSource.
            Map(m => m.Sku).Name("SKU");
            Map(m => m.Description).Name("Description");;
        }
    }
}