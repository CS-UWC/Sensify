import exec from 'k6/execution';
import http from 'k6/http';
import { sleep, check} from 'k6';
import { randomItem } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

export const options = {
    // vus: 1000, // virtual users or sensors
    // duration: '30s',
    discardResponseBodies: true,
    cloud: {
        // Project: sensify
        projectID: 3715388,
        // Test runs with the same name groups test runs together.
        name: 'Test (19/09/2024-09:12:22)'
    },
    thresholds: {
        http_req_failed: ['rate<0.01'], // http errors should be less than 1%
        http_req_duration: ['p(95)<200'], // 95% of requests should be below 200ms
      },
    scenarios: {
        contacts: {
          executor: 'ramping-vus',
        //   preAllocatedVUs: 10,
          startVUs: 1000,
          stages: [
            { target: 4000, duration: '10m' }, // linearly go from 3 VUs to 20 VUs for 30s
          ],
        },
      },
};

export default function() {
    const payload = "0100e202290400270506060308070d62";
    const serverPorts = [8080, 8081, 8082, 8083, 8084];

    const port = randomItem(serverPorts);

    const res = http.post(`http://10.10.1.4:${port}/api/record/${exec.vu.idInTest}`, payload);
    check(res, {
        'is status 200': (r) => r.status === 200,
      });
    sleep(1);
}