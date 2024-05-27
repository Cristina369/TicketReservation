import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAllReservationsComponent } from './admin-all-reservations.component';

describe('AdminAllReservationsComponent', () => {
  let component: AdminAllReservationsComponent;
  let fixture: ComponentFixture<AdminAllReservationsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminAllReservationsComponent]
    });
    fixture = TestBed.createComponent(AdminAllReservationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
