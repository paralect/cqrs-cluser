# publish binaries
docker-compose -f docker-compose.ci.build.yml up

# build dev images
docker-compose -f docker-compose.yml -f docker-compose.override.yml build

# run dev images
docker-compose up