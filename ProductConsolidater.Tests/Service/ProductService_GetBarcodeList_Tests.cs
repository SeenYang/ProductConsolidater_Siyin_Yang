using System;
using System.Collections.Generic;
using System.Linq;
using productConsolidater.model;
using productConsolidater.model.dto;
using productConsolidater.service;
using Xunit;

namespace ProductConsolidater.Tests.Service
{
    public class ProductService_GetBarcodeList_Tests
    {
        private readonly IProductService _service;

        private readonly string barcode1 = "fake-barcode-1";
        private readonly string barcode2 = "fake-barcode-2";
        private readonly int dataSourceId1 = 1;
        private readonly int dataSourceId2 = 2;
        private readonly string sku1 = Guid.NewGuid().ToString();
        private readonly string sku2 = Guid.NewGuid().ToString();
        private readonly int supplierId1 = 99;
        private readonly int supplierId2 = 100;

        public ProductService_GetBarcodeList_Tests()
        {
            _service = new ProductService();
        }

        [Fact(DisplayName = "Get Barcode List, Valid case. Should return list of barcode and group SKUs, etc.")]
        public void Test1()
        {
            // Arrange
            var bc1 = new SupplierProductBarcode
            {
                Barcode = barcode1,
                DataSourceId = dataSourceId1,
                Sku = sku1,
                SupplierId = supplierId1
            };
            // Same barcode, we consider same product, even they are not the same sku.
            var SameBarcodeWithBc1 = new SupplierProductBarcode
            {
                Barcode = barcode1,
                DataSourceId = dataSourceId2,
                Sku = sku2,
                SupplierId = supplierId2
            };
            var SameSkuWithBc1DiffBarcode = new SupplierProductBarcode
            {
                Barcode = barcode2,
                DataSourceId = dataSourceId1,
                Sku = sku1,
                SupplierId = supplierId1
            };


            // Prevent context shared across test and mess up result.
            var context = new MockDbContextDto
            {
                Barcodes = new List<SupplierProductBarcode> {bc1, SameBarcodeWithBc1, SameSkuWithBc1DiffBarcode},
                Catalog = new List<Catalog>(),
                Supplier = new List<Supplier>(),
                ConsolidatedCatalog = new List<ConsolidatedCatalog>()
            };

            // Action
            List<BarcodeDto> result = null;
            var exception = Record.Exception(() => { result = _service.GetBarcodeList(context); });

            // Assert
            Assert.Null(exception);
            Assert.NotNull(result);
            Assert.True(result.Count == 2, "Should only return one barcode.");
            Assert.True(result.First().Skus.Count == 2, $"Barcode {result.First().Barcode} should contain 2 SKUs.");
        }
    }
}