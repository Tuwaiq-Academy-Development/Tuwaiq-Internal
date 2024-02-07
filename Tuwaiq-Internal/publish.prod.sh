
cd ..

docker image rm docker.tuwaiqdev.com/tuwaiq-internal:latest
docker image rm docker.tuwaiqdev.com/tuwaiq-internal:v1.0
docker image rm tuwaiq-internal:latest
docker image rm tuwaiq-internal:v1.0
docker buildx build --platform linux/amd64 --load -t docker.tuwaiqdev.com/tuwaiq-internal:v1.0 -f Tuwaiq-Internal/Dockerfile . --no-cache
docker push docker.tuwaiqdev.com/tuwaiq-internal:v1.0
docker tag docker.tuwaiqdev.com/tuwaiq-internal:v1.0 docker.tuwaiqdev.com/tuwaiq-internal:latest
docker push docker.tuwaiqdev.com/tuwaiq-internal:latest
