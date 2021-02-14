using CsvHelper.Configuration;

namespace productConsolidater.model
{
    public class ConsolidatedCatalog : Catalog
    {
        public string Source { get; set; }
    }

    public sealed class ConsolidatedCatalogMap : ClassMap<ConsolidatedCatalog>
    {
        public ConsolidatedCatalogMap()
        {
            // Ignore DataSource.
            Map(m => m.Sku).Name("SKU");
            Map(m => m.Description).Name("Description");
            Map(m => m.Source).Name("Source");
        }
    }
}