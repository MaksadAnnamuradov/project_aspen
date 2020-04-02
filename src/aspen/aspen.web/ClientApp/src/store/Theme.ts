import { Action, Reducer } from "redux";
import { Theme } from "../models/Theme";
import { APIService } from "../services/APIService";
import { DomainService } from "../services/DomainService"
import { Typography } from "@material-ui/core";
import { AppThunkAction } from "./index";

const apiService = new APIService(new DomainService())

//State
export interface ThemeState {
  typography: Typography;
  palette: Palette;
}

export interface Typography {
  fontFamily: string;
}

export interface Palette {
  primary: Primary;
  secondary: Secondary;
}

export interface Primary {
  main: string;
  light: string;
  contrastText: string;
}

export interface Secondary {
  main: string;
}

//Actions
interface ReceiveThemeAction {
  type: "LOADING_THEME";
  // typography: Typography;
  // palette: Palette;
  theme: Theme;
}

function receiveTheme(theme: Theme) : ReceiveThemeAction {
  return {
    type: "LOADING_THEME",
    theme: theme
  }
}

export const loadThemeAction = (): AppThunkAction<KnownAction> => {
  return function(dispatch) {
    return apiService
    .GetCharityHomePage()
    .then(result => dispatch(receiveTheme(result.Theme)))
    .catch(e => e);
  };
};

export const actionCreators = {
  loadThemeAction
};

const typ: Typography = { fontFamily: "Arial" };
const prim: Primary = {
  main: "#fcb8ab",
  light: "#ffddd6",
  contrastText: "#000000"
};
const sec: Secondary = { main: "#ffe0a6" };
const pal: Palette = { primary: prim, secondary: sec };

const initialState: ThemeState = {
  typography: typ,
  palette: pal
};

//Known Actions
export type KnownAction = ReceiveThemeAction;
//Reducer
export const reducer: Reducer<ThemeState> = (
  state: ThemeState | undefined,
  incomingAction: Action
): ThemeState => {
  if (state == undefined) {
    return initialState;
  }

  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "LOADING_THEME":
      return {
        typography: action.theme.typography,
        palette: action.theme.palette
      };
    default:
        return state;
  }
};
