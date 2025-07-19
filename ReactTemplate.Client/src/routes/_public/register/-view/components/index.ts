import { _RegisterCard } from "./card";
import { _RegisterFormConfirmPassword } from "./confirm-password";
import { _RegisterCardContent } from "./content";
import { _RegisterFormEmail } from "./email";
import { _RegisterCardFooter } from "./footer";
import { _RegisterForm } from "./form";
import { _RegisterCardHeader } from "./header";
import { _RegisterFormPassword } from "./password";
import { _RegisterFormSubmit } from "./submit";
import { _RegisterCardSuccess } from "./success";

export * from "./layout";

export const RegisterCard = Object.assign(_RegisterCard, {
  Content: _RegisterCardContent,
  Header: _RegisterCardHeader,
  Footer: _RegisterCardFooter,
  Success: _RegisterCardSuccess,
});

export const RegisterForm = Object.assign(_RegisterForm, {
  Email: _RegisterFormEmail,
  Password: _RegisterFormPassword,
  ConfirmPassword: _RegisterFormConfirmPassword,
  Submit: _RegisterFormSubmit,
});
