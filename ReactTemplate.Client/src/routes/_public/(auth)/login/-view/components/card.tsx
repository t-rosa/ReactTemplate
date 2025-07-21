import { Link } from "@tanstack/react-router";
import { FrameIcon } from "lucide-react";
import * as React from "react";

function _LoginCard(props: React.PropsWithChildren) {
  return (
    <div className="bg-card/50 ring-border w-full max-w-md rounded-xl shadow-md ring-1">
      {props.children}
    </div>
  );
}

function _LoginCardContent(props: React.PropsWithChildren) {
  return <div className="p-7 sm:p-11">{props.children}</div>;
}

function _LoginCardFooter(props: React.PropsWithChildren) {
  return (
    <div className="ring-border bg-card m-1.5 rounded-lg py-4 text-center text-sm/5 ring-1">
      {props.children}
    </div>
  );
}

function _LoginCardHeader() {
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

export const LoginCard = Object.assign(_LoginCard, {
  Content: _LoginCardContent,
  Header: _LoginCardHeader,
  Footer: _LoginCardFooter,
});
