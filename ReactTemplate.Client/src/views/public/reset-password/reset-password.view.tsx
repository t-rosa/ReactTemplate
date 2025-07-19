import { FormField } from "@/components/ui/form";
import { Link } from "@tanstack/react-router";
import { useResetPasswordView } from "./reset-password.hook";
import {
  ResetPasswordCard,
  ResetPasswordForm,
  ResetPasswordLayout,
  ResetPasswordSuccessState,
} from "./reset-password.ui";

export function ResetPassword() {
  const { status, form, handleSubmit } = useResetPasswordView();

  if (status === "success") {
    return (
      <ResetPasswordLayout>
        <ResetPasswordCard>
          <ResetPasswordCard.Content>
            <ResetPasswordCard.Header />
            <ResetPasswordSuccessState />
          </ResetPasswordCard.Content>
          <ResetPasswordCard.Footer>
            <Link to="/login">Se connecter</Link>
          </ResetPasswordCard.Footer>
        </ResetPasswordCard>
      </ResetPasswordLayout>
    );
  }

  return (
    <ResetPasswordLayout>
      <ResetPasswordCard>
        <ResetPasswordCard.Content>
          <ResetPasswordCard.Header />
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
            <ResetPasswordForm.SubmitButton isPending={status === "pending"} />
          </ResetPasswordForm>
        </ResetPasswordCard.Content>
        <ResetPasswordCard.Footer>
          <Link to="/login">Se connecter</Link>
        </ResetPasswordCard.Footer>
      </ResetPasswordCard>
    </ResetPasswordLayout>
  );
}
