import { EventM } from '../../events/models/event';
import { Seat } from './seat';

export interface ReservationRequest {
  paymentMethod: string;
  transactionNumber: string;
  totalPrice: string;
  customerId: string;
  clientLastName: string;
  customerName: string;
  customerEmail: string;
  event: EventM;
  seats: Seat[];
}
