import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminEditEventComponent } from './admin-edit-event.component';

describe('AdminEditEventComponent', () => {
  let component: AdminEditEventComponent;
  let fixture: ComponentFixture<AdminEditEventComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminEditEventComponent]
    });
    fixture = TestBed.createComponent(AdminEditEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
