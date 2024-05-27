import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EventModel } from '../models/event-model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EventM } from '../models/event';
import { AddEventRequest } from '../models/add-event';

@Injectable({
  providedIn: 'root',
})
export class AdminEventsService {
  constructor(private http: HttpClient) {}

  getEventById(id: string): Observable<EventModel> {
    return this.http.get<EventModel>(
      `${environment.apiBaseUrl}/api/Events/${id}`
    );
  }

  getAllEvents(): Observable<EventModel[]> {
    return this.http.get<EventModel[]>(`${environment.apiBaseUrl}/api/Events`);
  }

  deleteEvent(id: string): Observable<EventModel> {
    return this.http.delete<EventModel>(
      `${environment.apiBaseUrl}/api/Events/${id}`
    );
  }

  createEvent(model: AddEventRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/api/Events`, model);
  }

  updateEvent(
    id: string,
    updateEvent: AddEventRequest
  ): Observable<EventModel> {
    return this.http.put<EventModel>(
      `${environment.apiBaseUrl}/api/Events/${id}?addAuth=true`,
      updateEvent
    );
  }
}
