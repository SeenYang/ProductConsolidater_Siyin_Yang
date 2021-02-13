using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using CsvHelper;
using CsvHelper.Configuration;
using productConsolidater.model;

namespace productConsolidater.service
{
    public class CsvServices : ICsvServices
    {
        private readonly string _filePath = "Files/"; // Release build
        // private readonly string _filePath = "../../../Files/";    // Debug build

        public IEnumerable<Catalog> ReadCatalogs(string filename)
        {
            try
            {
                using var reader = new StreamReader($"../../../input/{filename}"); // Debug build
                // using var reader = new StreamReader($"input/{filename}");
                using var file = new CsvReader(reader, CultureInfo.InvariantCulture);
                return file.GetRecords<Catalog>()?.ToList();
            }
            catch (Exception e)
            {
                var message = $"Something wrong occurred while reading the file. Exception message: {e}";
                throw new Exception(message);
            }
        }

        public IEnumerable<Supplier> ReadSuppliers(string filename)
        {
            try
            {
                using var reader = new StreamReader($"../../../input/{filename}"); // Debug build
                // using var reader = new StreamReader($"input/{filename}");
                using var file = new CsvReader(reader, CultureInfo.InvariantCulture);
                return file.GetRecords<Supplier>()?.ToList();
            }
            catch (Exception e)
            {
                var message = $"Something wrong occurred while reading the file. Exception message: {e}";
                throw new Exception(message);
            }
        }

        public IEnumerable<SupplierProductBarcode> ReadBarcodes(string filename)
        {
            try
            {
                using var reader = new StreamReader($"../../../input/{filename}"); // Debug build
                // using var reader = new StreamReader($"input/{filename}");
                using var file = new CsvReader(reader, CultureInfo.InvariantCulture);
                return file.GetRecords<SupplierProductBarcode>()?.ToList();
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