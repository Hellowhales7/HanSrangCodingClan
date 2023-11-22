AWS_REGION="ap-northeast-2"
ECS_CLUSTER="sejongs-cipher-cluster"
ECS_SERVICE="sejongs-cipher-service"
IMAGE_NAME="sejongs-cipher"
PROJECT_DIR="$(cd "$(dirname "$0")" && pwd)/.."

docker build -t sejongs-cipher-web "$PROJECT_DIR/web"
docker tag sejongs-cipher-web:latest 408047345469.dkr.ecr.ap-northeast-2.amazonaws.com/sejongs-cipher-web:latest
docker push 408047345469.dkr.ecr.ap-northeast-2.amazonaws.com/sejongs-cipher-web:latest
aws ecs update-service --cluster $ECS_CLUSTER --service $ECS_SERVICE --force-new-deployment