
cd ..

VERSION=1.2

#docker image rm docker.tuwaiqdev.com/tuwaiq-internal:latest
#docker image rm docker.tuwaiqdev.com/tuwaiq-internal:v$VERSION
docker image rm tuwaiq-internal:latest
docker image rm tuwaiq-internal:v$VERSION

docker buildx build --platform linux/amd64 --load -t docker.tuwaiqdev.com/tuwaiq-internal:v$VERSION -f Tuwaiq-Internal/Dockerfile . --no-cache

docker push docker.tuwaiqdev.com/tuwaiq-internal:v$VERSION

docker tag docker.tuwaiqdev.com/tuwaiq-internal:v$VERSION docker.tuwaiqdev.com/tuwaiq-internal:latest

docker push docker.tuwaiqdev.com/tuwaiq-internal:latest
