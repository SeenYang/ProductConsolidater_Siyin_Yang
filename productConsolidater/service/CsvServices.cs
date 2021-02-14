using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using NLog;
using productConsolidater.model;

namespace productConsolidater.service
{
    public class CsvServices : ICsvServices
    {
        // todo: read from config file.

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                var message = $"Something wrong occurred while reading the file {filename}. Exception message: {e}";
                logger.Error(message);
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
                var message = $"Something wrong occurred while reading the file {filename}. Exception message: {e}";
                logger.Error(message);
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
                var message = $"Something wrong occurred while reading the file {filename}. Exception message: {e}";
                logger.Error(message);
                throw new Exception(message);
            }
        }

        public void WriteOutput(List<ConsolidatedCatalog> consolidatedCatalogs)
        {
            try
            {
                var fileName = $"result_output_{DateTime.UtcNow:yyyyMMddhhssss}";
                logger.Info($"Printing file {fileName}.csv");
                
                using var streamWriter =
                    new StreamWriter($"../../../output/{fileName}.csv");
                // using var writer = new StreamWriter($"output/result_output_{DateTime.UtcNow:yyyyMMddhhssss}.csv");
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                csvWriter.Context.RegisterClassMap<ConsolidatedCatalogMap>();
                csvWriter.Flush();
                csvWriter.WriteHeader<ConsolidatedCatalog>();
                csvWriter.NextRecord(); // csvHelper won't add newline after header.
                // https://joshclose.github.io/CsvHelper/getting-started/#writing-a-csv-file
                csvWriter.WriteRecords(consolidatedCatalogs);
            }
            catch (Exception e)
            {
                var message = $"Something wrong occurred while writing the file. Exception message: {e}";
                logger.Error(message);
                throw new Exception(message);
            }
        }
    }
}