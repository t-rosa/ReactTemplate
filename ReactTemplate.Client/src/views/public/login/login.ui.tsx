import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { components } from "@/lib/api/schema";
import { Link } from "@tanstack/react-router";
import { FrameIcon, Loader2Icon } from "lucide-react";
import * as React from "react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { LoginFormSchema } from "./login.types";

export function LoginLayout(props: React.PropsWithChildren) {
  return (
    <main className="overflow-hidden">
      <div className="isolate flex min-h-dvh items-center justify-center p-6 lg:p-8">
        {props.children}
      </div>
    </main>
  );
}

export function LoginCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function LoginCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

function LoginCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Bienvenue !</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">Connectez-vous pour continuer.</p>
    </div>
  );
}

function LoginCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

LoginCard.Content = LoginCardContent;
LoginCard.Header = LoginCardHeader;
LoginCard.Footer = LoginCardFooter;

interface LoginFormProps extends React.PropsWithChildren {
  form: UseFormReturn<LoginFormSchema>;
  onSubmit: (values: LoginFormSchema) => void;
}

export function LoginForm(props: LoginFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}

interface LoginFormEmailProps {
  field: ControllerRenderProps<components["schemas"]["LoginRequest"], "email">;
}

function LoginFormEmail(props: LoginFormEmailProps) {
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

interface LoginFormPasswordProps {
  field: ControllerRenderProps<components["schemas"]["LoginRequest"], "password">;
}

function LoginFormPassword(props: LoginFormPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Mot de passe" {...props.field} />
      </FormControl>
      <Link
        to="/forgot-password"
        className="text-foreground/80 inline-flex w-fit justify-start p-0 text-sm/5 font-normal hover:underline"
      >
        Mot de passe oublié ?
      </Link>
      <FormMessage />
    </FormItem>
  );
}

interface LoginFormSubmitButtonProps {
  isPending: boolean;
}

function LoginFormSubmitButton(props: LoginFormSubmitButtonProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Connexion
      {props.isPending && <Loader2Icon className="animate-spin" />}
    </Button>
  );
}

LoginForm.Email = LoginFormEmail;
LoginForm.Password = LoginFormPassword;
LoginForm.SubmitButton = LoginFormSubmitButton;
