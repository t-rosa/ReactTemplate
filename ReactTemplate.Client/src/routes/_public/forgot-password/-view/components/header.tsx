import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";

export function _ForgotPasswordCardHeader() {
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
