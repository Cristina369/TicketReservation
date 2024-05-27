import { Component, OnInit, Renderer2 } from '@angular/core';
import { EventModel } from '../models/event-model';
import { ActivatedRoute, Router } from '@angular/router';
import { EventsService } from '../service/events.service';
import { catchError, forkJoin, Observable, of, Subscription, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { SeatRequest } from '../models/seat-request';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css'],
})
export class EventDetailsComponent implements OnInit {
  id: string | null = null;
  svgPath: string | undefined;
  eventM?: EventModel;

  svgData?: string;
  selectedSeats: Set<string> = new Set();

  routeSubscription?: Subscription;
  getEventSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private eventsService: EventsService,
    private renderer: Renderer2,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe((params) => {
      this.id = params.get('id');

      if (this.id) {
        this.getEventSubscription = this.eventsService
          .getEventById(this.id)
          .subscribe((response) => {
            this.eventM = response;

            import('../../../shared/components/json/SvgListFile.json').then(
              (jsonData) => {
                console.log('JSON Data:', jsonData.default);

                if (
                  Array.isArray(jsonData.default) &&
                  this.eventM &&
                  this.eventM.seatMapPath
                ) {
                  const matchingSvg = jsonData.default.find(
                    (item: any) => item.name === this.eventM?.seatMapPath
                  );
                  if (matchingSvg) {
                    this.svgPath = matchingSvg.seatMapPath;
                    console.log('SVG Path:', this.svgPath);

                    this.http
                      .get(matchingSvg.seatMapPath, {
                        responseType: 'text',
                      })
                      .subscribe((svgData) => {
                        this.svgData = svgData;
                        this.createSeatMap();
                      });
                  } else {
                    console.error(
                      'Matching SVG not found' + this.eventM.seatMapPath
                    );
                  }
                } else {
                  console.error(
                    'jsonData is not an array or eventM or seatMapPath is not defined:',
                    jsonData.default,
                    this.eventM
                  );
                }
              }
            );
          });
      }
    });
  }

  extractFileName(filePath: string): string {
    return filePath.split('/').pop()!;
  }

  createSeatMap(): void {
    if (this.eventM && this.eventM.id) {
      this.eventsService
        .getSeatsByEventId(this.eventM.id)
        .subscribe((seats: any[]) => {
          if (seats && seats.length > 0) {
            this.renderSvgWithSeats(seats);
            this.highlightSeats();
          } else {
            console.error('No seats available for the event.');
          }
        });
    } else {
      console.error('Event ID is missing.');
    }
  }

  renderSvgWithSeats(seats: any[]): void {
    const container = document.getElementById('event-container');
    if (container && this.svgData) {
      container.innerHTML = '';
      const parser = new DOMParser();
      const svgElement = parser.parseFromString(
        this.svgData,
        'image/svg+xml'
      ).documentElement;
      container.appendChild(svgElement);

      const svgSeats = container.querySelectorAll('.seat');
      svgSeats.forEach((svgSeat) => {
        const [zone, row, number] = svgSeat.id.split('.');
        const seat = seats.find(
          (seat) =>
            seat.row === row && seat.zone === zone && seat.number === number
        );
        if (seat) {
          if (!seat.isAvailable) {
            svgSeat.setAttribute('fill', 'red');
            svgSeat.setAttribute('pointer-events', 'none');
          } else if (this.selectedSeats.has(seat.id)) {
            svgSeat.setAttribute('fill', 'red');
          } else {
            svgSeat.setAttribute('fill', 'green');
          }
        } else {
          svgSeat.setAttribute('fill', 'green');
        }

        this.renderer.listen(svgSeat, 'click', () =>
          this.toggleSeatSelection(svgSeat.id)
        );
      });
    }
  }

  toggleSeatSelection(seatId: string): void {
    const seat = document.getElementById(seatId);
    if (!seat) {
      console.error('Seat not found:', seatId);
      return;
    }

    if (
      !seat.getAttribute('pointer-events') ||
      seat.getAttribute('pointer-events') !== 'none'
    ) {
      if (this.selectedSeats.has(seatId)) {
        this.selectedSeats.delete(seatId);
        seat.setAttribute('fill', 'green');
      } else {
        if (this.selectedSeats.size < 5) {
          this.selectedSeats.add(seatId);
          seat.setAttribute('fill', 'red');
        } else {
          alert('You can select a maximum of 5 seats.');
        }
      }
    } else {
      seat.setAttribute('fill', 'red');
    }
    this.highlightSeats();
  }

  highlightSeats(): void {
    const container = document.getElementById('event-container');
    if (container) {
      const seats = container.querySelectorAll('.seat');
      seats.forEach((seat) => {
        const seatId = seat.id;
        const isSeatAvailable =
          !seat.getAttribute('pointer-events') ||
          seat.getAttribute('pointer-events') !== 'none';

        if (this.selectedSeats.has(seatId)) {
          seat.setAttribute('fill', 'red');
        } else if (!isSeatAvailable) {
          seat.setAttribute('fill', 'red');
        } else if (seat.classList.contains('seat-yellow')) {
          seat.setAttribute('fill', 'yellow');
        } else if (seat.classList.contains('seat-orange')) {
          seat.setAttribute('fill', 'orange');
        } else if (seat.classList.contains('seat-dark_orange')) {
          seat.setAttribute('fill', '#ff6600');
        } else {
          seat.setAttribute('fill', 'green');
        }
      });
    }
  }

  continueToBuy(): void {
    if (this.eventM && this.selectedSeats.size > 0) {
      if (this.eventM.id) {
        const observables: Observable<any>[] = [];
        const seatIds: string[] = [];

        Array.from(this.selectedSeats).forEach((seatId: string) => {
          const [zone, row, number] = seatId.split('.');
          const seatData: SeatRequest = {
            row,
            zone,
            number,
            eventId: this.eventM!.id,
          };

          const observable = this.eventsService
            .getSeatByPosition(seatData)
            .pipe(
              tap((response: any) => {
                console.log('Seat data sent to the backend:', response);
                const seatId = response;
                seatIds.push(seatId);
              }),
              catchError((error) => {
                console.error('Error sending seat data to the backend:', error);
                return of(null);
              })
            );
          observables.push(observable);
        });

        forkJoin(observables).subscribe(() => {
          this.router.navigate(['/buy-ticket'], {
            state: { seatIds, eventId: this.eventM!.id },
          });
        });
      } else {
        console.error('Event ID is undefined.');
      }
    } else {
      console.error('Event or selected seats not available.');
    }
  }
}
