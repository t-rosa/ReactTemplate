import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { useLoginView } from "./login.hook";
import { LoginCard, LoginForm, LoginLayout } from "./login.ui";

export function Login() {
  const { status, form, handleSubmit } = useLoginView();

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
