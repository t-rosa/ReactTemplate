import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";

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
