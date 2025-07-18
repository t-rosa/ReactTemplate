import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { SuccessState } from "./reset-password.ui";

const formSchema = z
  .object({
    email: z.email({
      error: "L'email est invalide",
    }),
    resetCode: z.string({
      error: "Le code est invalide",
    }),
    newPassword: z
      .string({
        error: "Le mot de passe est invalide",
      })
      .min(6, "Le mot de passe doit contenir au moins 6 caractères.")
      .regex(/[A-Z]/, "Le mot de passe doit contenir au moins une lettre majuscule.")
      .regex(/[a-z]/, "Le mot de passe doit contenir au moins une lettre minuscule.")
      .regex(/[0-9]/, "Le mot de passe doit contenir au moins un chiffre.")
      .regex(/[^a-zA-Z0-9]/, "Le mot de passe doit contenir au moins un caractère spécial."),
    confirmPassword: z
      .string({
        error: "Le mot de passe est invalide",
      })
      .min(6, "Le mot de passe doit contenir au moins 6 caractères.")
      .regex(/[A-Z]/, "Le mot de passe doit contenir au moins une lettre majuscule.")
      .regex(/[a-z]/, "Le mot de passe doit contenir au moins une lettre minuscule.")
      .regex(/[0-9]/, "Le mot de passe doit contenir au moins un chiffre.")
      .regex(/[^a-zA-Z0-9]/, "Le mot de passe doit contenir au moins un caractère spécial."),
  })
  .refine((values) => values.newPassword === values.confirmPassword, {
    error: "Les mots de passe ne correspondent pas.",
    path: ["confirmPassword"],
  });

type ResetPasswordFormSchema = z.infer<typeof formSchema>;

export function ResetPassword() {
  const { mutate, status } = $api.useMutation("post", "/resetPassword");

  const form = useForm<ResetPasswordFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
      resetCode: "",
      newPassword: "",
      confirmPassword: "",
    },
  });

  function onSubmit(values: ResetPasswordFormSchema) {
    mutate({
      body: {
        email: values.email,
        newPassword: values.newPassword,
        resetCode: values.resetCode,
      },
    });
  }

  if (status === "success") {
    return <SuccessState />;
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input type="email" placeholder="example@react-template.fr" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="resetCode"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Code de réinitialisation</FormLabel>
              <FormControl>
                <Input type="password" placeholder="Code" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="newPassword"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Nouveau mot de passe</FormLabel>
              <FormControl>
                <Input type="password" placeholder="Mot de passe" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="confirmPassword"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Confirmation</FormLabel>
              <FormControl>
                <Input type="password" placeholder="Confirmation" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit" disabled={status === "pending"}>
          Réinitialiser
          {status === "pending" && <Loader />}
        </Button>
      </form>
    </Form>
  );
}
