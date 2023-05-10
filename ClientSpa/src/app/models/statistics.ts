export interface PlugsDataPoints {
  plugId: string;
  consumptionSum: number;
  data: DataPoint[];
}

export interface DataPoint {
  date: Date;
  powerUsage: number;
  temperature: number;
}

export interface GraphQuery {
  resourceIds: string[];
  from: Date;
  to: Date;
  tact: 'minutes' | 'hours' | 'days' | 'weeks' | 'years';
}

export interface ConsumptionQuery {
  resourceIds: string[];
  from: Date;
  to: Date;
}
