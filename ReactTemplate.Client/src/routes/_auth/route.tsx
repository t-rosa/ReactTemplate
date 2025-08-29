import { $client } from "@/lib/api/client";
import { createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth")({
  async beforeLoad() {
    const query = await $client.GET("/api/auth/info");
    if (query.response.ok) {
      redirect({ to: "/forecasts", throw: true });
    }
  },
});
