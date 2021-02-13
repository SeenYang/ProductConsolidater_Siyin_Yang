using productConsolidater.model.dto;

namespace productConsolidater.service
{
    public interface IDatasourceService
    {
        string GetDataSourceName(string fileName, out DataSourceEnum dataSourceType);
    }
}