# publish binaries
docker-compose -f docker-compose.ci.build.yml up

# build prod images
docker-compose -f docker-compose.yml -f docker-compose.prod.yml build

# push images to Docker Registry
docker push $1/frontend
docker push $1/webapi
docker push $1/writemodel
docker push $1/readmodel