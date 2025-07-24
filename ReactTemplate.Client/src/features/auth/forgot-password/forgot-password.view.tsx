import { FormField } from "@/components/ui/form";
import { AuthCard } from "@/features/auth/components/auth-card";
import { AuthLayout } from "@/features/auth/components/auth-layout";
import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { Link, useNavigate } from "@tanstack/react-router";
import { useForm } from "react-hook-form";
import { ForgotPasswordForm } from "./components/form";
import { formSchema, type ForgotPasswordFormSchema } from "./forgot-password.types";

export function ForgotPasswordView() {
  const navigate = useNavigate();

  const { mutate, status } = $api.useMutation("post", "/forgotPassword", {
    async onSuccess() {
      await navigate({ to: "/reset-password" });
    },
  });

  const form = useForm<ForgotPasswordFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
    },
  });

  function handleSubmit(values: ForgotPasswordFormSchema) {
    mutate({
      body: values,
    });
  }

  return (
    <AuthLayout>
      <AuthCard>
        <AuthCard.Content>
          <AuthCard.Header>
            <AuthCard.Title>Mot de passe oublié.</AuthCard.Title>
            <AuthCard.Description>Saisissez votre adresse e-mail.</AuthCard.Description>
          </AuthCard.Header>
          <ForgotPasswordForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={ForgotPasswordForm.Email} />
            <ForgotPasswordForm.Submit isPending={status === "pending"} />
          </ForgotPasswordForm>
        </AuthCard.Content>
        <AuthCard.Footer>
          <Link to="/login">Se connecter</Link>
        </AuthCard.Footer>
      </AuthCard>
    </AuthLayout>
  );
}
