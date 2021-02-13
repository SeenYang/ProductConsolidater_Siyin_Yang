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
        /// 2. When two line contain same barcode, take the line with smaller source Id (company id)
        /// 3. 
        /// </summary>
        /// <param name="mockContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessProduct(MockDbContextDto mockContext)
        {
            throw new System.NotImplementedException();
        }
    }
}