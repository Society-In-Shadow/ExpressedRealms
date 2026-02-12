#!/bin/bash

curl 'https://localhost:5001/openapi/v1.json' -ks | jq '[.paths[] | keys[]] | length' | awk '{print $0 " endpoints"}'
curl 'https://localhost:8443/openapi/v1.json' -ks | jq '[.paths[] | keys[]] | length' | awk '{print $0 " endpoints"}'

