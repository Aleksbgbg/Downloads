import { Injectable, Inject } from "@angular/core";
import { Title, Meta } from "@angular/platform-browser";

@Injectable({
  providedIn: "root"
})
export class AppTitleService {
  constructor(private titleService: Title, private metaService: Meta, @Inject("protocol") protocol: string, @Inject("host") host: string) {
    metaService.addTag({
      name: "og:image",
      content: `${protocol}//${host}/favicon.png`
    });
    metaService.addTag({
      name: "og:site_name",
      content: "Aleksbgbg Downloads"
    });
    metaService.addTag({
      name: "og:title",
      content: "Aleksbgbg Downloads"
    });
    metaService.addTag({
      name: "og:description",
      content: "Downloads page for all of Aleksbgbg's released software on GitHub."
    });
  }

  public chooseHome(): void {
    this.setTitle("All Apps");
  }

  public chooseApp(appName: string): void {
    this.setTitle(appName);
  }

  private setTitle(modifier: string): void {
    const title = `Aleksbgbg Downloads | ${modifier}`;

    this.titleService.setTitle(title);
    this.metaService.updateTag({ name: "og:title", content: title });
  }
}
