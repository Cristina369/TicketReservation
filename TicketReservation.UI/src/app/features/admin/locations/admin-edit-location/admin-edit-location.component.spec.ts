import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminEditLocationComponent } from './admin-edit-location.component';

describe('AdminEditLocationComponent', () => {
  let component: AdminEditLocationComponent;
  let fixture: ComponentFixture<AdminEditLocationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminEditLocationComponent]
    });
    fixture = TestBed.createComponent(AdminEditLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
