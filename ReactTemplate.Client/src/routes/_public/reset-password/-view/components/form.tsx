import { Form } from "@/components/ui/form";
import * as React from "react";
import type { UseFormReturn } from "react-hook-form";
import type { ResetPasswordFormSchema } from "../types";

interface ResetPasswordFormProps extends React.PropsWithChildren {
  form: UseFormReturn<ResetPasswordFormSchema>;
  onSubmit: (values: ResetPasswordFormSchema) => void;
}

export function _ResetPasswordForm(props: ResetPasswordFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}
