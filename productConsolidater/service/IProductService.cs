using productConsolidater.model;

namespace productConsolidater.service
{
    public interface IProductService
    {
        void ProcessProduct(MockDbContextDto mockContext);
    }
}