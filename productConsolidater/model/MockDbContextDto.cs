using System.Collections.Generic;

namespace productConsolidater.model
{
    public class MockDbContextDto
    {
        public List<Catalog> Catalog { get; set; }
        public List<Supplier> Supplier { get; set; }
        public List<SupplierProductBarcode> Barcodes { get; set; }
        public List<ConsolidatedCatalog> ConsolidatedCatalog { get; set; }
        public List<Company> Company { get; set; }
    }
}