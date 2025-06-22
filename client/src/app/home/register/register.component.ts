import { Component, EventEmitter, inject, Input, input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountSerivice = inject(AccountService);
  private toastr = inject(ToastrService);

  // @Input() usersFromParent: any;
  usersFromParent = input.required<any>();

  // @Output() cancelFromChild = new EventEmitter();
  cancelFromChild = output<boolean>();
  model: any = {}

  register() {
    this.accountSerivice.register(this.model).subscribe(
      {
        next: response => {
          console.log(response);
          this.cancel();
        },
        error: error => this.toastr.error(error.error)
      }
    )
  }

  cancel() {
    this.cancelFromChild.emit(false);
  }
}
