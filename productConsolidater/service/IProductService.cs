using System.Collections.Generic;
using productConsolidater.model;
using productConsolidater.model.dto;

namespace productConsolidater.service
{
    public interface IProductService
    {
        void ProcessProduct(MockDbContextDto mockContext);

        IEnumerable<ConsolidatedCatalog> BuildConsolidatedCatalog(
            IEnumerable<ConsolidatedCatalog> consolidatedList, MockDbContextDto mockContext);

        IEnumerable<ConsolidatedCatalog> GetCatalogList(List<BarcodeDto> barcodeList);

        List<BarcodeDto> GetBarcodeList(MockDbContextDto mockDbContext);
    }
}