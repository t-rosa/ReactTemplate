import { Form } from "@/components/ui/form";
import * as React from "react";
import type { UseFormReturn } from "react-hook-form";
import type { LoginFormSchema } from "../types";

interface LoginFormProps extends React.PropsWithChildren {
  form: UseFormReturn<LoginFormSchema>;
  onSubmit: (values: LoginFormSchema) => void;
}

export function _LoginForm(props: LoginFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}
