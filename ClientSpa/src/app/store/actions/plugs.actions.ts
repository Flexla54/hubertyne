import { createAction, props } from '@ngrx/store';
import { Plug } from 'src/app/models/plug';

// --------------------------------------------------
// Actions for plugs
export const addPlug = createAction('[Plug] Add plug', props<{ plug: Plug }>());

export const addMutlitplePlugs = createAction(
  '[Plug] Add multiple plugs',
  props<{ plugs: Plug[] }>()
);

export const turnOnPlug = createAction(
  '[Plug] Turn on plug',
  props<{ id: string }>()
);

export const turnOffPlug = createAction(
  '[Plug] Turn off plug',
  props<{ id: string }>()
);

export const renamePlug = createAction(
  '[Plug] Rename plug',
  props<{ id: string; name: string }>()
);

export const removePlug = createAction(
  '[Plug] Remove plug',
  props<{ id: string }>()
);
