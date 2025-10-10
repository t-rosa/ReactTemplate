import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Spinner } from "@/components/ui/spinner";
import * as React from "react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { ForgotPasswordFormSchema } from "./forgot-password.types";

interface RootProps extends React.PropsWithChildren {
  form: UseFormReturn<ForgotPasswordFormSchema>;
  onSubmit: (values: ForgotPasswordFormSchema) => void;
}

function Root(props: RootProps) {
  return (
    <Form {...props.form}>
      <form
        onSubmit={(e) => {
          void props.form.handleSubmit(props.onSubmit)(e);
        }}
        className="grid gap-6"
      >
        {props.children}
      </form>
    </Form>
  );
}

interface EmailProps {
  field: ControllerRenderProps<ForgotPasswordFormSchema, "email">;
}

function Email(props: EmailProps) {
  return (
    <FormItem>
      <FormLabel>Email</FormLabel>
      <FormControl>
        <Input placeholder="email@react-template.com" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface SubmitProps {
  isPending: boolean;
}

function Submit(props: SubmitProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Envoyer
      {props.isPending && <Spinner />}
    </Button>
  );
}

export const ForgotPasswordForm = Object.assign(Root, {
  Email,
  Submit,
});
