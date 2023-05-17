import { createSelector } from '@ngrx/store';
import { AppState } from '../state/app.state';

export const selectPlugs = (state: AppState) => state.plugs;

export const selectSpecificPlug = (id: string) =>
  createSelector(selectPlugs, (plugs) => {
    plugs = plugs.filter((plug) => {
      return plug.id == id;
    });
    return plugs.slice();
  });
