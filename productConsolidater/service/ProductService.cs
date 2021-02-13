using System;
using System.Linq;
using productConsolidater.model;

namespace productConsolidater.service
{
    public class ProductService : IProductService
    {
        public ProductService()
        {
        }


        /// <summary>
        /// Steps for processing products:
        /// 1. Product barcode is provided by manufacturers, which is unique.
        /// 2. When two lines contain same barcode, take the line with smaller source Id (company id)
        /// 3. Group data by SKU (internal product code), Source Id (Company)
        /// </summary>
        /// <param name="mockContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessProduct(MockDbContextDto mockContext)
        {
            var temp = (
                from bc in mockContext.Barcodes
                group bc by bc.Barcode
                into groupList
                select new
                {
                    barcode = groupList.Key,
                    Skus = groupList.Select(g => 
                        new
                        {
                            g.Sku, 
                            g.SupplierId, 
                            g.DataSourceId
                        })
                }).ToList();
            

            throw new System.NotImplementedException();
        }
    }
}