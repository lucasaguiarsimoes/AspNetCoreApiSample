import { TestBed } from '@angular/core/testing';

import { ThemeResolver } from './theme.resolver';

describe('ThemeResolver', () => {
  let resolver: ThemeResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(ThemeResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
