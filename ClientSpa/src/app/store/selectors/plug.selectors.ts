import { createSelector } from '@ngrx/store';
import { AppState } from '../state/app.state';

export const selectPlugs = (state: AppState) => state.plugs;

export const selectPlugById = (id: string) =>
  createSelector(selectPlugs, (plugs) => {
    return plugs.find((plug) => {
      return plug.id == id;
    });
  });

// ############################################
// Statistics

export const selectStatisticsByPlugId = (id: string) =>
  createSelector(selectPlugs, (plugs) => {
    const plug = plugs.find((plug) => {
      return plug.id == id;
    });

    return plug?.powerStatistics;
  });

export const selectConsumptionByPlugId = (id: string) =>
  createSelector(selectStatisticsByPlugId(id), (stats) => {
    return stats?.consumption;
  });

export const selectConsumptionByPlugId = (id: string) =>
  createSelector(selectStatisticsByPlugId(id), (stats) => {
    return stats?.consumption;
  });
