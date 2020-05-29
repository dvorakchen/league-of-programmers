export class Global {
  private static USERNAME_STORE_KEY = '__username__';

  static get userName(): string {
    return localStorage.getItem(this.USERNAME_STORE_KEY);
  }
  static set userName(name: string) {
    localStorage.setItem(this.USERNAME_STORE_KEY, name);
  }
}
