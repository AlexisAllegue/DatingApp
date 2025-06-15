import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {

  private accountService = inject(AccountService);
  loggedin = false;
  model: any = {};

  login() {
    this.accountService.login(this.model).subscribe({
      next: response =>
      {
        console.log(response);
        this.loggedin = true;
      },
      error: error => console.log(error)
    })
  }
}
