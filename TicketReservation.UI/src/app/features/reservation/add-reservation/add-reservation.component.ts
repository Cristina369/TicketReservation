import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { addReservationRequest } from '../models/add-reservation';
import { ReservationService } from '../service/reservation.service';

@Component({
  selector: 'app-add-reservation',
  templateUrl: './add-reservation.component.html',
  styleUrls: ['./add-reservation.component.css'],
})
export class AddReservationComponent implements OnInit {
  addReservationRequest: addReservationRequest = {
    paymentMethod: '',
    clientFirstName: '',
    clientLastName: '',
    clientEmail: '',
    clientPhone: '',
    clientAddress: '',
    event: '',
    seats: [],
  };

  eventId: string | undefined;
  selectedSeats: string[] | undefined;

  constructor(
    private reservationService: ReservationService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const state = window.history.state;

    if (state && state.seatIds && state.eventId) {
      this.eventId = state.eventId;
      this.selectedSeats = Array.isArray(state.seatIds) ? state.seatIds : [];
      console.log('Received eventId:', this.eventId);
      console.log('Received selectedSeats:', this.selectedSeats);
    } else {
      console.error('Event ID or selected seats are missing');
    }
  }

  onSubmit(): void {
    console.log(
      'The Reservation is here: ' + JSON.stringify(this.addReservationRequest)
    );
    if (this.eventId && this.selectedSeats) {
      this.addReservationRequest.event = this.eventId;
      this.addReservationRequest.seats = this.selectedSeats;

      this.reservationService.reserveSeat(this.addReservationRequest).subscribe(
        (response) => {
          console.log('Reservation added successfully:', response);
          this.router.navigateByUrl('/success');
        },
        (error) => {
          console.error('Error adding reservation:', error);
        }
      );
    } else {
      console.error('Event ID or selected seats are missing');
    }
  }
}
