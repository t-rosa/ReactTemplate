import { FormField } from "@/components/ui/form";
import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { Link, useNavigate } from "@tanstack/react-router";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { LoginCard, LoginForm, LoginLayout } from "./login.ui";

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

export type LoginFormSchema = z.infer<typeof formSchema>;

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

  function handleSubmit(values: LoginFormSchema) {
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
    <LoginLayout>
      <LoginCard>
        <LoginCard.Content>
          <LoginCard.Header />
          <LoginForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={LoginForm.Email} />
            <FormField control={form.control} name="password" render={LoginForm.Password} />
            <LoginForm.SubmitButton isPending={status === "pending"} />
          </LoginForm>
        </LoginCard.Content>
        <LoginCard.Footer>
          <Link to="/register">Créer un compte</Link>
        </LoginCard.Footer>
      </LoginCard>
    </LoginLayout>
  );
}
