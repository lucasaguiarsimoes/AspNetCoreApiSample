/**
 * Válida se os texto é nulo, undefined ou vazio
 */
export function isNullOrEmpty(text: string) {
  return (isNullOrUndefined(text) || text === '');
}

/**
 * Função para verificar se um valor está Nulo ou Indefinido
 *
 * @param value Valor a ser validado
 */
export function isNullOrUndefined(value: any): boolean {
  return value === null || value === undefined;
}

/**
 * Realiza o scroll na tela até a primeira ocorrência de elemento com a classe informada
 */
export function scrollIntoClass(document: Document, classe: string, options: ScrollIntoViewOptions) {
  setTimeout(() => {
    const firstOcurrence = document.querySelector(classe);

    if (!isNullOrUndefined(firstOcurrence)) {
      firstOcurrence.scrollIntoView(options);
    }
  }, 500);
}
