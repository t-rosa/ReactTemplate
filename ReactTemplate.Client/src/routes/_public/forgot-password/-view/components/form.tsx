import { Form } from "@/components/ui/form";
import * as React from "react";
import type { UseFormReturn } from "react-hook-form";
import type { ForgotPasswordFormSchema } from "../types";

interface ForgotPasswordFormProps extends React.PropsWithChildren {
  form: UseFormReturn<ForgotPasswordFormSchema>;
  onSubmit: (values: ForgotPasswordFormSchema) => void;
}

export function _ForgotPasswordForm(props: ForgotPasswordFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}
