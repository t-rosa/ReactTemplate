import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { AuthCard } from "../../../../features/auth/components/auth-card";
import { AuthLayout } from "../../../../features/auth/components/auth-layout";
import { ResetPasswordForm } from "./components/form";
import { SuccessState } from "./components/success-state";
import { useResetPasswordView } from "./reset-password.hooks";

export function ResetPasswordView() {
  const { status, form, handleSubmit } = useResetPasswordView();

  if (status === "success") {
    return (
      <AuthLayout>
        <AuthCard>
          <AuthCard.Content>
            <AuthCard.Header>
              <AuthCard.Title>Réinitialiser.</AuthCard.Title>
              <AuthCard.Description>
                Veuillez saisir votre adresse e-mail afin de réinitialiser votre mot de passe.
              </AuthCard.Description>
            </AuthCard.Header>
            <SuccessState />
          </AuthCard.Content>
          <AuthCard.Footer>
            <Link to="/login">Se connecter</Link>
          </AuthCard.Footer>
        </AuthCard>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout>
      <AuthCard>
        <AuthCard.Content>
          <AuthCard.Header>
            <AuthCard.Title>Réinitialiser.</AuthCard.Title>
            <AuthCard.Description>
              Veuillez saisir votre adresse e-mail afin de réinitialiser votre mot de passe.
            </AuthCard.Description>
          </AuthCard.Header>
          <ResetPasswordForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={ResetPasswordForm.Email} />
            <FormField
              control={form.control}
              name="resetCode"
              render={ResetPasswordForm.ResetCode}
            />
            <FormField
              control={form.control}
              name="newPassword"
              render={ResetPasswordForm.NewPassword}
            />
            <FormField
              control={form.control}
              name="confirmPassword"
              render={ResetPasswordForm.ConfirmPassword}
            />
            <ResetPasswordForm.Submit isPending={status === "pending"} />
          </ResetPasswordForm>
        </AuthCard.Content>
        <AuthCard.Footer>
          <Link to="/login">Se connecter</Link>
        </AuthCard.Footer>
      </AuthCard>
    </AuthLayout>
  );
}
