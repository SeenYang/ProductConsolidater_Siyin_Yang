using System.Collections.Generic;
using productConsolidater.model;

namespace productConsolidater.service
{
    public interface ICsvServices
    {
        IEnumerable<Catalog> ReadCatalogs(string filename, int sourceId, string filePath);

        IEnumerable<Supplier> ReadSuppliers(string filename, int sourceId, string filePath);

        IEnumerable<SupplierProductBarcode> ReadBarcodes(string filename, int sourceId, string filePath);

        void WriteOutput(List<ConsolidatedCatalog> detectedTransactions, string filePath);
    }
}