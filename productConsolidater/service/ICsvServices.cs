using System.Collections.Generic;
using productConsolidater.model;

namespace productConsolidater.service
{
    public interface ICsvServices
    {
        IEnumerable<Catalog> ReadCatalogs(string filename);

        IEnumerable<Supplier> ReadSuppliers(string filename);

        IEnumerable<SupplierProductBarcode> ReadBarcodes(string filename);
        
        void WriteOutput(List<ConsolidatedCatalog> detectedTransactions);
    }
}