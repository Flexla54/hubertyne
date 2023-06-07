import { PlugStatistic } from './statistics';

// TODO: review if the two statistics are required
export interface Plug {
  id: string;
  name: string;
  addedDate: Date;
  isConnected: boolean;
  isTurnedOn: boolean;
  userId: string;
  statistics: number[];
  powerStatistics: PlugStatistic;
}
