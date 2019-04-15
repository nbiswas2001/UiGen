import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'sample-ui';

  name = ''
  statuses: string[] = ["New", "In-Progress", "Complete"]
  status = "Not Set"

  onOK(){
    alert(this.name + " "+ this.status)
  }
}
