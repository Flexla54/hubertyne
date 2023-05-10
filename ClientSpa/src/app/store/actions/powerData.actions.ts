import { createAction, props } from '@ngrx/store';

// --------------------------------------------------
// Actions for power data
export const setPowerDataSuggestions = createAction(
  '[PowerData] Set Power Data',
  props<{ data: [] }>()
);

export const clearPowerDataSuggestions = createAction(
  '[PowerData] Clear Power Data'
);
