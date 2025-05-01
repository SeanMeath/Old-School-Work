#!/bin/bash
i=$1
div=1
while [[ $i -ne $div ]]; do
	i=$(($i/$div))
	div=$(($div+1))
	echo "div=$div,i=$i"
done
echo $div
