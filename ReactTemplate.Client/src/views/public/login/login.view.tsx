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
import { Link, useNavigate } from "@tanstack/react-router";
import { Loader2Icon } from "lucide-react";
import { useForm } from "react-hook-form";
import { z } from "zod";

const formSchema = z.object({
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
});

type LoginFormSchema = z.infer<typeof formSchema>;

export function Login() {
  const navigate = useNavigate();
  const { mutate, status } = $api.useMutation("post", "/login", {
    async onSuccess() {
      await navigate({ to: "/dashboard" });
    },
  });

  const form = useForm<LoginFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  function onSubmit(values: LoginFormSchema) {
    mutate({
      body: values,
      params: {
        query: {
          useCookies: true,
        },
      },
    });
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
              <Link to="/forgot-password">Mot de passe oublié ?</Link>
              <FormControl>
                <Input type="password" placeholder="Mot de passe" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit" disabled={status === "pending"}>
          Connexion
          {status === "pending" && <Loader2Icon className="animate-spin" />}
        </Button>
      </form>
    </Form>
  );
}
