import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";
import * as React from "react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { RegisterFormSchema } from "./register.types";

export function RegisterSuccessState() {
  return (
    <div className="grid gap-2">
      <p>Votre compte a été créé avec succès !</p>
      <p>
        Un e-mail de confirmation vous a été envoyé, merci de le valider avant de vous connecter.
      </p>
    </div>
  );
}

export function RegisterLayout(props: React.PropsWithChildren) {
  return (
    <main className="overflow-hidden">
      <div className="isolate flex min-h-dvh items-center justify-center p-6 lg:p-8">
        {props.children}
      </div>
    </main>
  );
}

export function RegisterCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function RegisterCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

function RegisterCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Créer un compte.</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">
        Enregistrez-vous pour vous connecter à l&apos;application.
      </p>
    </div>
  );
}

function RegisterCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

RegisterCard.Content = RegisterCardContent;
RegisterCard.Header = RegisterCardHeader;
RegisterCard.Footer = RegisterCardFooter;

interface RegisterFormProps extends React.PropsWithChildren {
  form: UseFormReturn<RegisterFormSchema>;
  onSubmit: (values: RegisterFormSchema) => void;
}

export function RegisterForm(props: RegisterFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}

interface RegisterFormEmailProps {
  field: ControllerRenderProps<RegisterFormSchema, "email">;
}

function RegisterFormEmail(props: RegisterFormEmailProps) {
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

interface RegisterFormPasswordProps {
  field: ControllerRenderProps<RegisterFormSchema, "password">;
}

function RegisterFormPassword(props: RegisterFormPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Mot de passe" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface RegisterFormConfirmPasswordProps {
  field: ControllerRenderProps<RegisterFormSchema, "confirmPassword">;
}

function RegisterFormConfirmPassword(props: RegisterFormConfirmPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Confirmer le mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Confirmation" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface RegisterFormSubmitButtonProps {
  isPending: boolean;
}

function RegisterFormSubmitButton(props: RegisterFormSubmitButtonProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Créer un compte
      {props.isPending && <Loader />}
    </Button>
  );
}

RegisterForm.Email = RegisterFormEmail;
RegisterForm.Password = RegisterFormPassword;
RegisterForm.ConfirmPassword = RegisterFormConfirmPassword;
RegisterForm.SubmitButton = RegisterFormSubmitButton;
