import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EventImage } from '../models/event-image';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  selectedImage: BehaviorSubject<EventImage> = new BehaviorSubject<EventImage>({
    imageUrl: '',
    title: '',
  });

  constructor(private http: HttpClient) {}

  getAllImages(): Observable<EventImage[]> {
    return this.http.get<EventImage[]>(
      `${environment.apiBaseUrl}/api/Images/GetAllImages`
    );
  }

  uploadImage(file: File, title: string): Observable<EventImage> {
    if (!file) {
      return throwError('No file selected');
    }

    const formData = new FormData();
    formData.append('file', file);
    formData.append('title', title);

    const headers = new HttpHeaders();

    console.log('File is here: ' + JSON.stringify(formData));
    return this.http
      .post<EventImage>(
        `${environment.apiBaseUrl}/api/Images/UploadAsync`,
        formData,
        { headers }
      )
      .pipe(
        catchError((error) => {
          console.error('Error uploading image:', error);
          return throwError('Failed to upload image');
        })
      );
  }

  selectImage(image: EventImage): void {
    this.selectedImage.next(image);
  }

  onSelectImage(): Observable<EventImage> {
    return this.selectedImage.asObservable();
  }
}
