import http from 'k6/http';
import {check} from 'k6';
import {secrets} from './secrets.ts';

export const BASE_URL = 'https://localhost:8443';

export function GetAuthCookieValue() {
  const res = http.post(
    `${BASE_URL}/auth/login`,
    JSON.stringify({
      email: secrets.email,
      password: secrets.password,
    }),
    {
      headers: {
        'Content-Type': 'application/json',
      },
    }
  );

  check(res, { 'login ok': r => r.status === 200 });

  // 1. Target the raw .NET Identity bearer cookie string
  let authCookie = res.cookies['.AspNetCore.Identity.Bearer'];

  if (!authCookie || !authCookie[0] || !authCookie[0].value) {
    throw new Error("Authentication failed: .AspNetCore.Identity.Bearer cookie payload missing!");
  }

  // 2. Return ONLY the raw string value to cross the VU thread isolation barrier
  return authCookie[0].value;
}

export function loginParams(data) {
  return {
    headers: {
      'Cookie': `.AspNetCore.Identity.Bearer=${data.rawToken}`
    }
  };
}