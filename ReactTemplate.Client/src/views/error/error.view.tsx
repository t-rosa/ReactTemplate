import { useErrorView } from "./error.hooks";
import { ErrorCard, ErrorLayout } from "./error.ui";

interface ErrorProps {
  error: { message?: string };
  reset: () => void;
}

export function ErrorView(props: ErrorProps) {
  const { handleReload, handleCopyClick } = useErrorView(props.error.message);

  return (
    <ErrorLayout>
      <ErrorCard>
        <ErrorCard.Container>
          <ErrorCard.Header />
          <ErrorCard.Content
            error={props.error}
            onReloadClick={handleReload}
            onCopyClick={handleCopyClick}
          />
        </ErrorCard.Container>
      </ErrorCard>
    </ErrorLayout>
  );
}
