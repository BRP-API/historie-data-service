#!/bin/bash

PARAMS="{ \
    \"apiUrl\": \"http://localhost:8000/haalcentraal/api\", \
    \"logFileToAssert\": \"./test-data/logs/historie-data-service.json\", \
    \"oAuth\": { \
        \"enable\": false \
    } \
}"

npx cucumber-js -f json:./test-reports/cucumber-js/step-definitions/test-result-zonder-dependency-integratie.json \
                -f summary:./test-reports/cucumber-js/step-definitions/test-result-zonder-dependency-integratie-summary.txt \
                -f summary \
                features/docs \
                --tags "not @integratie" \
                --tags "not @skip-verify"


npx cucumber-js -f json:./test-reports/cucumber-js/historie/test-result.json \
                -f summary:./test-reports/cucumber-js/historie/test-result-peildatum-summary.txt \
                -f summary \
                features/raadpleeg-verblijfplaats-met-peildatum \
                --tags "not @skip-verify" \
                --world-parameters "$PARAMS"

npx cucumber-js -f json:./test-reports/cucumber-js/historie/test-result.json \
                -f summary:./test-reports/cucumber-js/historie/test-result-periode-summary.txt \
                -f summary \
                features/raadpleeg-verblijfplaats-met-periode \
                --tags "not @skip-verify" \
                --world-parameters "$PARAMS"