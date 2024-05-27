import { Component } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Reservation } from '../models/reservation';
import { ReservationsService } from '../services/reservations.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-all-reservations',
  templateUrl: './admin-all-reservations.component.html',
  styleUrls: ['./admin-all-reservations.component.css'],
})
export class AdminAllReservationsComponent {
  id: string | null = null;
  reservations$?: Observable<Reservation[]>;

  deleteReservationSubscription?: Subscription;

  constructor(
    private adminReservationService: ReservationsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.reservations$ = this.adminReservationService.getAllReservations();

    this.reservations$.subscribe((reservations) => {
      reservations.forEach((reservation) => {
        if (reservation.reservationId) {
          console.log(`Reservation Id here: ${reservation.reservationId}`);
        } else {
          console.log('Reservation is undefined.');
        }
      });
    });
  }

  onDelete(id: string): void {
    if (id) {
      this.deleteReservationSubscription = this.adminReservationService
        .deleteReservation(id)
        .subscribe({
          next: (response) => {
            this.router
              .navigateByUrl('/', { skipLocationChange: true })
              .then(() => {
                this.router.navigate(['/admin/reservations']);
              });
          },
        });
    }
  }

  ngOnDestroy(): void {
    this.deleteReservationSubscription?.unsubscribe();
  }
}
