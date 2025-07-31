import { queryCollectionOptions } from "@tanstack/query-db-collection";
import { createCollection } from "@tanstack/react-db";
import { $api, $client } from "./api/client";
import { queryClient } from "./query-client";

export const forecastsCollection = createCollection(
  queryCollectionOptions({
    id: "weather-forecasts",
    queryKey: $api.queryOptions("get", "/api/weather-forecasts").queryKey,
    refetchInterval: 5000,
    queryFn: async () => {
      const query = await $client.GET("/api/weather-forecasts");
      return query.data!;
    },
    queryClient,
    getKey: (item) => item.id!,
    onInsert: async ({ transaction }) => {
      await Promise.all(
        transaction.mutations.map((mutation) =>
          $client.POST("/api/weather-forecasts", { body: mutation.modified }),
        ),
      );
    },
    onUpdate: async ({ transaction }) => {
      const { original, modified } = transaction.mutations[0];
      await $client.PUT("/api/weather-forecasts/{id}", {
        params: { path: { id: original.id! } },
        body: modified,
      });
    },
    onDelete: async ({ transaction }) => {
      const { original } = transaction.mutations[0];
      await $client.DELETE("/api/weather-forecasts/{id}", {
        params: { path: { id: original.id! } },
      });
    },
  }),
);
