export default class SignOutError extends Error {
  constructor(
    public readonly popupError: any,
    public readonly redirectError: any
  ) {
    super('Unable to sign out via popup or redirect methods');
  }
}
