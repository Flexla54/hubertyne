import { Plug } from 'src/app/models/plug';
import { StatisticState } from './statistic.state';

export interface AppState {
  statistic: StatisticState;
  plugs: Plug[];
}
