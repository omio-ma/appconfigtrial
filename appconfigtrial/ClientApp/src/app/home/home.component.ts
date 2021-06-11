import { HttpClient } from "@angular/common/http";
import { Component, Inject } from "@angular/core";

import * as signalR from "@microsoft/signalr";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  dataBasedOnFeatureOne: string;

  featureOne: boolean;
  featureTwo: boolean;

  hubMessage: string;
  private hubConnection: signalR.HubConnection | undefined;

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    http.get<boolean>(baseUrl + "feature/getFeature/FeatureOne").subscribe(
      (result) => {
        this.featureOne = result;
      },
      (error) => console.error(error)
    );

    http.get<boolean>(baseUrl + "feature/getFeature/FeatureTwo").subscribe(
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

  async ngOnInit() {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    let connection = new signalR.HubConnectionBuilder()
      .withUrl("/featureHub")
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.on("FeatureHubHealth", (data) => {
      this.hubMessage = data;
    });

    connection.on("CheckFeature", (data) => {
      this.featureOne = data;
    });

    await connection.start();
    connection.invoke("HealthCheck");
    connection.invoke("CheckFeature", "featureOne");
  }
}

interface Features {
  featureOne: boolean;
  featureTwo: boolean;
}
