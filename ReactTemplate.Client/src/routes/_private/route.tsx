import { $api, $client } from "@/lib/api/client";
import { PrivateView } from "@/views/private/private.view";
import { createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/_private")({
  async beforeLoad() {
    const query = await $client.GET("/manage/info");
    if (!query.response.ok) {
      redirect({
        to: "/login",
        throw: true,
      });
    }
  },
  loader({ context }) {
    return context.queryClient.ensureQueryData($api.queryOptions("get", "/manage/info"));
  },
  component: PrivateView,
});
