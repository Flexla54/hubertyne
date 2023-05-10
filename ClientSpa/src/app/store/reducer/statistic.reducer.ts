import { createReducer, on } from '@ngrx/store';
import * as StatisticActions from '../actions/statistic.actions';
import { StatisticState } from '../state/statistic.state';

export const InitialStatisticsState: StatisticState = {
  updatedOn: new Date(),
  queried: {
    resourceIds: [],
    from: new Date(),
    to: new Date(),
    tact: 'minutes',
  },
  plugs: [],
};

export const productsReducer = createReducer(
  InitialStatisticsState,
  on(StatisticActions.setSingleStatistic, (state, { data, query }) => ({
    ...state,
    UpdatedOn: new Date(),
    queried: query,
    plugs: [data],
  })),
  on(StatisticActions.setMultipleStatistics, (state, { data, query }) => ({
    ...state,
    updatedOn: new Date(),
    queried: query,
    plugs: data,
  }))
);
