import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { components } from "@/lib/api/schema";
import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";
import * as React from "react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { ForgotPasswordFormSchema } from "./forgot-password.type";

export function ForgotPasswordLayout(props: React.PropsWithChildren) {
  return (
    <main className="overflow-hidden">
      <div className="isolate flex min-h-dvh items-center justify-center p-6 lg:p-8">
        {props.children}
      </div>
    </main>
  );
}

export function ForgotPasswordCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function ForgotPasswordCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

function ForgotPasswordCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Mot de passe oublié.</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">Saisissez votre adresse e-mail.</p>
    </div>
  );
}

function ForgotPasswordCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

ForgotPasswordCard.Content = ForgotPasswordCardContent;
ForgotPasswordCard.Header = ForgotPasswordCardHeader;
ForgotPasswordCard.Footer = ForgotPasswordCardFooter;

interface ForgotPasswordFormProps extends React.PropsWithChildren {
  form: UseFormReturn<ForgotPasswordFormSchema>;
  onSubmit: (values: ForgotPasswordFormSchema) => void;
}

export function ForgotPasswordForm(props: ForgotPasswordFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}

interface ForgotPasswordFormEmailProps {
  field: ControllerRenderProps<components["schemas"]["ForgotPasswordRequest"], "email">;
}

function ForgotPasswordFormEmail(props: ForgotPasswordFormEmailProps) {
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

interface ForgotPasswordFormSubmitButtonProps {
  isPending: boolean;
}

function ForgotPasswordFormSubmitButton(props: ForgotPasswordFormSubmitButtonProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Envoyer
      {props.isPending && <Loader />}
    </Button>
  );
}

ForgotPasswordForm.Email = ForgotPasswordFormEmail;
ForgotPasswordForm.SubmitButton = ForgotPasswordFormSubmitButton;
