import { Component, OnInit, Inject } from "@angular/core";
import { AppService } from "src/app/core/services/app.service";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { AppTitleService } from "src/app/core/services/app-title.service";

@Component({
  selector: "app-specific-app",
  templateUrl: "./specific-app.component.html",
  styleUrls: ["./specific-app.component.sass"]
})
export class SpecificAppComponent implements OnInit {
  public app: Observable<App>;

  constructor(private route: ActivatedRoute, private titleService: AppTitleService, private appService: AppService) { }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      const appName: string = params.name;
      this.titleService.chooseApp(appName);
      this.renderApp(appName);
    });
  }

  private renderApp(name: string): void {
    this.app = this.appService.getApp(name);
  }
}
