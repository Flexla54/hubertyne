import { createReducer, on } from '@ngrx/store';
import * as PlugsActions from '../actions/plugs.actions';
import { Plug } from 'src/app/models/plug';

export const InitialPlugsstate: Plug[] = [];

export const productsReducer = createReducer(
  InitialPlugsstate,
  on(PlugsActions.addPlug, (state, { plug }) => [...state, plug]),
  on(PlugsActions.addMutlitplePlugs, (state, { plugs }) => [
    ...state,
    ...plugs,
  ]),
  on(PlugsActions.renamePlug, (state, { id, name }) =>
    state.map((plug) => (id == plug.id ? { ...plug, name: name } : plug))
  ),
  on(PlugsActions.turnOffPlug, (state, { id }) =>
    state.map((plug) => (id == plug.id ? { ...plug, isTurnedOn: false } : plug))
  ),
  on(PlugsActions.turnOnPlug, (state, { id }) =>
    state.map((plug) => (id == plug.id ? { ...plug, isTurnedOn: true } : plug))
  ),
  on(PlugsActions.removePlug, (state, { id }) =>
    state.filter((plug) => plug.id !== id)
  )
);
