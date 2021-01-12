import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-picture",
  templateUrl: "./picture.component.html",
  styleUrls: ["./picture.component.scss"],
})
export class PictureComponent implements OnInit {
  constructor(private route: ActivatedRoute) {
    this.route.queryParams.subscribe((params) => {
      this.start = params.start;
      this.end = params.end;
    });
  }

  ngOnInit() {
    console.log(this.start);
    console.log(this.end);
  }

  start: string;
  end: string;
  imageObject: Array<object> = [
    {
      image: "../../../../../DKS-API/Resources/Pictures/ArticlePics/2.jpg",
      thumbImage: "../../../../../DKS-API/Resources/Pictures/ArticlePics/2.jpg",
    },
    {
      image: "../../../assets/favicon.ico", // Support base64 image
      thumbImage: "../../../assets/favicon.ico", // Support base64 image
    },
  ];
}
