using System.ComponentModel;

namespace productConsolidater.model.dto
{
    public enum DataSourceEnum
    {
        [Description("Default")]
        Default,
        
        [Description("Catalog")]
        Catalog,
        
        [Description("Barcode")]
        Barcode,
        
        [Description("Supplier")]
        Supplier,
    }
}