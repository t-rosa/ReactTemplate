import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { AuthCard } from "../../../../features/auth/components/auth-card";
import { AuthLayout } from "../../../../features/auth/components/auth-layout";
import { LoginForm } from "./components/form";
import { useLoginView } from "./login.hooks";

export function LoginView() {
  const { status, form, handleSubmit } = useLoginView();

  return (
    <AuthLayout>
      <AuthCard>
        <AuthCard.Content>
          <AuthCard.Header>
            <AuthCard.Title>Bienvenue !</AuthCard.Title>
            <AuthCard.Description>Connectez-vous pour continuer.</AuthCard.Description>
          </AuthCard.Header>
          <LoginForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={LoginForm.Email} />
            <FormField control={form.control} name="password" render={LoginForm.Password} />
            <LoginForm.Submit isPending={status === "pending"} />
          </LoginForm>
        </AuthCard.Content>
        <AuthCard.Footer>
          <Link to="/register">Créer un compte</Link>
        </AuthCard.Footer>
      </AuthCard>
    </AuthLayout>
  );
}
