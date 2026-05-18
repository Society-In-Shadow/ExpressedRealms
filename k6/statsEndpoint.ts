import http from 'k6/http';
import {BASE_URL, GetAuthCookieValue, loginParams} from "./auth.ts";
import {normalSustainTest} from "./optionTypes.ts";

export function setup() {
  return {
    rawToken: GetAuthCookieValue()
  };
}

export const options = {
  ...normalSustainTest
};

export default function (data) {

  http.get(`${BASE_URL}/characters/124/stats`, loginParams(data));

}
