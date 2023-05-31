import { PlugsDataPoints } from './statistics';

export interface Plug {
  id: string;
  name: string;
  addedDate: Date;
  isConnected: boolean;
  isTurnedOn: boolean;
  userId: string;
  statistics: number[];
}
