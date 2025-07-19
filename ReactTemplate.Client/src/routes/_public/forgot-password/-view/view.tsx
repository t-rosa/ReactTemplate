import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { ForgotPasswordCard, ForgotPasswordForm, ForgotPasswordLayout } from "./components";
import { useForgotPasswordView } from "./hooks";

export function ForgotPasswordView() {
  const { status, form, handleSubmit } = useForgotPasswordView();

  return (
    <ForgotPasswordLayout>
      <ForgotPasswordCard>
        <ForgotPasswordCard.Content>
          <ForgotPasswordCard.Header />
          <ForgotPasswordForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={ForgotPasswordForm.Email} />
            <ForgotPasswordForm.Submit isPending={status === "pending"} />
          </ForgotPasswordForm>
        </ForgotPasswordCard.Content>
        <ForgotPasswordCard.Footer>
          <Link to="/login">Se connecter</Link>
        </ForgotPasswordCard.Footer>
      </ForgotPasswordCard>
    </ForgotPasswordLayout>
  );
}
