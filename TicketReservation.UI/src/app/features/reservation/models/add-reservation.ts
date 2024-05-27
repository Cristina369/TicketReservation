export interface addReservationRequest {
  paymentMethod: string;
  clientFirstName: string;
  clientLastName: string;
  clientEmail: string;
  clientPhone: string;
  clientAddress: string;
  event: string;
  seats: string[];
}
