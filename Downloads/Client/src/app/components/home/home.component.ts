import { Component, OnInit } from "@angular/core";
import { AppService } from "src/app/core/services/app.service";
import { Observable } from "rxjs";
import { AppTitleService } from "src/app/core/services/app-title.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.sass"]
})
export class HomeComponent implements OnInit {
  public apps: Observable<App[]>;

  constructor(private appService: AppService, private appTitleService: AppTitleService) { }

  public ngOnInit(): void {
    this.apps = this.appService.getAllApps();
    this.appTitleService.chooseHome();
  }
}
