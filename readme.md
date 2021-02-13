1. Setup input/output data structure (/)
2. Setup CSV input/output method (/)
3. setup in-memory data source (/)
4. Setup processor for handle duplciate products.
5. unit tests.
6. (optional) Upgrade it to handle as much as companies instead of two.



-------
1. Diff supplier have same SKUs
2. Diff product code (SKU) point to same product.
3. Poduct code (SKU) points to diff products in different company

-- way to identify products
1. products with MULTIPLE suppliers, each supplier provide ONE or MORE barcode to ONE product.
2. One product might has multiple supplier
3. if barcode same from company A & B, we consider they are same.

----- processing logic:
Assumption: 
1. If same product exist in both barcode set, take Company A's.
2. file name checking order: catalog > supplier > barcode

Steps:
1. 