import { GraphQuery, PlugsDataPoints } from 'src/app/models/statistics';

export interface StatisticState {
  updatedOn: Date;
  queried: GraphQuery;
  plugs: PlugsDataPoints[];
}
