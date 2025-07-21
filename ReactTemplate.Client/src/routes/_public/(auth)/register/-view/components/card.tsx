import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";

function _RegisterCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function _RegisterCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

function _RegisterCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

function _RegisterCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Créer un compte.</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">
        Enregistrez-vous pour vous connecter à l&apos;application.
      </p>
    </div>
  );
}

function _RegisterCardSuccess() {
  return (
    <div className="grid gap-2">
      <p>Votre compte a été créé avec succès !</p>
      <p>
        Un e-mail de confirmation vous a été envoyé, merci de le valider avant de vous connecter.
      </p>
    </div>
  );
}

export const RegisterCard = Object.assign(_RegisterCard, {
  Content: _RegisterCardContent,
  Header: _RegisterCardHeader,
  Footer: _RegisterCardFooter,
  Success: _RegisterCardSuccess,
});
