/**
 * 拦截器桶
 */

import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { DefaultInterceptor } from './default-interceptors';

/** Http interceptor providers in outside-in order */
export const HTTP_INTERCEPTOR_PROVIDERS = [
  { provide: HTTP_INTERCEPTORS, useClass: DefaultInterceptor, multi: true },
];
