using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using productConsolidater.model;

namespace productConsolidater.service
{
    public class CsvServices : ICsvServices
    {
        // todo: read from config file.

        public IEnumerable<Catalog> ReadCatalogs(string filename, int sourceId)
        {
            try
            {
                using var reader = new StreamReader($"../../../input/{filename}"); // Debug build
                // using var reader = new StreamReader($"input/{filename}");
                using var file = new CsvReader(reader, CultureInfo.InvariantCulture);
                file.Context.RegisterClassMap<CatalogMap>();

                var result = file.GetRecords<Catalog>()?.ToList();
                result?.ForEach(r => r.DataSourceId = sourceId);
                return result;
            }
            catch (Exception e)
            {
                var message = $"Something wrong occurred while reading the file. Exception message: {e}";
                throw new Exception(message);
            }
        }

        public IEnumerable<Supplier> ReadSuppliers(string filename, int sourceId)
        {
            try
            {
                using var reader = new StreamReader($"../../../input/{filename}"); // Debug build
                // using var reader = new StreamReader($"input/{filename}");
                using var file = new CsvReader(reader, CultureInfo.InvariantCulture);
                file.Context.RegisterClassMap<SupplierMap>();

                var result = file.GetRecords<Supplier>()?.ToList();
                result?.ForEach(r => r.DataSourceId = sourceId);
                return result;
            }
            catch (Exception e)
            {
                var message = $"Something wrong occurred while reading the file. Exception message: {e}";
                throw new Exception(message);
            }
        }

        public IEnumerable<SupplierProductBarcode> ReadBarcodes(string filename, int sourceId)
        {
            try
            {
                using var reader = new StreamReader($"../../../input/{filename}"); // Debug build
                // using var reader = new StreamReader($"input/{filename}");
                using var file = new CsvReader(reader, CultureInfo.InvariantCulture);
                file.Context.RegisterClassMap<BarcodeMap>();

                var result = file.GetRecords<SupplierProductBarcode>()?.ToList();
                result?.ForEach(r => r.DataSourceId = sourceId);
                return result;
            }
            catch (Exception e)
            {
                var message = $"Something wrong occurred while reading the file. Exception message: {e}";
                throw new Exception(message);
            }
        }

        public void WriteOutput(List<ConsolidatedCatalog> detectedTransactions)
        {
            using var writer = new StreamWriter($"../../../output/result_output_{DateTime.UtcNow:yyyyMMddhhssss}.csv");
            // using var writer = new StreamWriter($"output/result_output_{DateTime.UtcNow:yyyyMMddhhssss}.csv");
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.Flush();
            csv.WriteHeader<ConsolidatedCatalog>();
            csv.WriteRecords(detectedTransactions);
        }
    }
}