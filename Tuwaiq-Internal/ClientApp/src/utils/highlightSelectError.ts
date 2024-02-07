
export const highlightSelectError = (error: string | undefined, value: any) => {
    const element = value instanceof HTMLElement ? value : document.getElementById(value);
    error ? element?.classList.add('!border-red-600') : element?.classList.remove('!border-red-600');
}