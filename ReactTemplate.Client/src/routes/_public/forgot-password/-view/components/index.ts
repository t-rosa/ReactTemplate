import { _ForgotPasswordCard } from "./card";
import { _ForgotPasswordCardContent } from "./content";
import { _ForgotPasswordFormEmail } from "./email";
import { _ForgotPasswordCardFooter } from "./footer";
import { _ForgotPasswordForm } from "./form";
import { _ForgotPasswordCardHeader } from "./header";
import { _ForgotPasswordFormSubmit } from "./submit";

export * from "./layout";

export const ForgotPasswordCard = Object.assign(_ForgotPasswordCard, {
  Content: _ForgotPasswordCardContent,
  Header: _ForgotPasswordCardHeader,
  Footer: _ForgotPasswordCardFooter,
});

export const ForgotPasswordForm = Object.assign(_ForgotPasswordForm, {
  Email: _ForgotPasswordFormEmail,
  Submit: _ForgotPasswordFormSubmit,
});
