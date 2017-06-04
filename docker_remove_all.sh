docker rm -f -v $(docker ps -a -q)
docker rmi -f $(docker images | grep 'frontend\|webapi\|writemodel\|readmodel' | awk '{ print $3 }' )
docker volume rm $(docker volume ls -q -f dangling=true)