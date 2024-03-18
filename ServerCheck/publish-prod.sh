cd ..

docker image rm docker.tuwaiqdev.com/tuwaiq-hadaf-service:latest
docker image rm docker.tuwaiqdev.com/tuwaiq-hadaf-service:v1.0
docker image rm tuwaiq-hadaf-service:latest
docker image rm tuwaiq-hadaf-service:v1.0
docker build -t docker.tuwaiqdev.com/tuwaiq-hadaf-service:v1.0 -f Tuwaiq-ServerCheck/Dockerfile . #--no-cache
docker tag docker.tuwaiqdev.com/tuwaiq-hadaf-service:v1.0 docker.tuwaiqdev.com/tuwaiq-hadaf-service:latest
docker push docker.tuwaiqdev.com/tuwaiq-hadaf-service:v1.0
docker push docker.tuwaiqdev.com/tuwaiq-hadaf-service:latest
