export default class SignInError extends Error {
  constructor(
    public readonly silentError: any,
    public readonly popupError: any,
    public readonly redirectError: any
  ) {
    super('Unable to sign in via silent, popup, or redirect methods');
  }
}
