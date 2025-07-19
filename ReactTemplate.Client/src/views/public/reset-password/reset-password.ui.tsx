import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";
import * as React from "react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { ResetPasswordFormSchema } from "./reset-password.types";

export function ResetPasswordSuccessState() {
  return (
    <div className="grid gap-2">
      <p>Mot de passe réinitialisé avec succès !</p>
      <p>Vous pouvez désormais vous connecter avec votre nouveau mot de passe.</p>
    </div>
  );
}

export function ResetPasswordLayout(props: React.PropsWithChildren) {
  return (
    <main className="overflow-hidden">
      <div className="isolate flex min-h-dvh items-center justify-center p-6 lg:p-8">
        {props.children}
      </div>
    </main>
  );
}

export function ResetPasswordCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function ResetPasswordCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

function ResetPasswordCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Réinitialiser.</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">
        Veuillez saisir votre adresse e-mail afin de réinitialiser votre mot de passe.
      </p>
    </div>
  );
}

function ResetPasswordCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

ResetPasswordCard.Content = ResetPasswordCardContent;
ResetPasswordCard.Header = ResetPasswordCardHeader;
ResetPasswordCard.Footer = ResetPasswordCardFooter;

interface ResetPasswordFormProps extends React.PropsWithChildren {
  form: UseFormReturn<ResetPasswordFormSchema>;
  onSubmit: (values: ResetPasswordFormSchema) => void;
}

export function ResetPasswordForm(props: ResetPasswordFormProps) {
  return (
    <Form {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.onSubmit)} className="grid gap-6">
        {props.children}
      </form>
    </Form>
  );
}

interface ResetPasswordFormEmailProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "email">;
}

function ResetPasswordFormEmail(props: ResetPasswordFormEmailProps) {
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

interface ResetPasswordFormResetCodeProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "resetCode">;
}

function ResetPasswordFormResetCode(props: ResetPasswordFormResetCodeProps) {
  return (
    <FormItem>
      <FormLabel>Code de réinitialisation</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Code de réinitialisation" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface ResetPasswordFormNewPasswordProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "newPassword">;
}

function ResetPasswordFormNewPassword(props: ResetPasswordFormNewPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Nouveau mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Mot de passe" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface ResetPasswordFormConfirmPasswordProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "confirmPassword">;
}

function ResetPasswordFormConfirmPassword(props: ResetPasswordFormConfirmPasswordProps) {
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

interface ResetPasswordFormSubmitButtonProps {
  isPending: boolean;
}

function ResetPasswordFormSubmitButton(props: ResetPasswordFormSubmitButtonProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Créer un compte
      {props.isPending && <Loader />}
    </Button>
  );
}

ResetPasswordForm.Email = ResetPasswordFormEmail;
ResetPasswordForm.ResetCode = ResetPasswordFormResetCode;
ResetPasswordForm.NewPassword = ResetPasswordFormNewPassword;
ResetPasswordForm.ConfirmPassword = ResetPasswordFormConfirmPassword;
ResetPasswordForm.SubmitButton = ResetPasswordFormSubmitButton;
