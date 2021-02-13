using System.Collections.Generic;
using productConsolidater.model;

namespace productConsolidater.service
{
    public interface ICsvServices
    {
        IEnumerable<Catalog> ReadCatalogs(string filename, int sourceId);

        IEnumerable<Supplier> ReadSuppliers(string filename, int sourceId);

        IEnumerable<SupplierProductBarcode> ReadBarcodes(string filename, int sourceId);
        
        void WriteOutput(List<ConsolidatedCatalog> detectedTransactions);
    }
}