#!/bin/bash

sed -i -e 's+http://localhost+http://'"$LOCALSTACK_HOST"'+g' serverless.yml