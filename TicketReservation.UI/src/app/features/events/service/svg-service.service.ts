import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SvgPath } from '../models/svg-path';

@Injectable({
  providedIn: 'root',
})
export class SvgServiceService {
  constructor(private http: HttpClient) {}

  getSvgUrl(svgRelativePath: string): string {
    const baseUrl = 'http://localhost:7156/';

    return `${baseUrl}${svgRelativePath}`;
  }

  getSvgAsBlob(svgRelativePath: string): Observable<Blob> {
    const svgUrl = this.getSvgUrl(svgRelativePath);
    return this.http.get(svgUrl, { responseType: 'blob' });
  }

  getSvgPaths(): Observable<SvgPath[]> {
    return this.http.get<SvgPath[]>(
      '../../../shared/components/SvgListFile.json'
    );
  }
}
