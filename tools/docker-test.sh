#!/usr/bin/env bash

VM=default
DOCKER_MACHINE=$(which docker-machine)
VBOXMANAGE=$(which VBoxManage)

unset DYLD_LIBRARY_PATH
unset LD_LIBRARY_PATH

if [ ! -f $DOCKER_MACHINE ] || [ ! -f $VBOXMANAGE ]; then
  echo "Either VirtualBox or Docker Machine are not installed or are not in the PATH. Please re-run the Toolbox Installer and try again."
  exit 1
fi

$VBOXMANAGE showvminfo $VM &> /dev/null
VM_EXISTS_CODE=$?

if [ $VM_EXISTS_CODE -eq 1 ]; then
  echo "Creating Machine $VM..."
  $DOCKER_MACHINE rm -f $VM &> /dev/null
  rm -rf ~/.docker/machine/machines/$VM
  $DOCKER_MACHINE create -d virtualbox --virtualbox-memory 2048 --virtualbox-disk-size 204800 $VM
else
  echo "Machine $VM already exists in VirtualBox."
fi

VM_STATUS=$($DOCKER_MACHINE status $VM)
if [ "$VM_STATUS" != "Running" ]; then
  echo "Starting machine $VM..."
  $DOCKER_MACHINE start $VM
fi

echo "Setting environment variables for machine $VM"
eval $($DOCKER_MACHINE env $VM --shell=bash)

echo "Building docker machine..."
docker pull natemcmaster/aspnet-test:latest
buildLog=$(cd $1 ; docker build --rm --pull=false .)
rc=$?

echo "$buildLog"
if [[ $rc != 0 ]]; then
  exit $rc;
fi

ID=$(echo $buildLog | awk '{print $NF}')
echo "Built docker image $ID"

echo "Running docker command: '$2'"
docker run -t --rm $ID $2
rc=$?
echo "Docker exited with $rc"
echo "Cleaning up docker $ID"

docker rmi $ID 2>/dev/null >/dev/null
exit $rc
