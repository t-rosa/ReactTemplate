import { Outlet } from "@tanstack/react-router";
import { AuthLayout } from "./auth.ui";

export function AuthView() {
  return (
    <AuthLayout>
      <Outlet />
    </AuthLayout>
  );
}
