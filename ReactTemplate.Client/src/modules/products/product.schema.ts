import * as z from "zod";

export const createProductSchema = z.object({
  // TODO: Add fields based on entity properties
});

export const updateProductSchema = z.object({
  // TODO: Add fields based on entity properties
});

export type CreateProductFormSchema = z.infer<typeof createProductSchema>;
export type UpdateProductFormSchema = z.infer<typeof updateProductSchema>;
