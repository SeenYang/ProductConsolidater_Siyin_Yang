## Project Description
This is a console app written in c# with .net 5.
The console app is mainly consolidating catalogs from multiple sources as describe above.

[Original Task description](https://github.com/tosumitagrawal/codingskills)

### Before Run
Before running the app, there's some tricks need to be done about the input files.
1. All file should in this format `[tyupe][source].csv`. i.e. For `catalog` info from `company A`, it should be `catalogA.csv`.
2. Currently every source need to provide three files: `catalog`, `supplier`, and `barcode`. Missing any of this the source will be skipped.

### how to run
```
option 1:
build the repo, and run in any IDE as you like.
```

```
option 2:
1. umccompress the zip file, put `csv` files into \input folder
2. run productConsolidater.exe
3. check \output folder for result.
```

```
option 3:
1. get the source code
2. use termial go in the foler
3. run `dotnet publish -c Release`
4. go to ~\productConsolidater\bin\Release\net5.0\publish\ and put files into `\input` folder
5. run productConsolidater.exe
6. check /output folder for result.
```

## Task Checklist:

###Initial load
Mega merge: All products from both companies should get merge into a common catalog

###BAU mode
* A new product added in Catalog A - (/)Add into `CatalogA.csv`.
* An existing product removed from Catalog A - (/)Same as above
* An existing product in Catalog B got new supplier with set of barcodes - (/)Handled by solution.

## Case Scenarios
1. Company A and B could have conflicting product codes (SKUs).
2. Product codes might be same, but they are different products. _(*Please refer to assumption)_
3. Product codes are different, but they are same product. (Distinct by barcode)
4. You should not be duplicating product records in merged catalog _(*Please refer to assumption)_.
5. Product on merged catalog must have information about the company it belongs to originally.

### Assumption
1. For #2 scenario, same SKU points to different products (barcode) must be in same data source (company). 
Same SKU in multiple data sources can't be different barcode. In current solution, it will save in to `ConsolidatedCatalog` with different `Source ID`.
2. For #4 scenario, duplicating product records refers to `SKU + Source`. Same `SKU` could points to different product in different `Source`.
3. All `suppliers` and `catalog` appears in `barcode` exist in provide files.

### unsupported case:
1. Product codes (SKU) might be same, but they are different products, with different barcode.
We only support if same barcode with two SKU, or same SKU

## todo list:
1. Setup input/output data structure (/) 2021-02-13
2. Setup CSV input/output method (/) 2021-02-13
3. setup in-memory data source (/) 2021-02-13
4. Setup processor for handle duplicate products. (/) 2021-02-14
5. unit tests. (/) 2021-02-14
6. (optional) Upgrade it to handle as much as companies instead of two. (/) 2021-02-14
Total approx spent 6 hours.
