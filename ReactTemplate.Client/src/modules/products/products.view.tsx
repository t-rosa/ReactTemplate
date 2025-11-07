import { ProductTable } from "./product-table/product-table.view";
import { CreateProduct } from "./create-product.view";
import { $api } from "@/lib/api/client";

export function Products() {
  const { data, isLoading, error } = $api.useQuery("get", "/api/product", {
    query: {
      limit: 10,
      offset: 0,
    },
  });

  return (
    <div className="flex flex-col gap-4">
      <div className="flex justify-between items-center">
        <h1>Products</h1>
        <CreateProduct />
      </div>

      {isLoading && <p>Loading...</p>}
      {error && <p>Error loading products</p>}
      {data && <ProductTable data={data} />}
    </div>
  );
}
