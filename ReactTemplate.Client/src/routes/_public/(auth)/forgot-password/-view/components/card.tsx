import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";

function _ForgotPasswordCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function _ForgotPasswordCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

function _ForgotPasswordCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Mot de passe oublié.</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">Saisissez votre adresse e-mail.</p>
    </div>
  );
}

function _ForgotPasswordCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

export const ForgotPasswordCard = Object.assign(_ForgotPasswordCard, {
  Content: _ForgotPasswordCardContent,
  Header: _ForgotPasswordCardHeader,
  Footer: _ForgotPasswordCardFooter,
});
