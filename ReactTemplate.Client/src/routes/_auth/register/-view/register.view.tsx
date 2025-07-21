import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { AuthCard } from "../../-components/auth-card";
import { AuthLayout } from "../../-components/auth-layout";
import { RegisterForm } from "./components/form";
import { SuccessState } from "./components/success-state";
import { useRegisterView } from "./register.hooks";

export function RegisterView() {
  const { status, form, handleSubmit } = useRegisterView();

  if (status === "success") {
    return (
      <AuthLayout>
        <AuthCard>
          <AuthCard.Content>
            <AuthCard.Header>
              <AuthCard.Title>Créer un compte.</AuthCard.Title>
              <AuthCard.Description>
                Enregistrez-vous pour vous connecter à l&apos;application.
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
            <AuthCard.Title>Créer un compte.</AuthCard.Title>
            <AuthCard.Description>
              Enregistrez-vous pour vous connecter à l&apos;application.
            </AuthCard.Description>
          </AuthCard.Header>
          <RegisterForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={RegisterForm.Email} />
            <FormField control={form.control} name="password" render={RegisterForm.Password} />
            <FormField
              control={form.control}
              name="confirmPassword"
              render={RegisterForm.ConfirmPassword}
            />
            <RegisterForm.Submit isPending={status === "pending"} />
          </RegisterForm>
        </AuthCard.Content>
        <AuthCard.Footer>
          <Link to="/login">Se connecter</Link>
        </AuthCard.Footer>
      </AuthCard>
    </AuthLayout>
  );
}
