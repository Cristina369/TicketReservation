import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAllLocationsComponent } from './admin-all-locations.component';

describe('AdminAllLocationsComponent', () => {
  let component: AdminAllLocationsComponent;
  let fixture: ComponentFixture<AdminAllLocationsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminAllLocationsComponent]
    });
    fixture = TestBed.createComponent(AdminAllLocationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
