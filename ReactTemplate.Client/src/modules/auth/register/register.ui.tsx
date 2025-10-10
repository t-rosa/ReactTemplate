import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Spinner } from "@/components/ui/spinner";
import * as React from "react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { RegisterFormSchema } from "./register.types";

interface RootProps extends React.PropsWithChildren {
  form: UseFormReturn<RegisterFormSchema>;
  onSubmit: (values: RegisterFormSchema) => void;
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
  field: ControllerRenderProps<RegisterFormSchema, "email">;
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

interface PasswordProps {
  field: ControllerRenderProps<RegisterFormSchema, "password">;
}

function Password(props: PasswordProps) {
  return (
    <FormItem>
      <FormLabel>Password</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Password" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface ConfirmPasswordProps {
  field: ControllerRenderProps<RegisterFormSchema, "confirmPassword">;
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

interface SubmitProps {
  isPending: boolean;
}

function Submit(props: SubmitProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Create account
      {props.isPending && <Spinner />}
    </Button>
  );
}

export const RegisterForm = Object.assign(Root, {
  Email,
  Password,
  ConfirmPassword,
  Submit,
});
