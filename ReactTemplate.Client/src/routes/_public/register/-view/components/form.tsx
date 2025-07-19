import { Form } from "@/components/ui/form";
import * as React from "react";
import type { UseFormReturn } from "react-hook-form";
import type { RegisterFormSchema } from "../types";

interface RegisterFormProps extends React.PropsWithChildren {
  form: UseFormReturn<RegisterFormSchema>;
  onSubmit: (values: RegisterFormSchema) => void;
}

export function _RegisterForm(props: RegisterFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}
