export interface Plug {
  id: string;
  name: string;
  addedDate: Date;
  isConnected: boolean;
  isTurnedOn: boolean;
  userId: string;
  statistics: any;
}
