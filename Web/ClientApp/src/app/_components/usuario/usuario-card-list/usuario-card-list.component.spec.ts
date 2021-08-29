import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuarioCardListComponent } from './usuario-card-list.component';

describe('UsuarioCardListComponent', () => {
  let component: UsuarioCardListComponent;
  let fixture: ComponentFixture<UsuarioCardListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsuarioCardListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UsuarioCardListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
