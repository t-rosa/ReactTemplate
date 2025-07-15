import { NotFoundScreen } from "@/components/not-found-screen";
import { PendingScreen } from "@/components/pending-screen";
import { routeTree } from "@/routeTree.gen";
import { Error } from "@/views/error/error.view";
import { createRouter } from "@tanstack/react-router";
import { queryClient } from "./query-client";

export const router = createRouter({
  routeTree,
  defaultNotFoundComponent: NotFoundScreen,
  defaultPendingComponent: PendingScreen,
  defaultErrorComponent: Error,
  defaultPreloadStaleTime: 0,
  scrollRestoration: true,
  defaultViewTransition: true,
  defaultPreload: "intent",
  context: {
    queryClient,
  },
});

declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}
