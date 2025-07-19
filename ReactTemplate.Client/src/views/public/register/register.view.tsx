import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { useRegisterView } from "./register.hook";
import { RegisterCard, RegisterForm, RegisterLayout, RegisterSuccessState } from "./register.ui";

export function Register() {
  const { status, form, handleSubmit } = useRegisterView();

  if (status === "success") {
    return (
      <RegisterLayout>
        <RegisterCard>
          <RegisterCard.Content>
            <RegisterCard.Header />
            <RegisterSuccessState />
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
            <RegisterForm.SubmitButton isPending={status === "pending"} />
          </RegisterForm>
        </RegisterCard.Content>
        <RegisterCard.Footer>
          <Link to="/login">Se connecter</Link>
        </RegisterCard.Footer>
      </RegisterCard>
    </RegisterLayout>
  );
}
