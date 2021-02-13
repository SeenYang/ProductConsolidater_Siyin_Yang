using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace productConsolidater.model
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
         public int DataSourceId { get; set; }
    }

    public sealed class SupplierMap : ClassMap<Supplier>
    {
        public SupplierMap()
        {
            // Ignore DataSource.
            Map(m => m.Id).Name("ID");
            Map(m => m.Name).Name("Name");
        }
    }
}