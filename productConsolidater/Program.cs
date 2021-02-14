using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Targets;
using productConsolidater.model;
using productConsolidater.model.dto;
using productConsolidater.service;

namespace productConsolidater
{
    internal class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IMapper _mapper;
        private static IConfigurationRoot _config;
        private static Logger Logger;

        // Initial in-memory data set.
        private static readonly List<Catalog> _catalogs = new List<Catalog>();
        private static readonly List<Supplier> _suppliers = new List<Supplier>();
        private static readonly List<SupplierProductBarcode> _barcodes = new List<SupplierProductBarcode>();
        private static readonly List<ConsolidatedCatalog> _consolidatedCatalogs = new List<ConsolidatedCatalog>();
        private static readonly List<Company> _companies = new List<Company>();

        private static string HostingEnvironment =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        private static void Main(string[] args)
        {
            try
            {
                RegisterServices();
                AddConfiguration();


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
                PrintProcessedData(mockContext);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            Console.WriteLine("Press enter to exist.");
            Console.ReadLine();
            DisposeServices();
        }

        private static void PrintProcessedData(MockDbContextDto context)
        {
            var csvSvc = _serviceProvider.GetService<ICsvServices>();
            if (csvSvc == null) throw new Exception("Fail to initial service(s).");

            csvSvc.WriteOutput(context.ConsolidatedCatalog);
        }

        private static void GetDataSource(MockDbContextDto context)
        {
            var csvSvc = _serviceProvider.GetService<ICsvServices>();
            var datasourceService = _serviceProvider.GetService<IDatasourceService>();
            if (csvSvc == null || datasourceService == null) throw new Exception("Fail to initial service(s).");

            // read file names
            var fileNames = Directory.GetFiles(_config["InputFilePath"])
                .Select(Path.GetFileName)
                .ToArray();

            var dataSources = new List<DataSourceDto>();

            foreach (var fileName in fileNames)
            {
                var sourceName = datasourceService.GetDataSourceName(fileName, out var dataSourceType);

                var exist = dataSources.FirstOrDefault(d => string.Equals(d.SourceName, sourceName));
                if (exist == null)
                    dataSources.Add(new DataSourceDto(dataSources.Count + 1, sourceName, fileName, dataSourceType));
                else
                    exist.SetupFileName(fileName, dataSourceType);
            }

            // Log incomplete data source.
            var incompleteDataSource = dataSources.Where(d => !d.GotAllDataSource()).ToList();
            if (incompleteDataSource.Any())
            {
                Logger.Info("** Following data source will be skip due to missing data source file;");
                foreach (var dataSource in incompleteDataSource)
                {
                    var msg = $"Data Source: {dataSource.SourceName}" +
                              $"{(string.IsNullOrWhiteSpace(dataSource.BarcodeFilename) ? Environment.NewLine + "Barcode file." : string.Empty)}" +
                              $"{(string.IsNullOrWhiteSpace(dataSource.BarcodeFilename) ? Environment.NewLine + "Barcode file." : string.Empty)}" +
                              $"{(string.IsNullOrWhiteSpace(dataSource.BarcodeFilename) ? Environment.NewLine + "Barcode file." : string.Empty)}";

                    Logger.Info(msg);
                }
            }

            foreach (var source in dataSources.Where(d => d.GotAllDataSource()))
            {
                Logger.Info($"Adding data source from {source.SourceName}, sourceId: {source.SourceId}");
                context.Company.Add(_mapper.Map<DataSourceDto, Company>(source));
                context.Catalog.AddRange(csvSvc.ReadCatalogs(source.CatalogFilename, source.SourceId,
                    _config["InputFilePath"]));
                context.Supplier.AddRange(csvSvc.ReadSuppliers(source.SupplierFileName, source.SourceId,
                    _config["InputFilePath"]));
                context.Barcodes.AddRange(csvSvc.ReadBarcodes(source.BarcodeFilename, source.SourceId,
                    _config["InputFilePath"]));
            }
        }

        private static void ProcessProducts(MockDbContextDto mockContext)
        {
            Logger.Info("Processing data.");
            var _service = _serviceProvider.GetService<IProductService>();
            if (_service == null) throw new Exception("Fail to initial IProductService.");

            _service.ProcessProduct(mockContext);
        }

        private static void AddConfiguration()
        {
            var logConfig = new LoggingConfiguration();
            var logConsole = new ConsoleTarget("logconsole");
            logConfig.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
            LogManager.Configuration = logConfig;
            Logger = LogManager.GetCurrentClassLogger();
            
            Logger.Info($"Current env is {HostingEnvironment}.");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{HostingEnvironment}.json", true, true);

            _config = builder.Build();

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

        private static void InitializeAutomapper()
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