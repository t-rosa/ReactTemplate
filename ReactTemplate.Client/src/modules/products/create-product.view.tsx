import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Field, FieldError, FieldGroup, FieldLabel } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Spinner } from "@/components/ui/spinner";
import { $api } from "@/lib/api/client";
import { zodResolver } from "@hookform/resolvers/zod";
import { PlusIcon } from "lucide-react";
import * as React from "react";
import { useForm, Controller } from "react-hook-form";
import { CreateProductFormSchema, createProductSchema } from "./product.schema";

export function CreateProduct() {
  const [open, setOpen] = React.useState(false);

  const form = useForm<CreateProductFormSchema>({
    resolver: zodResolver(createProductSchema),
    defaultValues: {
      // TODO: Set appropriate default values
    },
  });

  const createProduct = $api.useMutation("post", "/api/product", {
    meta: {
      invalidatesQuery: ["get", "/api/product"],
      successMessage: "Product added",
      errorMessage: "An error occurred.",
    },
    onSuccess() {
      setOpen(!open);
      form.reset();
    },
  });

  function onSubmit(values: CreateProductFormSchema) {
    createProduct.mutate({
      body: {
        // TODO: Map form values to API request
      },
    });
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>
          <PlusIcon /> Add product
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add product</DialogTitle>
          <DialogDescription>Add a new product to your list.</DialogDescription>
        </DialogHeader>
        <form id="create-product" onSubmit={form.handleSubmit(onSubmit)}>
          <FieldGroup>
            {/* TODO: Add form fields */}
          </FieldGroup>
        </form>
        <DialogFooter>
          <DialogClose asChild>
            <Button type="button" variant="outline">
              Cancel
            </Button>
          </DialogClose>
          <Button type="submit" form="create-product" disabled={createProduct.isPending}>
            {createProduct.isPending ? "Submitting..." : "Submit"}
            {createProduct.isPending && <Spinner />}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
