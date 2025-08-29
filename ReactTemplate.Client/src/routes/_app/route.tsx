import { $api, $client } from "@/lib/api/client";
import { PrivateView } from "@/views/app/private.view";
import { createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/_app")({
  async beforeLoad() {
    const query = await $client.GET("/api/auth/info");
    if (!query.response.ok) {
      redirect({
        to: "/login",
        throw: true,
      });
    }
  },
  loader({ context }) {
    return context.queryClient.ensureQueryData($api.queryOptions("get", "/api/auth/info"));
  },
  component: PrivateView,
});
