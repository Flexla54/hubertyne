import { createAction, props } from '@ngrx/store';
import { Plug } from 'src/app/models/plug';
import { ConsumptionQuery, UsageQuery } from 'src/app/models/statistics';

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

// ############################################
// Statistics

export const requestSpecificPlugUsage = createAction(
  '[Plug-Statistics] Request usage of specific plug',
  props<{ query: UsageQuery }>()
);

export const requestAllPlugsUsage = createAction(
  '[Plug-Statistics] Request usage of all plugs',
  props<{ query: UsageQuery }>()
);

export const requestSpecificConsumption = createAction(
  '[Plug-Statistics] Request consumption of specific plug',
  props<{ query: ConsumptionQuery }>()
);

export const requestAllConsumption = createAction(
  '[Plug-Statistics] Request consumption of all plugs',
  props<{ query: ConsumptionQuery }>()
);
