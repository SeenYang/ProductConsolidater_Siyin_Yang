using System;

namespace productConsolidater.model.dto
{
    public class DataSourceDto
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string CatalogFilename { get; set; }
        public string SupplierFileName { get; set; }
        public string BarcodeFilename { get; set; }

        public DataSourceDto(int id,
            string sourceName,
            string fileName,
            DataSourceEnum dataSourceType)
        {
            SourceId = id;
            SourceName = sourceName;
            switch (dataSourceType)
            {
                case DataSourceEnum.Catalog:
                    CatalogFilename = fileName;
                    break;
                case DataSourceEnum.Supplier:
                    SupplierFileName = fileName;
                    break;
                case DataSourceEnum.Barcode:
                    BarcodeFilename = fileName;
                    break;

                case DataSourceEnum.Default:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataSourceType), dataSourceType, null);
            }
        }

        public void SetupFileName(string fileName, DataSourceEnum type)
        {
            switch (type)
            {
                case DataSourceEnum.Catalog:
                    CatalogFilename = fileName;
                    break;
                case DataSourceEnum.Supplier:
                    SupplierFileName = fileName;
                    break;
                case DataSourceEnum.Barcode:
                    BarcodeFilename = fileName;
                    break;

                case DataSourceEnum.Default:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public bool GotAllDataSource()
        {
            return !string.IsNullOrWhiteSpace(CatalogFilename) &&
                   !string.IsNullOrWhiteSpace(SupplierFileName) &&
                   !string.IsNullOrWhiteSpace(BarcodeFilename);
        }
    }
}