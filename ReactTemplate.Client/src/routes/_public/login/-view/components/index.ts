import { _LoginCard } from "./card";
import { _LoginCardContent } from "./content";
import { _LoginFormEmail } from "./email";
import { _LoginCardFooter } from "./footer";
import { _LoginForm } from "./form";
import { _LoginCardHeader } from "./header";
import { _LoginFormPassword } from "./password";
import { _LoginFormSubmit } from "./submit";

export * from "./layout";

export const LoginCard = Object.assign(_LoginCard, {
  Content: _LoginCardContent,
  Header: _LoginCardHeader,
  Footer: _LoginCardFooter,
});

export const LoginForm = Object.assign(_LoginForm, {
  Email: _LoginFormEmail,
  Password: _LoginFormPassword,
  Submit: _LoginFormSubmit,
});
