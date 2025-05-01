#!/bin/bash

procs=($(ps | sed '1d' | grep 'randomDie.sh'))

while true; do
    if [[ ${#procs[@]} -eq 0 ]]; then
        wait $pid
        exitCode=$?
        if [[ $exitCode -eq 0 ]]; then
            echo "$(date +%d/%m/%Y' '%T): pid $pid just died without error: $exitCode"
        else
            echo "$(date +%d/%m/%Y' '%T): pid $pid just died with error: $exitCode"
        fi
        echo "$(date +%d/%m/%Y' '%T): no running instance of ./randomDie.sh found"
        ./randomDie.sh &
        pid=$!
        echo "$(date +%d/%m/%Y' '%T): re-spawning ./randomDie.sh, pid: $pid, DONE!"
    else
        echo "$(date +%d/%m/%Y' '%T): pid $pid found, ./randomDie.sh still running"
        cpuPer=$(ps -o pid,%cpu,%mem | grep $pid | awk '{ print $2 }')
        memPer=$(ps -o pid,%cpu,%mem | grep $pid | awk '{ print $3 }')
        echo "$(date +%d/%m/%Y' '%T): pid $pid performance stats: using $cpuPer% of system CPU"
        echo "$(date +%d/%m/%Y' '%T): pid $pid performance stats: using $memPer% of system memory"
    fi
    echo "$(date +%d/%m/%Y' '%T): checking health of pid #$pid"
    sleep 1s
    procs=($(ps | sed '1d' | grep 'randomDie.sh'))
done