import { HttpClient } from "@angular/common/http";
import { Component, Inject } from "@angular/core";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  featureToggles: FeatureTogglesResponse;
  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    http
      .get<FeatureTogglesResponse>(baseUrl + "weatherforecast/featuretoggles")
      .subscribe(
        (result) => {
          this.featureToggles = result;
        },
        (error) => console.error(error)
      );
  }
}

interface FeatureTogglesResponse {
  funkySummary: boolean;
  anotherFeature: boolean;
}
