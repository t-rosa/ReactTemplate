import { $api } from "@/lib/api/client";

export type UserRole = "Admin" | "Member" | "User";

export function useUser() {
  const { data: user } = $api.useSuspenseQuery("get", "/api/users/me");

  function hasRole(role: UserRole) {
    return user.roles.includes(role) ?? false;
  }

  return { user, hasRole };
}
