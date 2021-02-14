using System.Collections.Generic;
using System.Linq;
using productConsolidater.model;
using productConsolidater.model.dto;
using productConsolidater.service;
using Xunit;

namespace ProductConsolidater.Tests.Service
{
    public class ProductService_GetCatalogList_Tests
    {
        private readonly IProductService _service;
        private readonly string sku1 = "sku-1";
        private readonly string sku2 = "sku-2";
        private readonly string sku3 = "sku-3";
        private readonly string barcode1 = "bar-code-1";
        private readonly string barcode2 = "bar-code-2";
        private readonly string barcode3 = "bar-code-3";
        private readonly int sourceId1 = 1;
        private readonly int sourceId2 = 2;
        private readonly int sourceId3 = 3;


        public ProductService_GetCatalogList_Tests()
        {
            _service = new ProductService();
        }

        /// <summary>
        /// Scenario 1: 2 barcode with 3 SKUs. First barcode with two SKUs, take the one with smaller sourceId.
        ///
        /// Expect Output: 2 SKUs (catalog).
        /// </summary>
        [Fact(DisplayName = "2 barcode with 3 SKUs. First barcode with two SKUs, take the one with smaller sourceId.")]
        public void Test1()
        {
            var input = new List<BarcodeDto>
            {
                new BarcodeDto
                {
                    Barcode = barcode1,
                    Skus = new List<SkuDto>
                    {
                        new SkuDto
                        {
                            Sku = sku1,
                            DataSourceId = sourceId1,
                            SupplierId = 314
                        },
                        new SkuDto
                        {
                            Sku = sku2,
                            DataSourceId = sourceId2,
                            SupplierId = 999
                        }
                    }
                },
                new BarcodeDto
                {
                    Barcode = barcode2,
                    Skus = new List<SkuDto>
                    {
                        new SkuDto
                        {
                            Sku = sku3,
                            DataSourceId = sourceId1,
                            SupplierId = 314
                        }
                    }
                }
            };

            List<ConsolidatedCatalog> result = null;

            var exception = Record.Exception(() => { result = _service.GetCatalogList(input).ToList(); });

            Assert.Null(exception);
            Assert.True(result.Count == 2, "Return catalog list should contain 2 items.");
            Assert.NotNull(result.FirstOrDefault(r => r.Sku == sku1));
            Assert.NotNull(result.FirstOrDefault(r => r.Sku == sku3));
        }

        /// <summary>
        /// Scenario 2: Two barcode with 1 SKU.
        ///
        /// Expect Output: 1 SKU (catalog).
        /// </summary>
        [Fact(DisplayName = "Two barcode with 1 SKU. Should return 1 catalog.")]
        public void Test2()
        {
            var input = new List<BarcodeDto>
            {
                new BarcodeDto
                {
                    Barcode = barcode1,
                    Skus = new List<SkuDto>
                    {
                        new SkuDto
                        {
                            Sku = sku1,
                            DataSourceId = sourceId1,
                            SupplierId = 314
                        }
                    }
                },
                new BarcodeDto
                {
                    Barcode = barcode2,
                    Skus = new List<SkuDto>
                    {
                        new SkuDto
                        {
                            Sku = sku1,
                            DataSourceId = sourceId1,
                            SupplierId = 314
                        }
                    }
                }
            };

            List<ConsolidatedCatalog> result = null;

            var exception = Record.Exception(() => { result = _service.GetCatalogList(input).ToList(); });

            Assert.Null(exception);
            Assert.True(result.Count == 1, "Return catalog list should contain 1 items.");
            Assert.NotNull(result.FirstOrDefault(r => r.Sku == sku1));
        }

        /// <summary>
        /// Scenario 3: Same SKU used in different data source (company), each (SKU, Company) contain different barcode
        ///
        /// Expect Output: 2 lines, same SKU (catalog) with different source Id.
        /// </summary>
        [Fact(DisplayName =
            "Same SKU used in different data source (company), each (SKU, Company) contain different barcode")]
        public void Test3()
        {
            var input = new List<BarcodeDto>
            {
                new BarcodeDto
                {
                    Barcode = barcode1,
                    Skus = new List<SkuDto>
                    {
                        new SkuDto
                        {
                            // SKU1 + SOURCE1
                            Sku = sku1,
                            DataSourceId = sourceId1,
                            SupplierId = 314
                        }
                    }
                },
                new BarcodeDto
                {
                    Barcode = barcode2,
                    Skus = new List<SkuDto>
                    {
                        new SkuDto
                        {
                            // SKU1 + SOURCE2
                            Sku = sku1,
                            DataSourceId = sourceId2,
                            SupplierId = 2123
                        }
                    }
                }
            };

            List<ConsolidatedCatalog> result = null;

            var exception = Record.Exception(() => { result = _service.GetCatalogList(input).ToList(); });

            Assert.Null(exception);
            Assert.True(result.Count == 2, "Return catalog list should contain 1 items.");
            Assert.NotNull(result.FirstOrDefault(r => r.Sku == sku1));
            Assert.True(result.FirstOrDefault(r => r.Sku == sku1)?.DataSourceId == sourceId1);
            Assert.True(result.Skip(1).FirstOrDefault(r => r.Sku == sku1)?.DataSourceId == sourceId2);
        }
    }
}