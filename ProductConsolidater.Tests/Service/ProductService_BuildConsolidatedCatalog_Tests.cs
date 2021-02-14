using System.Collections.Generic;
using System.Linq;
using productConsolidater.model;
using productConsolidater.service;
using Xunit;

namespace ProductConsolidater.Tests.Service
{
    public class ProductService_BuildConsolidatedCatalog_Tests
    {
        private readonly IProductService _service;

        public ProductService_BuildConsolidatedCatalog_Tests()
        {
            _service = new ProductService();
        }

        [Fact(DisplayName = "Should build ConsolidatedCatalog List with two items.")]
        public void Test1()
        {
            #region Data Mock

            var company1 = new Company
            {
                SourceId = 1,
                SourceName = "Company A"
            };
            var company2 = new Company
            {
                SourceId = 2,
                SourceName = "B"
            };
            var product1 = new Catalog
            {
                DataSourceId = 1,
                Description = "Fake product 1",
                Sku = "FAKE-PRD-1"
            };
            var product2 = new Catalog
            {
                DataSourceId = 2,
                Description = "Fake product 2",
                Sku = "FAKE-PRD-2"
            };

            #endregion

            var input = new List<ConsolidatedCatalog>
            {
                new ConsolidatedCatalog
                {
                    DataSourceId = 1,
                    Sku = "FAKE-PRD-1",
                },

                new ConsolidatedCatalog
                {
                    DataSourceId = 2,
                    Sku = "FAKE-PRD-2",
                }
            };

            var context = new MockDbContextDto
            {
                Company = new List<Company> {company1, company2},
                Catalog = new List<Catalog> {product1, product2}
            };

            List<ConsolidatedCatalog> result = null;
            var exception = Record.Exception(() =>
            {
                result = _service.BuildConsolidatedCatalog(input, context).ToList();
            });

            Assert.Null(exception);
            Assert.True(result.Count == 2, "Should return 2 items.");
            var result1 = result.First();
            var result2 = result.Skip(1).First();
            Assert.Equal("FAKE-PRD-1", result1.Sku);
            Assert.Equal(1, result1.DataSourceId);
            Assert.Equal("Company A", result1.Source);
            Assert.Equal("FAKE-PRD-2", result2.Sku);
            Assert.Equal(2, result2.DataSourceId);
            Assert.Equal("B", result2.Source);
        }
    }
}