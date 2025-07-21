import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { RegisterCard } from "./components/card";
import { RegisterForm } from "./components/form";
import { RegisterLayout } from "./components/layout";
import { useRegisterView } from "./hook";

export function RegisterView() {
  const { status, form, handleSubmit } = useRegisterView();

  if (status === "success") {
    return (
      <RegisterLayout>
        <RegisterCard>
          <RegisterCard.Content>
            <RegisterCard.Header />
            <RegisterCard.Success />
          </RegisterCard.Content>
          <RegisterCard.Footer>
            <Link to="/login">Se connecter</Link>
          </RegisterCard.Footer>
        </RegisterCard>
      </RegisterLayout>
    );
  }

  return (
    <RegisterLayout>
      <RegisterCard>
        <RegisterCard.Content>
          <RegisterCard.Header />
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
        </RegisterCard.Content>
        <RegisterCard.Footer>
          <Link to="/login">Se connecter</Link>
        </RegisterCard.Footer>
      </RegisterCard>
    </RegisterLayout>
  );
}
