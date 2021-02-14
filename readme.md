## todo list:
1. Setup input/output data structure (/)
2. Setup CSV input/output method (/)
3. setup in-memory data source (/)
4. Setup processor for handle duplicate products. (/)
5. unit tests. 
6. (optional) Upgrade it to handle as much as companies instead of two. (/)

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