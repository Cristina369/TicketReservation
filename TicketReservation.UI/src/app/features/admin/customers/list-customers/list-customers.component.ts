import { Component } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Customer } from '../models/customer';
import { CustomersService } from '../services/customers.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-customers',
  templateUrl: './list-customers.component.html',
  styleUrls: ['./list-customers.component.css'],
})
export class ListCustomersComponent {
  id: string | null = null;
  customers$?: Observable<Customer[]>;

  deleteCustomerSubscription?: Subscription;

  constructor(
    private adminCustomersService: CustomersService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.customers$ = this.adminCustomersService.getAllCustomers();

    this.customers$.subscribe((customersM) => {
      customersM.forEach((customerM) => {
        if (customerM.customerId) {
          console.log(`Customer Id here: ${customerM.customerId}`);
        } else {
          console.log('Customer is undefined.');
        }
      });
    });
  }

  onDelete(id: string): void {
    if (id) {
      this.deleteCustomerSubscription = this.adminCustomersService
        .deleteCustomer(id)
        .subscribe({
          next: (response) => {
            this.router
              .navigateByUrl('/', { skipLocationChange: true })
              .then(() => {
                this.router.navigate(['admin/costumers']);
              });
          },
        });
    }
  }

  ngOnDestroy(): void {
    this.deleteCustomerSubscription?.unsubscribe();
  }
}
