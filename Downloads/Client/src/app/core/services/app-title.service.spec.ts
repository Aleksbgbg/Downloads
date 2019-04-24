import { TestBed } from "@angular/core/testing";

import { AppTitleService } from "./app-title.service";

describe("AppTitleService", () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it("should be created", () => {
    const service: AppTitleService = TestBed.get(AppTitleService);
    expect(service).toBeTruthy();
  });
});
