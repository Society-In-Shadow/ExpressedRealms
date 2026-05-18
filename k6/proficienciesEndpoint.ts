import http from 'k6/http';
import {BASE_URL, GetAuthCookieValue, loginParams} from "./auth.ts";
import {stressTest} from "./optionTypes.ts";

export function setup() {
  return {
    rawToken: GetAuthCookieValue()
  };
}

export const options = {
  ...stressTest
};

export default function (data) {

  http.get(`${BASE_URL}/proficiencies/124`, loginParams(data));

}
