import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/")({
  component: Index,
});

export function Index() {
  return (
    <ul>
      <li>
        <Link to="/login">login</Link>
      </li>
      <li>
        <Link to="/register">register</Link>
      </li>
      <li>
        <Link to="/reset-password">reset-password</Link>
      </li>
      <li>
        <Link to="/forgot-password">forgot-password</Link>
      </li>
      <li>
        <Link to="/dashboard">dashboard</Link>
      </li>
    </ul>
  );
}
