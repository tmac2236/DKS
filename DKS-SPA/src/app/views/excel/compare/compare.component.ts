import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
@Component({
  selector: "app-compare",
  templateUrl: "./compare.component.html",
  styleUrls: ["./compare.component.scss"],
})
export class CompareComponent implements OnInit {
  showModal: boolean;
  registerForm: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.formInit();
  }

  formInit() {
    this.registerForm = this.formBuilder.group({
      email: ["dasd"],
      password: ["asdasd"],
      firstname: ["asdasd"],
      mobile: ["asd"],
    });
  }
  show() {
    this.showModal = true; // Show-Hide Modal Check
  }
  //Bootstrap Modal Close event
  hide() {
    this.showModal = false;
  }
  // convenience getter for easy access to form fields
  get f() {
    return this.registerForm.controls;
  }
  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }
    if (this.submitted) {
      this.showModal = false;
    }
  }
}
