import { $client } from "@/lib/api/client";
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
});
