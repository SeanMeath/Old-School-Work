#!/bin/bash
set -euo pipefail
IFS=$'\n'

allLoggers=$(cat /etc/passwd | egrep "^1[0-9]{6}" | cut -f1 -d:)

files=$(ls /var/log/wtmp* | sort -r)

for user in $allLoggers; do

    logins=()

    for file in $files; do
        logins+=($(last -f $file | tac | grep $user || true))
    done

    name=$(cat /etc/passwd | grep $user | awk -F ':' '{ print $5 }')
    totalLogins=${#logins[@]}

    if [[ totalLogins -eq 0 ]]; then
        printf "Processing $name ($user), found 0 logins (combined 0 logins).  Total logged in time (-10:0) \n"
        continue
    fi

    # https://stackoverflow.com/questions/325933/determine-whether-two-date-ranges-overlap?fbclid=IwAR2Rghnb2ZfTUQ94OdSmUjmzRfnUg4Cmg1zUKCYI6VhGxCf98gT60bzz2OY
    #                   |---- DateRange A ------|
    # |---Date Range B -----|

    lastStart=0
    lastEnd=0
    totalTime=0
    numLogs=0

    for login in ${logins[@]}; do

        dateStart=$(date -d "$(echo $login | awk '{ print $5" "$6" "$7 }')" +%s)

        #CHECKS CRASHES AND RUNNING SESSIONS
        logIn=$(echo $login | grep 'still logged in' | wc -l || true)
        crash=$(echo $login | grep 'crash' | wc -l || true)

        #GETS END
        if [[ logIn -eq 1 ]]; then
            dateEnd=$(date +%s)
        elif [[ crash -eq 1 ]]; then
            dateEnd=$dateStart
        else
            dateEnd=$(date -d "$(echo $login | awk '{ print $5" "$6" "$9 }')" +%s)
        fi
        
        #CALC INTERSECTS
        if [[ $lastEnd -lt $dateStart ]]; then
            numLogs=$(( numLogs + 1 ))
        elif [[ $lastEnd -ge $dateStart ]]; then
            difDate=$(( lastEnd - dateStart ))
            totalTime=$(( totalTime - difDate ))
        fi

        #CALCS TOTAL TIME
        if [[ $dateStart -gt $dateEnd ]]; then
            timeOfLogin=$(( 86400 - $dateStart + $dateEnd ))
        else
            timeOfLogin=$(( dateEnd - dateStart ))
        fi

        totalTime=$(( timeOfLogin + totalTime ))
        

        #RESETS LAST
        lastStart=$dateStart
        lastEnd=$dateEnd
    done

    totalTime=$(( totalTime - 36000 ))

    hours=$(( $totalTime / 3600 ))
    leftOver=$(( $totalTime % 3600 ))
    minutes=$(( $leftOver / 60 ))

    printf "Processing \e[0;32m$name\e[m ($user), found $totalLogins logins (combined $numLogs logins).  Total logged in time (\e[0;32m$hours\e[m:\e[0;32m$minutes\e[m) \n"
done

# Processing Jeremy Barnard-Rycroft (1555594), found 29 logins (combined 12 logins).  Total logged in time (5:12)