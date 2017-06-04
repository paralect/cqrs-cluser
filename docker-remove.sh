# remove all CQRS cluster containers with volumes
docker rm -f -v cqrscluster_frontend_1
docker rm -f -v cqrscluster_webapi_1
docker rm -f -v cqrscluster_writemodel_1
docker rm -f -v cqrscluster_readmodel_1
docker rm -f -v rabbitmq
docker rm -f -v eventstore
docker rm -f -v redis
docker rm -f -v mongo

# remove Frontend, Webapi, WriteModel, ReadModel images
docker rmi -f $(docker images | grep 'frontend\|webapi\|writemodel\|readmodel' | awk '{ print $3 }' )

# remove dangling images
docker rmi $(docker images -q -f dangling=true)

# remove dangling volumes 
docker volume rm $(docker volume ls -q -f dangling=true)