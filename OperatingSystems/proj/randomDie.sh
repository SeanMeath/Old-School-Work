#!/bin/bash

BIG_DIE=10
SMALL_DIE=5

dieTime=0
while [ $dieTime -lt $SMALL_DIE ]; do
  dieTime=$RANDOM
  dieTime=$(($dieTime%$BIG_DIE)) # Scales $number down within $RANGE.
done

sleep ${dieTime}s

rd=$RANDOM

if [[ $(($rd%2)) -eq 0 ]]; then
  exit 1
fi
exit 0