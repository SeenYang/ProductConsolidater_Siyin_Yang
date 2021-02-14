using productConsolidater.model.dto;
using Xunit;

namespace ProductConsolidater.Tests.Dtos
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("TestFile1.csv", DataSourceEnum.Barcode, @"Barcode file name should be ""TestFile1.csv"" ")]
        public void SetupFileNameTest_ValidCases_Barcode(string fileName, DataSourceEnum type, string errorMsg)
        {
            var sourceDto = new DataSourceDto();
            var exception = Record.Exception(() => { sourceDto.SetupFileName(fileName, type); });

            Assert.Null(exception);
            Assert.True(!string.IsNullOrWhiteSpace(sourceDto.BarcodeFilename), errorMsg);
        }

        [Theory]
        [InlineData("TestFile2.csv", DataSourceEnum.Catalog, @"Catalog file name should be ""TestFile2.csv"" ")]
        public void SetupFileNameTest_ValidCases_Catalog(string fileName, DataSourceEnum type, string errorMsg)
        {
            var sourceDto = new DataSourceDto();
            var exception = Record.Exception(() => { sourceDto.SetupFileName(fileName, type); });

            Assert.Null(exception);
            Assert.True(!string.IsNullOrWhiteSpace(sourceDto.CatalogFilename), errorMsg);
        }

        [Theory]
        [InlineData("TestFile3.csv", DataSourceEnum.Supplier, @"Supplier file name should be ""TestFile3.csv"" ")]
        public void SetupFileNameTest_ValidCases_Supplier(string fileName, DataSourceEnum type, string errorMsg)
        {
            var sourceDto = new DataSourceDto();
            var exception = Record.Exception(() => { sourceDto.SetupFileName(fileName, type); });

            Assert.Null(exception);
            Assert.True(!string.IsNullOrWhiteSpace(sourceDto.SupplierFileName), errorMsg);
        }


        [Theory]
        [InlineData("TestFile4.csv", DataSourceEnum.Default)]
        public void SetupFileNameTest_InvalidCases(string fileName, DataSourceEnum type)
        {
            // Should throw exception due to data source type invalid.
            var sourceDto = new DataSourceDto();
            var exception = Record.Exception(() => { sourceDto.SetupFileName(fileName, type); });

            Assert.NotNull(exception);
            Assert.Null(sourceDto.BarcodeFilename);
            Assert.Null(sourceDto.CatalogFilename);
            Assert.Null(sourceDto.SupplierFileName);
        }

        [Fact]
        public void SetupDataSourceDto_by_constructor()
        {
            var companyName = "Compnay A";
            var companyId = 1;
            var targetType = DataSourceEnum.Catalog;
            var fileName = "catalogA.csv";

            var testObj = new DataSourceDto(companyId, companyName, fileName, targetType);

            Assert.NotNull(testObj);
            Assert.Equal(companyId, testObj.SourceId);
            Assert.Equal(companyName, testObj.SourceName);
            Assert.Equal(fileName, testObj.CatalogFilename);
            Assert.True(string.IsNullOrWhiteSpace(testObj.SupplierFileName));
            Assert.True(string.IsNullOrWhiteSpace(testObj.BarcodeFilename));
        }
    }
}