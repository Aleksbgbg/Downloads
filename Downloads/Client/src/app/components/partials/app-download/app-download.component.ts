import { Component, Input } from "@angular/core";

@Component({
  selector: "app-download",
  templateUrl: "./app-download.component.html",
  styleUrls: ["./app-download.component.sass"]
})
export class AppDownloadComponent {
  @Input("app") public app: App;
}
