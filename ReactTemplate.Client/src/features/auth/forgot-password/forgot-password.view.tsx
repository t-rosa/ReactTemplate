import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { AuthCard } from "../../../../features/auth/components/auth-card";
import { AuthLayout } from "../../../../features/auth/components/auth-layout";
import { ForgotPasswordForm } from "./components/form";
import { useForgotPasswordView } from "./forgot-password.hooks";

export function ForgotPasswordView() {
  const { status, form, handleSubmit } = useForgotPasswordView();

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
