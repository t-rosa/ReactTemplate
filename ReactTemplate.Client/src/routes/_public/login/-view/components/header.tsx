import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";

export function _LoginCardHeader() {
  return (
    <div className="mb-8">
      <div className="flex items-start">
        <Link to="/" title="Home">
          <FrameIcon className="h-9 fill-black" />
        </Link>
      </div>
      <h1 className="mt-8 text-base/6 font-medium">Bienvenue !</h1>
      <p className="text-muted-foreground mt-1 text-sm/5">Connectez-vous pour continuer.</p>
    </div>
  );
}
