FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN apt-get upgrade -yq && apt-get update && apt-get install -yq zip && apt-get install -yq curl && apt-get install -yq awscli 
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash - && apt-get install -yq nodejs

ENV WAITFORIT_VERSION="v2.4.1"
RUN curl -o /usr/local/bin/waitforit -sSL https://github.com/maxcnunes/waitforit/releases/download/$WAITFORIT_VERSION/waitforit-linux_amd64 \
    && chmod +x /usr/local/bin/waitforit

RUN npm install -g serverless
RUN dotnet tool install --global Amazon.Lambda.Tools --version 3.3.1 && dotnet tool install --global LocalStack.AwsLocal --version 1.3.0
ENV PATH="/root/.dotnet/tools:${PATH}" 

RUN mkdir -p /usr/src/Armut.EventProcessor

COPY Armut.EventProcessor/Armut.EventProcessor.csproj /usr/src/Armut.EventProcessor/
COPY Armut.EventProcessor/Function.cs /usr/src/Armut.EventProcessor/
COPY Armut.EventProcessor/serverless.yml /usr/src/Armut.EventProcessor/

COPY . /usr/src/

WORKDIR /usr/src/Armut.EventProcessor

RUN npm install serverless-localstack

RUN dotnet restore
RUN dotnet lambda package --configuration Release --framework netcoreapp3.1 --output-package artifact/processor-lambda-csharp.zip
RUN chmod +x ./replace-localstack.sh

CMD ./replace-localstack.sh && waitforit -address=http://"$LOCALSTACK_HOST":4566 -timeout=120 -- \ 
        echo $CONN_STR \ 
        && echo $DOCKER_GATEWAY_HOST \ 
        && awslocal s3api list-buckets \ 
        && serverless deploy --verbose --stage local