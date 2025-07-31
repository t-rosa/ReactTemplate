import { $client } from "@/lib/api/client";
import { createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth")({
  async beforeLoad() {
    const query = await $client.GET("/manage/info");
    if (query.response.ok) {
      redirect({ to: "/dashboard", throw: true });
    }
  },
});
