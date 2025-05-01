#!/bin/bash
set -u

function descDigits(){
	input=$1
	
	for (( i=0; i<${#input}; i++ )) do
		echo ${input:$i:1}
	done | sort -nr | tr -d '\n'
	echo
}

descDigits $1
