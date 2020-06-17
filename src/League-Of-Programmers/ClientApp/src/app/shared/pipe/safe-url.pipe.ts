import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
@Pipe({
  name: 'safeUrl'
})
export class SafeUrlPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {
  }
  transform(url: any, args?: any): any {
    // 允许通过不安全的 url
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}
