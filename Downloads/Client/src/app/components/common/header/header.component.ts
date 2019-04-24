import { Component, OnInit, Input } from "@angular/core";
import { AppService } from "src/app/core/services/app.service";
import { Observable } from "rxjs";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.sass"]
})
export class HeaderComponent implements OnInit {
  public apps: Observable<App[]>;

  constructor(private appService: AppService) { }

  public ngOnInit(): void {
    this.apps = this.appService.getAllApps();
  }
}
