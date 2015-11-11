#!/usr/bin/env bash
set -e
source $DNX_USER_HOME/dnvm/dnvm.sh
dnvm use default
./approot/test -diagnostics
dnvm use default -r coreclr
./approot/test -diagnostics
exit $?
