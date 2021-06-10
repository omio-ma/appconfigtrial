import { HttpClient } from "@angular/common/http";
import { Component, Inject } from "@angular/core";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  dataBasedOnFeatureOne: string;

  featureOne: boolean;
  featureTwo: boolean;

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    http
      .get<boolean>(baseUrl + "feature/getFeature/FeatureOne")
      .subscribe(
        (result) => {
          this.featureOne = result;
        },
        (error) => console.error(error)
    );
    
    http
      .get<boolean>(baseUrl + "feature/getFeature/FeatureTwo")
      .subscribe(
        (result) => {
          this.featureTwo = result;
        },
        (error) => console.error(error)
    );
    
    http
      .get<{ data: string }>(baseUrl + "feature/getFeatureOneDependantData")
      .subscribe(
        (result) => {
          this.dataBasedOnFeatureOne = result.data;
        },
        (error) => console.error(error)
      );
  }
}

interface Features {
  featureOne: boolean;
  featureTwo: boolean;
}
