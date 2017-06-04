# Remove all CQRS cluster containers with related volumes; Frontend, Webapi, WriteModel, ReadModel images; dangling volumes  

docker rm -f -v $(docker ps -a -q)
docker rmi -f $(docker images | grep 'frontend\|webapi\|writemodel\|readmodel' | awk '{ print $3 }' )
docker volume rm $(docker volume ls -q -f dangling=true)