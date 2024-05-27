import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Location } from '../models/location';
import { LocationsService } from '../service/locations.service';
import { Router } from '@angular/router';
import { PopulateLocationRequest } from '../models/populate-location';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin-all-locations',
  templateUrl: './admin-all-locations.component.html',
  styleUrls: ['./admin-all-locations.component.css'],
})
export class AdminAllLocationsComponent implements OnInit, OnDestroy {
  svgData?: string;
  id: string | null = null;
  locations$?: Observable<Location[]>;
  selectedLocation?: string | null;
  selectedSeats: Set<string> = new Set();

  deleteLocationSubscription?: Subscription;
  populateLocationSubscription?: Subscription;

  constructor(
    private adminLocationService: LocationsService,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.locations$ = this.adminLocationService.getAllLocations();

    this.locations$.subscribe((locations) => {
      locations.forEach((location) => {
        if (location.locationId) {
          console.log(`Location Id here: ${location.locationId}`);
        } else {
          console.log('Location is undefined.');
        }
      });
    });
  }

  onDelete(id: string): void {
    if (id) {
      this.deleteLocationSubscription = this.adminLocationService
        .deleteLocation(id)
        .subscribe({
          next: (response) => {
            this.router
              .navigateByUrl('/', { skipLocationChange: true })
              .then(() => {
                this.router.navigate(['/admin/locations']);
              });
          },
        });
    }
  }

  onPopulate(id: string, path: string): void {
    if (id && path) {
      const populateData: PopulateLocationRequest = {
        selectedLocation: id,
        svgFilePath: path,
      };

      this.populateLocationSubscription = this.adminLocationService
        .populateLocation(populateData)
        .subscribe({
          next: (response) => {
            this.router
              .navigateByUrl('/', { skipLocationChange: true })
              .then(() => {
                this.router.navigate(['/admin/locations']);
              });
          },
        });
    }
  }

  openImageModal(location: Location): void {
    this.http
      .get('assets/' + location.locationPath + '.svg', {
        responseType: 'text',
      })
      .subscribe((svgData) => {
        this.svgData = svgData;
        this.createSeatMap();
      });

    this.selectedLocation = location.name;
  }

  closeModal(): void {
    this.selectedLocation = null;
  }

  createSeatMap(): void {
    const container = document.getElementById('container');
    if (container && this.svgData) {
      container.innerHTML = this.svgData;
      const seats = container.querySelectorAll('.seat');
      seats.forEach((seat) => {
        seat.addEventListener('click', () => this.toggleSeatSelection(seat.id));
      });
    }
  }

  toggleSeatSelection(seatId: string): void {
    if (this.selectedSeats.has(seatId)) {
      this.selectedSeats.delete(seatId);
    } else {
      this.selectedSeats.add(seatId);
    }
    this.highlightSeats();
  }

  highlightSeats(): void {
    const container = document.getElementById('container');
    if (container) {
      const seats = container.querySelectorAll('.seat');
      seats.forEach((seat) => {
        if (this.selectedSeats.has(seat.id)) {
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

  ngOnDestroy(): void {
    this.deleteLocationSubscription?.unsubscribe();
    this.populateLocationSubscription?.unsubscribe();
  }
}
