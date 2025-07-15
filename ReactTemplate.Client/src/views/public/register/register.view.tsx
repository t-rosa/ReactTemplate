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
import { Loader2Icon } from "lucide-react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { SuccessState } from "./register.ui";

const formSchema = z
  .object({
    email: z.email({
      error: "L'email est invalide",
    }),
    password: z
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
  .refine((values) => values.password === values.confirmPassword, {
    error: "Les mots de passe ne correspondent pas.",
    path: ["confirmPassword"],
  });

type RegisterFormSchema = z.infer<typeof formSchema>;

export function Register() {
  const { mutate, status } = $api.useMutation("post", "/register", {});

  const form = useForm<RegisterFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  function onSubmit(values: RegisterFormSchema) {
    mutate({
      body: {
        email: values.email,
        password: values.password,
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
          name="password"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Mot de passe</FormLabel>
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
          {status === "pending" && <Loader2Icon className="animate-spin" />}
          S&apos;enregistrer
        </Button>
      </form>
    </Form>
  );
}
