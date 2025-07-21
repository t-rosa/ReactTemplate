export function useErrorView(error?: string) {
  async function handleCopyClick() {
    if (error) {
      await navigator.clipboard.writeText(error);
    }
  }

  function handleReload() {
    location.reload();
  }

  return { handleCopyClick, handleReload };
}
