import { GraphQuery, PlugsDataPoints } from 'src/app/models/statistics';

export interface StatisticsState {
  updatedOn: Date;
  queried: GraphQuery;
  plugs: PlugsDataPoints[];
}

export const InitialStatisticsState: StatisticsState = {
  updatedOn: new Date(),
  queried: {
    resourceIds: [],
    from: new Date(),
    to: new Date(),
    tact: 'minutes',
  },
  plugs: [
    {
      plugId: '',
      consumptionSum: 0,
      data: [],
    },
  ],
};
