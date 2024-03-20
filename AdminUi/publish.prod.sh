
cd ..

VERSION="2.0"

#docker image rm docker.tuwaiqdev.com/tuwaiq-internal:latest
#docker image rm docker.tuwaiqdev.com/tuwaiq-internal:v$VERSION
docker image rm tuwaiq-internals:latest
docker image rm tuwaiq-internals:v$VERSION

docker buildx build --platform linux/amd64 --load -t docker.tuwaiqdev.com/tuwaiq-internals:v$VERSION -f AdminUi/Dockerfile . --no-cache

docker push docker.tuwaiqdev.com/tuwaiq-internals:v$VERSION

docker tag docker.tuwaiqdev.com/tuwaiq-internals:v$VERSION docker.tuwaiqdev.com/tuwaiq-internals:latest

docker push docker.tuwaiqdev.com/tuwaiq-internals:latest
