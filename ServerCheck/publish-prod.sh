VERSION="v2.0"
cd ..

docker image rm docker.tuwaiqdev.com/tuwaiq-gosi-service:latest
docker image rm docker.tuwaiqdev.com/tuwaiq-gosi-service:$VERSION
docker image rm tuwaiq-gosi-service:latest
docker image rm tuwaiq-gosi-service:$VERSION
docker buildx build --platform linux/amd64 -t docker.tuwaiqdev.com/tuwaiq-gosi-service:$VERSION -f ServerCheck/Dockerfile . #--no-cache
docker tag docker.tuwaiqdev.com/tuwaiq-gosi-service:$VERSION docker.tuwaiqdev.com/tuwaiq-gosi-service:latest
docker push docker.tuwaiqdev.com/tuwaiq-gosi-service:$VERSION
docker push docker.tuwaiqdev.com/tuwaiq-gosi-service:latest
