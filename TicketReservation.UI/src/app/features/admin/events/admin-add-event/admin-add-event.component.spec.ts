import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAddEventComponent } from './admin-add-event.component';

describe('AdminAddEventComponent', () => {
  let component: AdminAddEventComponent;
  let fixture: ComponentFixture<AdminAddEventComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminAddEventComponent]
    });
    fixture = TestBed.createComponent(AdminAddEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
