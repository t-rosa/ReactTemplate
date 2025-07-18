import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { useForgotPasswordView } from "./forgot-password.hook";
import { ForgotPasswordCard, ForgotPasswordForm, ForgotPasswordLayout } from "./forgot-password.ui";

export function ForgotPassword() {
  const { status, form, handleSubmit } = useForgotPasswordView();

  return (
    <ForgotPasswordLayout>
      <ForgotPasswordCard>
        <ForgotPasswordCard.Content>
          <ForgotPasswordCard.Header />
          <ForgotPasswordForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={ForgotPasswordForm.Email} />
            <ForgotPasswordForm.SubmitButton isPending={status === "pending"} />
          </ForgotPasswordForm>
        </ForgotPasswordCard.Content>
        <ForgotPasswordCard.Footer>
          <Link to="/login">Se connecter</Link>
        </ForgotPasswordCard.Footer>
      </ForgotPasswordCard>
    </ForgotPasswordLayout>
  );
}
