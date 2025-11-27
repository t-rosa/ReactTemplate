import { $client } from "@/lib/api/client";
import { AuthView } from "@/modules/auth/auth.view";
import { createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth")({
  async beforeLoad() {
    const query = await $client.GET("/api/users/me");
    if (query.response.ok) {
      redirect({ to: "/forecasts", throw: true });
    }
  },
  component: AuthView,
});
