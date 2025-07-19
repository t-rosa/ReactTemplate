import * as React from "react";

export function LoginLayout(props: React.PropsWithChildren) {
  return (
    <main className="overflow-hidden">
      <div className="isolate flex min-h-dvh items-center justify-center p-6 lg:p-8">
        {props.children}
      </div>
    </main>
  );
}
