import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Customer } from '../models/customer';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CustomersService {
  constructor(private http: HttpClient) {}

  getAllCustomers(): Observable<Customer[]> {
    return this.http.get<Customer[]>(`${environment.apiBaseUrl}/api/Customer`);
  }

  deleteCustomer(id: string): Observable<Customer> {
    return this.http.delete<Customer>(
      `${environment.apiBaseUrl}/api/Customer/${id}`
    );
  }
}
