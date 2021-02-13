using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using productConsolidater.model;
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
            var csvSvc = _serviceProvider.GetService<ICsvServices>();

            if (csvSvc == null)
            {
                throw new Exception("Fail to initial CSV service.");
            }
            
            // Company A
            _catalogs.AddRange(csvSvc.ReadCatalogs("catalogA.csv")); 
            _suppliers.AddRange(csvSvc.ReadSuppliers("suppliersA.csv"));
            _barcodes.AddRange(csvSvc.ReadBarcodes("barcodesA.csv"));
            // Company B
            _catalogs.AddRange(csvSvc.ReadCatalogs("catalogB.csv")); 
            _suppliers.AddRange(csvSvc.ReadSuppliers("suppliersB.csv"));
            _barcodes.AddRange(csvSvc.ReadBarcodes("barcodesB.csv"));
            
            foreach (var catalog in _catalogs)
            {
                Console.WriteLine($"Catalog SKU: {catalog.Sku}, Description: {catalog.Description}.");
            }
            
            foreach (var supplier in _suppliers)
            {
                Console.WriteLine($"Supplier Id: {supplier.Id}, Name: {supplier.Name}.");
            }
            
            foreach (var barcode in _barcodes)
            {
                Console.WriteLine($"Barcode SKU: {barcode.Sku}, Supplier: {barcode.SupplierId}, Barcode: {barcode.Barcode}.");
            }


            // var transactionSvc = _serviceProvider.GetService<ITransactionServices>();
            Console.WriteLine("Hello World!");

            DisposeServices();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<ICsvServices, CsvServices>();
            // collection.AddScoped<ITransactionServices, TransactionServices>();
            _serviceProvider = collection.BuildServiceProvider();
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