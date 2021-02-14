using System;
using System.Collections.Generic;
using System.Linq;
using productConsolidater.model;
using productConsolidater.model.dto;

namespace productConsolidater.service
{
    public class ProductService : IProductService
    {
        /// <summary>
        ///     Steps for processing products:
        ///     1. Product barcode is provided by manufacturers, which is unique.
        ///     2. When two lines contain same barcode, take the line with smaller source Id (company id)
        ///     3. Group data by SKU (internal product code), Source Id (Company)
        /// </summary>
        /// <param name="mockContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessProduct(MockDbContextDto mockContext)
        {
            // Step 1, group by barcode due to barcode is unique
            var barcodeList = GetBarcodeList(mockContext);

            // Step 2, extract SKU associated with supplier. If there's two supplier, take smaller source Id.
            var consolidatedList = GetCatalogList(barcodeList);

            // Step 3, Build data set.
            var result = BuildConsolidatedCatalog(consolidatedList, mockContext);

            mockContext.ConsolidatedCatalog.AddRange(result);
        }

        private IEnumerable<ConsolidatedCatalog> BuildConsolidatedCatalog(
            IEnumerable<ConsolidatedCatalog> consolidatedList, MockDbContextDto mockContext)
        {
            var result = (from cList in consolidatedList
                join company in mockContext.Company on cList.DataSourceId equals company.SourceId
                join product in mockContext.Catalog on
                    new {cList.Sku, cList.DataSourceId} equals new
                        {product.Sku, product.DataSourceId}
                select new ConsolidatedCatalog
                {
                    Sku = cList.Sku,
                    Description = product.Description,
                    DataSourceId = cList.DataSourceId,
                    Source = company.SourceName
                }).ToList();

            return result;
        }

        public IEnumerable<ConsolidatedCatalog> GetCatalogList(List<BarcodeDto> barcodeList)
        {
            var resultList = new List<ConsolidatedCatalog>();
            foreach (var barcode in barcodeList)
            {
                var catalogTemp = barcode.Skus.First();
                if (resultList.Any(b =>
                    b.Sku == catalogTemp.Sku &&
                    b.DataSourceId == catalogTemp.DataSourceId))
                {
                    continue;
                }

                resultList.Add(new ConsolidatedCatalog
                {
                    Sku = catalogTemp.Sku,
                    DataSourceId = catalogTemp.DataSourceId
                });
            }

            return resultList;
        }

        public List<BarcodeDto> GetBarcodeList(MockDbContextDto mockDbContext)
        {
            var barcodeList = (
                from bc in mockDbContext.Barcodes
                group bc by bc.Barcode
                into groupList
                select new BarcodeDto
                {
                    Barcode = groupList.Key,
                    Skus = groupList.Select(g =>
                        new SkuDto
                        {
                            Sku = g.Sku,
                            DataSourceId = g.DataSourceId,
                            SupplierId = g.SupplierId
                        }).OrderBy(g => g.DataSourceId).ToList()
                }).ToList();
            return barcodeList;
        }
    }
}