using System.Collections.Generic;

namespace productConsolidater.model.dto
{
    public class BarcodeDto
    {
        public string Barcode { get; set; }
        public List<SkuDto> Skus { get; set; }
    }
}