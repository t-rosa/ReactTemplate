import { GetProductResponse } from "../product.types";
import { ProductColumns } from "./product-columns";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

export interface ProductTableProps {
  data: GetProductResponse[];
}

export function ProductTable({ data }: ProductTableProps) {
  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            {/* TODO: Add table headers based on columns */}
          </TableRow>
        </TableHeader>
        <TableBody>
          {data.map((item) => (
            <TableRow key={item.id}>
              {/* TODO: Add table cells */}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
