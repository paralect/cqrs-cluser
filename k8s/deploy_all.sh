#!/bin/bash

kubectl create -f "storage.yaml"
kubectl create -f "mongo.yaml"
kubectl create -f "eventstore.yaml"

export NAMESPACE=default && export DOCKER_REPOSITORY=nanit && export RABBITMQ_REPLICAS=3 && export RABBITMQ_DEFAULT_USER=guest && export RABBITMQ_DEFAULT_PASS=guest && export RABBITMQ_ERLANG_COOKIE=secret && export RABBITMQ_EXPOSE_MANAGEMENT=TRUE && export RABBITMQ_HA_POLICY='{\"ha-mode\":\"all\"}' && export SUDO="" && make --directory "./kubernetes-rabbitmq-cluster" deploy

kubectl create -f "application.yaml"