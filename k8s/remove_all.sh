#!/bin/bash

kubectl delete -f "application.yaml"
kubectl delete -f "mongo.yaml"
kubectl delete -f "eventstore.yaml"
kubectl delete -f "redis.yaml"

kubectl delete statefulsets "rabbitmq"
kubectl delete service "rabbitmq"
kubectl delete service "rabbitmq-management"
kubectl delete service "rmq-cluster"

kubectl delete pods --all
kubectl delete pvc --all

kubectl delete -f "storage.yaml"