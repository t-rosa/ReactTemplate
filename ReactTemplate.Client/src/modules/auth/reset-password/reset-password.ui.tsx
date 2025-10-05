import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Spinner } from "@/components/ui/spinner";
import * as React from "react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { ResetPasswordFormSchema } from "./reset-password.types";

interface ResetPasswordFormProps extends React.PropsWithChildren {
  form: UseFormReturn<ResetPasswordFormSchema>;
  onSubmit: (values: ResetPasswordFormSchema) => void;
}

function ResetPasswordFormRoot(props: ResetPasswordFormProps) {
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
  field: ControllerRenderProps<ResetPasswordFormSchema, "email">;
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

interface NewPasswordProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "newPassword">;
}

function NewPassword(props: NewPasswordProps) {
  return (
    <FormItem>
      <FormLabel>New password</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Password" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface ConfirmPasswordProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "confirmPassword">;
}

function ConfirmPassword(props: ConfirmPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Confirm password</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Confirm password" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface ResetCodeProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "resetCode">;
}

function ResetCode(props: ResetCodeProps) {
  return (
    <FormItem>
      <FormLabel>Reset code</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Reset code" {...props.field} />
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
      Reset password
      {props.isPending && <Spinner />}
    </Button>
  );
}

export const ResetPasswordForm = Object.assign(ResetPasswordFormRoot, {
  Email,
  ResetCode,
  NewPassword,
  ConfirmPassword,
  Submit,
});
