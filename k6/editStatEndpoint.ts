import http from 'k6/http';
import {BASE_URL, GetAuthCookieValue, loginParams} from "./auth.ts";
import {realisticLoadTest} from "./optionTypes.ts";

export function setup() {
  return {
    rawToken: GetAuthCookieValue()
  };
}

export const options = {
  ...realisticLoadTest
};

export default function (data) {
  http.put(`${BASE_URL}/characters/124/stat/1`, JSON.stringify({ levelTypeId: 4 }), loginParams(data));
}
