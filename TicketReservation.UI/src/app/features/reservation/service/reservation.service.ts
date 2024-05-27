import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Reservation } from '../models/reservation';

@Injectable({
  providedIn: 'root',
})
export class ReservationService {
  constructor(private http: HttpClient, private cookieService: CookieService) {}

  reserveSeat(seatInfo: any): Observable<any> {
    return this.http.post<any>(
      `${environment.apiBaseUrl}/api/Reservation`,
      seatInfo
    );
  }

  getAllReservations(): Observable<Reservation> {
    return this.http.get<Reservation>(
      `${environment.apiBaseUrl}/api/Reservation/List?addAuth=true`
    );
  }
}
