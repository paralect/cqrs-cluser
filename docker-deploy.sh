# publish binaries
docker-compose -f docker-compose.ci.build.yml up

# build prod images
docker-compose -f docker-compose.yml -f docker-compose.prod.yml build

# push images to Docker Registry
docker push yarmolovichalex1/frontend
docker push yarmolovichalex1/webapi
docker push yarmolovichalex1/writemodel
docker push yarmolovichalex1/readmodel