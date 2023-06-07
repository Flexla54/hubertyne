/**
 * Used to extend a Plug with statistics
 * @param consumption - datapoints (including temp, usage, date) state what the plug consumed
 * @param usage - amount of power what a plug had used (given in watt/hours)
 * @param updatedOn - datetime on which (parts of) the statistics were updated
 */
export interface PlugStatistic {
  updatedOn: string;

  consumption?: DataPoint[];
  consumptionQuery?: ConsumptionQuery;

  usage?: number;
  usageQuery?: UsageQuery;
}

/**
 * Used to represent an (aggregated) datapoint
 * sent by the monitoring devices
 * @param powerUsage - given in W
 * @param temperature - given in °C
 */
export interface DataPoint {
  date: string;
  powerUsage: number;
  temperature: number;
}

/**
 * Used for receiving data from server to reference to plug
 */
export interface Usage {
  resourceId: string;
  data: number;
}

/**
 * Used for receiving data from API to reference to plug
 */
export interface Consumption {
  resourceId: string;
  data: DataPoint[];
}

/**
 * Used for querying the data points for the line charts.
 * It is possible to query all power monitoring devices or
 * only one (given in resourceId).
 *
 * @param resourceId - if this is set only one device is being queried,
 * if null -> all devices are queried
 * @param tact - After what unit the datapoints should be queried
 */
export interface ConsumptionQuery {
  resourceId?: string;
  from: string;
  to: string;
  tact: 'minutes' | 'hours' | 'days' | 'weeks' | 'years';
}

/**
 * Used for querying the amount of energy (in e.g. W/h)
 * consumed in the specified period.
 */
export interface UsageQuery {
  resourceId?: string;
  from: string;
  to: string;
}
