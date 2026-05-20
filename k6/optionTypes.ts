

export const stressTest = {
  insecureSkipTLSVerify: true,
  stages: [
    { duration: '30s', target: 100 },
    { duration: '30s', target: 200 },
    { duration: '30s', target: 300 },
    { duration: '30s', target: 400 },
    { duration: '1m', target: 400 },
    { duration: '30s', target: 200 },
    { duration: '30s', target: 0 },
  ],
};


export const realisticLoadTest = {
  insecureSkipTLSVerify: true,
  thresholds: {
    http_req_duration: [
      'avg<275',
      'p(95)<500',
    ],
    http_reqs: [
      'rate>50',
    ],
    http_req_failed: ['rate<0.01'],
  },
  stages: [
    { duration: '30s', target: 30 },
    { duration: '2m', target: 30 },
    { duration: '30s', target: 0 },
  ],
};

export const performanceTargetTests = {
  insecureSkipTLSVerify: true,
  thresholds: {
    http_req_duration: [
      'avg<100',
      'p(95)<200',
    ],
    http_reqs: [
      'rate>100',
    ],
    http_req_failed: ['rate<0.01'],
  },
  stages: [
    { duration: '30s', target: 100 },
    { duration: '2m', target: 100 },
    { duration: '30s', target: 0 },
  ],
};