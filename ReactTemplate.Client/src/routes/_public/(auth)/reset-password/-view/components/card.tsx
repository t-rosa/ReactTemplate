import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";

function _ResetPasswordCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function _ResetPasswordCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

export function _ResetPasswordCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Réinitialiser.</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">
        Veuillez saisir votre adresse e-mail afin de réinitialiser votre mot de passe.
      </p>
    </div>
  );
}

function _ResetPasswordCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

function _ResetPasswordCardSuccess() {
  return (
    <div className="grid gap-2">
      <p>Mot de passe réinitialisé avec succès !</p>
      <p>Vous pouvez désormais vous connecter avec votre nouveau mot de passe.</p>
    </div>
  );
}

export const ResetPasswordCard = Object.assign(_ResetPasswordCard, {
  Content: _ResetPasswordCardContent,
  Header: _ResetPasswordCardHeader,
  Footer: _ResetPasswordCardFooter,
  Success: _ResetPasswordCardSuccess,
});
