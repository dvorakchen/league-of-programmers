import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AdminMenuService {

  constructor(
    private http: HttpClient
  ) { }

  /**
   * 获取菜单
   */
  getMenus() {
    return this.http.get(`assets/data/menu.json`);
  }
}
