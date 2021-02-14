## todo list:
1. Setup input/output data structure (/)
2. Setup CSV input/output method (/)
3. setup in-memory data source (/)
4. Setup processor for handle duplicate products. (/)
5. unit tests. 
6. (optional) Upgrade it to handle as much as companies instead of two. (/)

## Case Scenarios
1. SKU could be same in from different company
2. Same SKU might points to different product (different barcode)
3. Same barcode might points to different SKU in different company
4. Consolidated catalog (SKU list) should not contain duplicate SKU.

Some boundary cases:
1. One product associated with at less one supplier. Each supplier provide not less than 1 barcode.
2. One product may have multiple suppliers.
3. But barcode across suppliers is unique.