using System;
using productConsolidater.model;
using productConsolidater.model.dto;

namespace productConsolidater.service
{
    public class DatasourceService : IDatasourceService
    {
        private ICsvServices _csvServices;

        public DatasourceService(ICsvServices csvServices)
        {
            _csvServices = csvServices;
        }

        public string GetDataSourceName(string fileName, out DataSourceEnum dataSourceType)
        {
            var companyName = string.Empty;
            dataSourceType = DataSourceEnum.Default;

            if (fileName.Contains(DataSourceEnum.Catalog.GetDescription(), StringComparison.InvariantCultureIgnoreCase))
            {
                dataSourceType = DataSourceEnum.Catalog;
                companyName = fileName.Replace(DataSourceEnum.Catalog.GetDescription(), "",
                        StringComparison.InvariantCultureIgnoreCase)
                    .Replace(".csv", "");
            }
            else if (fileName.Contains(DataSourceEnum.Supplier.GetDescription(),
                StringComparison.InvariantCultureIgnoreCase))
            {
                dataSourceType = DataSourceEnum.Supplier;
                companyName = fileName.Replace(DataSourceEnum.Supplier.GetDescription(), "",
                        StringComparison.InvariantCultureIgnoreCase)
                    .Replace(".csv", "");
            }
            else if (fileName.Contains(DataSourceEnum.Barcode.GetDescription(),
                StringComparison.InvariantCultureIgnoreCase))
            {
                dataSourceType = DataSourceEnum.Barcode;
                companyName = fileName.Replace(DataSourceEnum.Barcode.GetDescription(), "",
                        StringComparison.InvariantCultureIgnoreCase)
                    .Replace(".csv", "");
            }

            return companyName;
        }
    }
}