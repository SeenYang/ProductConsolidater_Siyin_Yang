using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using productConsolidater.model;
using productConsolidater.model.dto;
using productConsolidater.service;

namespace productConsolidater
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IMapper _mapper;
        private static readonly string _inputFilePath = "../../../input";

        // Initial in-memory data set.
        private static List<Catalog> _catalogs = new List<Catalog>();
        private static List<Supplier> _suppliers = new List<Supplier>();
        private static List<SupplierProductBarcode> _barcodes = new List<SupplierProductBarcode>();
        private static List<ConsolidatedCatalog> _consolidatedCatalogs = new List<ConsolidatedCatalog>();
        private static List<Company> _companies = new List<Company>();

        static void Main(string[] args)
        {
            RegisterServices();
            // todo: replace with real data provider and put into DI for better usage.
            var mockContext = new MockDbContextDto
            {
                Barcodes = _barcodes,
                Catalog = _catalogs,
                Supplier = _suppliers,
                ConsolidatedCatalog = _consolidatedCatalogs,
                Company = _companies
            };
            GetDataSource(mockContext);
            ProcessProducts(mockContext);

            foreach (var catalog in _catalogs)
            {
                Console.WriteLine(
                    $"Catalog SKU: {catalog.Sku}, Description: {catalog.Description}, Source: {catalog.DataSourceId}");
            }
            foreach (var supplier in _suppliers)
            {
                Console.WriteLine(
                    $"Supplier Id: {supplier.Id}, Name: {supplier.Name}, Source: {supplier.DataSourceId}");
            }
            foreach (var barcode in _barcodes)
            {
                Console.WriteLine(
                    $"Barcode SKU: {barcode.Sku}, Supplier: {barcode.SupplierId}, Barcode: {barcode.Barcode}, Source: {barcode.DataSourceId}");
            }

            DisposeServices();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<ICsvServices, CsvServices>();
            collection.AddScoped<IDatasourceService, DatasourceService>();
            collection.AddScoped<IProductService, ProductService>();
            _serviceProvider = collection.BuildServiceProvider();
            InitializeAutomapper();
        }

        private static void GetDataSource(MockDbContextDto context)
        {
            var csvSvc = _serviceProvider.GetService<ICsvServices>();
            var datasourceService = _serviceProvider.GetService<IDatasourceService>();
            if (csvSvc == null || datasourceService == null)
            {
                throw new Exception("Fail to initial service(s).");
            }

            // read file names
            var fileNames = Directory.GetFiles(_inputFilePath)
                .Select(Path.GetFileName)
                .ToArray();

            var dataSources = new List<DataSourceDto>();

            foreach (var fileName in fileNames)
            {
                var sourceName = datasourceService.GetDataSourceName(fileName, out var dataSourceType);

                var exist = dataSources.FirstOrDefault(d => string.Equals(d.SourceName, sourceName));
                if (exist == null)
                {
                    dataSources.Add(new DataSourceDto(dataSources.Count + 1, sourceName, fileName, dataSourceType));
                }
                else
                {
                    exist.SetupFileName(fileName, dataSourceType);
                }
            }

            // Log incomplete data source.
            var incompleteDataSource = dataSources.Where(d => !d.GotAllDataSource()).ToList();
            if (incompleteDataSource.Any())
            {
                Console.WriteLine("** Following data source will be skip due to missing data source file;");
                foreach (var dataSource in incompleteDataSource)
                {
                    var msg = $"Data Source: {dataSource.SourceName}" +
                              $"{(string.IsNullOrWhiteSpace(dataSource.BarcodeFilename) ? Environment.NewLine + "Barcode file." : string.Empty)}" +
                              $"{(string.IsNullOrWhiteSpace(dataSource.BarcodeFilename) ? Environment.NewLine + "Barcode file." : string.Empty)}" +
                              $"{(string.IsNullOrWhiteSpace(dataSource.BarcodeFilename) ? Environment.NewLine + "Barcode file." : string.Empty)}";

                    Console.WriteLine(msg);
                }
            }

            foreach (var source in dataSources.Where(d => d.GotAllDataSource()))
            {
                Console.WriteLine($"Adding data source from {source.SourceName}, sourceId: {source.SourceId}");
                context.Company.Add(_mapper.Map<DataSourceDto, Company>(source));
                context.Catalog.AddRange(csvSvc.ReadCatalogs(source.CatalogFilename, source.SourceId));
                context.Supplier.AddRange(csvSvc.ReadSuppliers(source.SupplierFileName, source.SourceId));
                context.Barcodes.AddRange(csvSvc.ReadBarcodes(source.BarcodeFilename, source.SourceId));
            }
        }

        private static void ProcessProducts(MockDbContextDto mockContext)
        {
            var _service = _serviceProvider.GetService<IProductService>();
            if (_service == null)
            {
                throw new Exception("Fail to initial IProductService.");
            }

            _service.ProcessProduct(mockContext);
        }

        static void InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<DataSourceDto, Company>(); });
            _mapper = new Mapper(config);
        }

        private static void DisposeServices()
        {
            switch (_serviceProvider)
            {
                case null:
                    return;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}