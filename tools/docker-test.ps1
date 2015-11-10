$VM="default"
$DOCKER_MACHINE=@(Get-Command docker-machine)
$VBOXMANAGE="$env:VBOX_MSI_INSTALL_PATH\VBoxManage.exe"

If(@(Test-Path $VBOXMANAGE) -ne $true){
    $VBOXMANAGE="$env:VBOX_INSTALL_PATH\VBoxManage.exe"
}

if( $DOCKER_MACHINE -eq $null -or @(Test-Path $VBOXMANAGE) -eq $false) {
  echo "Either VirtualBox or Docker Machine are not installed or are not in the PATH. Please re-run the Toolbox Installer and try again."
  exit 1
}

$vmExists=@(& $VBOXMANAGE showvminfo $VM 2>&1| Out-Null)


If ( $LASTEXITCODE -eq 1 ) {
  echo "Creating Machine $VM...(be patient. This can take several min and will prompt for admin actions to add network interface)"
  docker-machine rm -f $VM 2>&1| Out-Null
  Remove-Item $env:USERPROFILE/.docker/machine/machines/$VM -Force -Recurse -ErrorAction SilentlyContinue
  docker-machine create -d virtualbox --virtualbox-memory 2048 --virtualbox-disk-size 204800 $VM
} Else {
  echo "Machine $VM already exists in VirtualBox."
}

$VM_STATUS=@(docker-machine status $VM)
If ( "$VM_STATUS" -ne "Running" ) {
  echo "Starting machine $VM..."
  docker-machine start $VM
}

echo "Setting environment variables for machine $VM"
docker-machine env $VM --shell=powershell | Invoke-Expression

docker pull natemcmaster/aspnet-test:latest

echo "Building docker image..."
Set-Location $args[0]
docker build --rm --pull=false . | Out-String | %{ $id=$_.Substring($_.IndexOf("Successfully built") + 19, 12); $log=$_}
$rc = $LASTEXITCODE
echo "$log"
If($rc -ne 0) {
  exit $rc
}

echo "Built docker image $id"

echo "Running docker command '$($args[1])'"
docker run -t --rm $id $args[1]
$rc = $LASTEXITCODE

Write-Host "Docker exited with $rc" -ForegroundColor ("Red","Green")[$rc -eq 0]
echo "Cleaning up docker image $id"

docker rmi $id | Out-Null
exit $rc
