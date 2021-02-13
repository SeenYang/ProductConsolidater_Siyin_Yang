using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using productConsolidater.model;
using productConsolidater.model.dto;
using productConsolidater.service;

namespace productConsolidater
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        // Initial in-memory data set.
        private static List<Catalog> _catalogs = new List<Catalog>();
        private static List<Supplier> _suppliers = new List<Supplier>();
        private static List<SupplierProductBarcode> _barcodes = new List<SupplierProductBarcode>();

        static void Main(string[] args)
        {
            RegisterServices();
            GetDataSource();

            foreach (var catalog in _catalogs)
            {
                Console.WriteLine($"Catalog SKU: {catalog.Sku}, Description: {catalog.Description}, Source: {catalog.DataSourceId}");
            }

            foreach (var supplier in _suppliers)
            {
                Console.WriteLine($"Supplier Id: {supplier.Id}, Name: {supplier.Name}, Source: {supplier.DataSourceId}");
            }

            foreach (var barcode in _barcodes)
            {
                Console.WriteLine(
                    $"Barcode SKU: {barcode.Sku}, Supplier: {barcode.SupplierId}, Barcode: {barcode.Barcode}, Source: {barcode.DataSourceId}");
            }


            // var transactionSvc = _serviceProvider.GetService<ITransactionServices>();
            Console.WriteLine("Hello World!");

            DisposeServices();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<ICsvServices, CsvServices>();
            collection.AddScoped<IDatasourceService, DatasourceService>();
            // collection.AddScoped<ITransactionServices, TransactionServices>();
            _serviceProvider = collection.BuildServiceProvider();
        }

        public static void GetDataSource()
        {
            var csvSvc = _serviceProvider.GetService<ICsvServices>();
            var datasourceService = _serviceProvider.GetService<IDatasourceService>();
            if (csvSvc == null || datasourceService == null)
            {
                throw new Exception("Fail to initial service(s).");
            }


            // read file names
            var fileNames = Directory.GetFiles("../../../input")
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

            foreach (var source in dataSources.Where(d => d.GotAllDataSource()))
            {
                Console.WriteLine($"Adding data source from {source.SourceName}, sourceId: {source.SourceId}");
                _catalogs.AddRange(csvSvc.ReadCatalogs(source.CatalogFilename, source.SourceId));
                _suppliers.AddRange(csvSvc.ReadSuppliers(source.SupplierFileName, source.SourceId));
                _barcodes.AddRange(csvSvc.ReadBarcodes(source.BarcodeFilename, source.SourceId));
            }
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