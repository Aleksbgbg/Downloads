import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { SpecificAppComponent } from "./specific-app.component";

describe("SpecificAppComponent", () => {
  let component: SpecificAppComponent;
  let fixture: ComponentFixture<SpecificAppComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpecificAppComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecificAppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
