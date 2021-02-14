using CsvHelper.Configuration;

namespace productConsolidater.model
{
    public class SupplierProductBarcode
    {
        public int SupplierId { get; set; }

        /// <summary>
        ///     This is the SKU (Stock Keeping Unit)
        ///     numbers that unique to each store and allow the store to keep track of each item in inventory.
        ///     SKU is internal id that track where the inventory is.
        /// </summary>

        public string Sku { get; set; }

        /// <summary>
        ///     This is the UPC (universal product code)
        /// </summary>
        public string Barcode { get; set; }

        public int DataSourceId { get; set; }
    }

    public sealed class BarcodeMap : ClassMap<SupplierProductBarcode>
    {
        public BarcodeMap()
        {
            // Ignore DataSource.
            Map(m => m.SupplierId).Name("SupplierID");
            ;
            Map(m => m.Sku).Name("SKU");
            ;
            Map(m => m.Barcode).Name("Barcode");
            ;
        }
    }
}