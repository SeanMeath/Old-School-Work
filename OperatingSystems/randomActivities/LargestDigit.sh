#!/bin/bash

input=$1
max=${input:0:1}
for (( i=0; i<${#input}; i++ )) do
	if [[ ${input:$i:1} -gt $max ]]; then
		max=${input:$i:1}
	fi
done
echo $max
