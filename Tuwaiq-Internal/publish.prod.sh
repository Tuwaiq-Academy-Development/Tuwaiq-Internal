
cd ..

docker image rm docker.tuwaiqdev.com/tuwaiq-hiring:latest
docker image rm docker.tuwaiqdev.com/tuwaiq-hiring:v1.1
docker image rm tuwaiq-hiring:latest
docker image rm tuwaiq-hiring:v1.1
docker buildx build --platform linux/amd64 --load -t docker.tuwaiqdev.com/tuwaiq-hiring:v1.1 -f TuwaiqRecruitment/Dockerfile . #--no-cache
docker push docker.tuwaiqdev.com/tuwaiq-hiring:v1.1
docker tag docker.tuwaiqdev.com/tuwaiq-hiring:v1.1 docker.tuwaiqdev.com/tuwaiq-hiring:latest
docker push docker.tuwaiqdev.com/tuwaiq-hiring:latest
