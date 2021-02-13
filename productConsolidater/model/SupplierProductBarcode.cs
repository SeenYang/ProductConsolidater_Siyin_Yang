using CsvHelper.Configuration.Attributes;

namespace productConsolidater.model
{
    public class SupplierProductBarcode
    {
        [Name("SupplierID")] 
        public int SupplierId { get; set; }

        /// <summary>
        /// This is the SKU (Stock Keeping Unit)
        /// numbers that unique to each store and allow the store to keep track of each item in inventory.
        ///
        /// SKU is internal id that track where the inventory is.
        /// </summary>

        [Name("SKU")]
        public string Sku { get; set; }

        /// <summary>
        /// This is the UPC (universal product code)
        /// </summary>
        [Name("Barcode")]
        public string Barcode { get; set; }
    }
}