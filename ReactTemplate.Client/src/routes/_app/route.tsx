import { $api, $client } from "@/lib/api/client";
import { AppView } from "@/modules/app/app.view";
import { createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/_app")({
  async beforeLoad() {
    const query = await $client.GET("/api/users/me");
    if (!query.response.ok) {
      redirect({
        to: "/login",
        throw: true,
      });
    }
  },
  loader({ context }) {
    return context.queryClient.ensureQueryData($api.queryOptions("get", "/api/users/me"));
  },
  component: AppView,
});
