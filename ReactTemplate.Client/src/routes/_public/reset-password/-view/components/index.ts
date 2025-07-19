import { _ResetPasswordCard } from "./card";
import { _ResetPasswordFormConfirmPassword } from "./confirm-password";
import { _ResetPasswordCardContent } from "./content";
import { _ResetPasswordFormEmail } from "./email";
import { _ResetPasswordCardFooter } from "./footer";
import { _ResetPasswordForm } from "./form";
import { _ResetPasswordCardHeader } from "./header";
import { _ResetPasswordFormNewPassword } from "./new-password";
import { _ResetPasswordFormResetCode } from "./reset-code";
import { _ResetPasswordFormSubmit } from "./submit";
import { _ResetPasswordCardSuccess } from "./success";

export * from "./layout";

export const ResetPasswordCard = Object.assign(_ResetPasswordCard, {
  Content: _ResetPasswordCardContent,
  Header: _ResetPasswordCardHeader,
  Footer: _ResetPasswordCardFooter,
  Success: _ResetPasswordCardSuccess,
});

export const ResetPasswordForm = Object.assign(_ResetPasswordForm, {
  Email: _ResetPasswordFormEmail,
  ResetCode: _ResetPasswordFormResetCode,
  NewPassword: _ResetPasswordFormNewPassword,
  ConfirmPassword: _ResetPasswordFormConfirmPassword,
  Submit: _ResetPasswordFormSubmit,
});
