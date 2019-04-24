import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AppService {
  constructor(private httpClient: HttpClient) { }

  public getAllApps(): Observable<App[]> {
    return this.httpClient.get<App[]>("/api/apps/all");
  }

  public getApp(name: string): Observable<App> {
    return this.httpClient.get<App>(`/api/apps/${name}`);
  }
}
