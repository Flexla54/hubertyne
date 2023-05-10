import { createAction, props } from '@ngrx/store';
import { GraphQuery, PlugsDataPoints } from 'src/app/models/statistics';

// --------------------------------------------------
// Actions for plug statistics
export const setSingle = createAction(
  '[PowerData] Set Statistics for single plug',
  props<{ data: PlugsDataPoints; query: GraphQuery }>()
);

export const setMultipleStatistics = createAction(
  '[PowerData] Set Statistics for multiple plugs',
  props<{ data: PlugsDataPoints[]; query: GraphQuery }>()
);
