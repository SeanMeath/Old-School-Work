#!/bin/bash

declare -rx PROCESS_FILE="/var/asg4/processes.csv"
declare -rx DELIMITER=','
declare -rix AGING_FREQUENCY=2
declare -Arx FIELDNAMES=(   [pid]=0 [tid]=1 [priority]=2 [remainingTime]=3 \
                            [availTime]=4 [burstTime]=5 [agedPriority]=6 \
                            [numAgingSkips]=7 [remainingBurstTime]=8 \
)
declare -rx IO_BLOCKTIME=6
declare -rx QUANTUM=150
declare -rx HW_CS_TIME=8
declare -rx LW_CS_TIME=2
declare -rix NUM_PROCESSES=`wc -l $PROCESS_FILE | egrep -o '^[^ ]* '`
declare -arx PROCESS_INDEXES=(`seq 0 $(( $NUM_PROCESSES - 1 ))`)
declare -rx NUMFIELDS=$(head -n 1 $PROCESS_FILE | egrep -o , | wc -l)

declare -a _processes

function readFile(){
    readarray _processes < $PROCESS_FILE
    initializeCalculatedFields
}

function getField(){ 
    local procIdx_=$1
    local fieldName_=$2
    local retVal_=$3
    local p=${_processes[$procIdx_]}
    local regExp="";
    local i=0
    local endL=${FIELDNAMES[$fieldName_]}
    for ((i=1; $i<=$endL; i++)); do
       regExp="$regExp*$DELIMITER" 
    done
    begin=${p#$regExp};
    eval $retVal_="${begin%%,*}"
}

function setField(){
    local procIdx_=$1
    local fieldName_=$2
    local value_=$3
    local p=${_processes[$procIdx_]}
    local pos=${FIELDNAMES[$fieldName_]}
    arr=(${p//$DELIMITER/ })
    arr[$pos]=$value_
    arr_str=${arr[*]}
    p=${arr_str// /$DELIMITER}
    _processes[$procIdx_]=$p
}

function initializeCalculatedFields(){
    for proc in "${PROCESS_INDEXES[@]}"; do
        getField $proc 'priority' prio
        getField $proc 'burstTime' burstTime
        setField $proc 'agedPriority' $prio
        setField $proc 'numAgingSkips' 0
        setField $proc 'remainingBurstTime' $burstTime
    done
}

function getRemainingTime(){
    local ret=$1
    
    local sum=0
    local proc=-1
    for proc in "${PROCESS_INDEXES[@]}"; do
        timeLeft=-1
        getField $proc 'remainingTime' 'timeLeft'
        
        sum=$(( sum + timeLeft ))
    done

    eval $ret="$sum"
}

function getEligibleProcesses(){
    local currentTime_=$1
    local lastProc_=$2
    local output_=$3
    
    local eligibleProcs=()
    for c in "${PROCESS_INDEXES[@]}"; do
        [[ $c -eq $lastProc_ ]] && continue
        getField $c 'remainingTime' remTime
        getField $c 'availTime' avaTime
        
        if [[ $remTime -gt 0 ]] && [[ $avaTime -le $currentTime_ ]]; then
            eligibleProcs+=($c)
        fi
    done

    #DEBUG
    # for c in ${eligibleProcs[@]}; do
    #     getField $c 'tid' ti
    #     getField $c 'agedPriority' ap
    #     getField $c 'priority' pri
    #     getField $c 'remainingBurstTime' rbt
    #     getField $c 'numAgingSkips' ns
    #     echo "Proc: $ti with $ns Skips, BasePrio: $pri and CurPrio: $ap and Burst Time: $rbt"
    # done

    eval $output_="(${eligibleProcs[*]})"
}

function getNextProcess(){
    local currentTime_=$1
    local lastProc_=$2
    local ret_=$3
    
    local eligibleArr=-1
    getEligibleProcesses $currentTime_ $lastProc_ 'eligibleArr'

    if [[ ${#eligibleArr[@]} -eq 0 ]]; then
        eval $ret_="-1"
        return
    fi

    local numEligibleProcs=${#eligibleArr[@]}
    local eligibleIndexes=(`seq 0 $(( $numEligibleProcs - 1 ))`)

    local nextPriority=-1
    local bestBurst=-1
    local indexToRemove=0

    local nextPriority=-1
    local bestBurst=-1
    getField ${eligibleArr[0]} 'agedPriority' nextPriority
    getField ${eligibleArr[0]} 'remainingBurstTime' bestBurst

    local currentPrio=-1
    local currentBurst=-1

    for l in "${eligibleIndexes[@]}"; do
        getField ${eligibleArr[$l]} 'agedPriority' currentPrio
        getField ${eligibleArr[$l]} 'remainingBurstTime' currentBurst
        if [[ $currentPrio -gt $nextPriority ]]; then
            indexToRemove=$l
            nextPriority=$currentPrio
            bestBurst=$currentBurst
        elif [[ $currentPrio -eq $nextPriority ]] && [[ $currentBurst -le $bestBurst ]]; then
            indexToRemove=$l
            nextPriority=$currentPrio
            bestBurst=$currentBurst
        fi
    done

    eval $ret_="${eligibleArr[$indexToRemove]}"
    unset eligibleArr[$indexToRemove]

    if [[ ${#eligibleArr[@]} -eq 0 ]]; then
        return
    fi
    ageProcesses "${eligibleArr[@]}"
}

function runProcess(){
    local procToRun_=$1
    local curTime_=$2
    local retTime_=$3

    #DEBUG
    #getField $procToRun_ 'tid' t
    #getField $procToRun_ 'priority' pr
    #getField $procToRun_ 'remainingBurstTime' cb
    #getField $procToRun_ 'agedPriority' apr
    #echo "tid: $t Initial Prio: $pr Prio: $apr Rem Burst: $cb"

    local procRemBurst=-1
    local procRemTime=-1
    getField $procToRun_ 'remainingBurstTime' procRemBurst
    getField $procToRun_ 'remainingTime' procRemTime

    local rt=-1

    if [[ $procRemBurst -le $QUANTUM ]] && [[ $procRemBurst -le $procRemTime ]]; then
        rt=$procRemBurst
        local burst=-1
        getField $procToRun_ 'burstTime' burst
        setField $procId 'remainingBurstTime' $burst
        
        newAvailTime=$(($curTime_+$IO_BLOCKTIME+$rt))
        setField $procId 'availTime' $newAvailTime

        setField $procToRun_ 'numAgingSkips' 0
    elif [[ $QUANTUM -le $procRemTime ]]; then
        rt=$QUANTUM
        newBurst=$(($procRemBurst-$QUANTUM))
        setField $procId 'remainingBurstTime' $newBurst

        setField $procToRun_ 'numAgingSkips' 1
    else
        rt=$procRemTime
    fi

    local basePrio=-1
    getField $procToRun_ 'priority' basePrio
    setField $procToRun_ 'agedPriority' $basePrio
    
    eval "$retTime_='$rt'"
}


function displayStats(){
    local hwCSCount_=$1
    local lwCSCount_=$2
    local idleTime_=$3
    local totalTime_=$4

    local cpuUtilisation=-1
    local processingTime=-1
    local hwTime=-1
    local lwTime=-1

    hwTime=$(($hwCSCount_*$HW_CS_TIME))
    lwTime=$(($lwCSCount_*$LW_CS_TIME))

    cpuUtilisation=$(echo "scale=2 ; (100 - (100 * $idleTime_ / $totalTime_))" | bc)
    processingTime=$(echo "scale=2 ; (100 - (100 * ($idleTime_ + $hwTime + $lwTime) / $totalTime_))" | bc)

    echo '-----------------------------------------------------------------------------------------------------------------------------------------------------------------'
    printf "%-59s : %7s \n" "CPU Utilization (100 - (idle / in use))" "$cpuUtilisation%"
    printf "%-59s : %7s \n" "# of lightweight context switches" "$lwCSCount_"
    printf "%-59s : %7s \n" "# of heavyweight context switches" "$hwCSCount_"
    printf "%-59s : %7s \n" "Lightweight context switch time" "$lwTime"
    printf "%-59s : %7s \n" "Heavyweight context switch time" "$hwTime"
    printf "%-59s : %7s \n" "% CPU time spent processing (i.e. not CS'ing, not idle)" "$processingTime%"
}

function ageProcesses(){
    local procsToAge_=("$@")
    local numProcsToAge=${#procsToAge_[@]}
    
    local currProcSkips=0 # getField
    local procAge=0 # getField
    local oldProcAged=0 # getField
    local i=0
    local currProc=-1

    for (( i=0; i<numProcsToAge; i++ )); do
        currProc=${procsToAge_[$i]}

        getField $currProc 'numAgingSkips' currProcSkips
        setField $currProc 'numAgingSkips' $(( $currProcSkips + 1 ))
        getField $currProc 'numAgingSkips' currProcSkips

        if [[ $currProcSkips -eq $AGING_FREQUENCY ]]; then
            setField $currProc 'numAgingSkips' 0
            
            getField $currProc 'agedPriority' oldProcAged
            getField $currProc 'agedPriority' procAge
            procAge=$(($procAge+1))
            setField $currProc 'agedPriority' $procAge
            
            #DEBUG
            #getField $currProc 'agedPriority' procAge
            #getField $currProc 'tid' t
            #getField $currProc 'availTime' at
            #getField $currProc 'priority' pr
            #getField $currProc 'remainingBurstTime' cb
            #echo "tid: $t AvailTime: $at ,Initial Prio: $pr Prio Before Age: $oldProcAged Prio After Age: $procAge Rem Burst: $cb"
        fi

        #DEBUG
        #getField $currProc 'tid' t
        #getField $currProc 'numAgingSkips' nas
        #getField $currProc 'agedPriority' pa
        #echo "tid: $t, skips: $nas, prio: $pa"

    done
}

function idleTime(){
    local time_=$1
    local ret_=$2

    local procTimes=()

    local i=-1

    #get all idle times
    for i in "${PROCESS_INDEXES[@]}"; do
        local availT=-1
        getField $i 'availTime' availT
        if [[ $availT -ge $time_ ]]; then
            procTimes+=($availT)
        fi
    done

    #get lowest idle time
    local min=${procTimes[0]}
    for a in "${procTimes[@]}"; do
        ((a < min)) && min=$a
    done

    eval $ret_="$min"
}

readFile

currentTime=0
lastProc=-1
totalRemTime=-1
wasIdle=0

getRemainingTime 'totalRemTime'

#stat variables
hwCSCount=0
lwCSCount=0
timeSpentIdle=0

while [ $totalRemTime -gt 0 ]; do
    procId=-1

    #avoids missing the last process
    if [[ $wasIdle -eq 1 ]]; then
        wasIdle=0
        getNextProcess $currentTime -1 'procId'
    else
        getNextProcess $currentTime $lastProc 'procId'
    fi

    #idle
    if [[ $procId -eq -1 ]]; then
        wasIdle=1
        idleTime $currentTime 'idleT'  # when to stop idling
        timeSpentIdle=$(($timeSpentIdle-$currentTime+$idleT))
        printf "%5s..%5s: ********** IDLE **********, Total time remaining (all threads): $totalRemTime.\n" "$currentTime" "$(($idleT-1))"
        currentTime=$idleT
        continue
    fi
    
    curPID=-1
    curTID=-1
    getField $procId 'pid' curPID
    getField $procId 'tid' curTID

    #context switches
    if [[ $lastProc -eq -1 ]]; then # was idle
        newTime=$(($currentTime+$HW_CS_TIME))
        printf "start[%5s], end[%5s], status: HW context switch\n" "$currentTime" "$(($newTime-1))"
        currentTime=$newTime
        hwCSCount=$(($hwCSCount+1))
    else
        getField $lastProc 'pid' lastPID
        getField $lastProc 'tid' lastTID

        if [[ $curPID -ne $lastPID ]]; then # different process
            newTime=$(($currentTime+$HW_CS_TIME))
            printf "start[%5s], end[%5s], status: HW context switch\n" "$currentTime" "$(($newTime-1))"
            currentTime=$newTime
            hwCSCount=$(($hwCSCount+1))
        elif [[ $curTID -ne $lastTID ]]; then # different thread
            newTime=$(($currentTime+$LW_CS_TIME))
            printf "start[%5s], end[%5s], status: LW context switch\n" "$currentTime" "$(($newTime-1))"
            currentTime=$newTime
            lwCSCount=$(($lwCSCount+1))
        fi
    fi
    
    getField $procId 'burstTime' burst
    getField $procId 'remainingBurstTime' remBurst
    getField $procId 'remainingTime' remTime
    getField $procId 'agedPriority' curPrio
    getField $procId 'availTime' curAvail

    runTime=-1
    runProcess $procId $currentTime 'runTime'

    newTime=$(($currentTime+$runTime))

    lastProc=$procId
    newRemTime=$(($remTime-$runTime))
    setField $procId 'remainingTime' $newRemTime
    
    printf "start[%5s], end[%5s], pid[%2s], tid[%3s], priority[%2s], runtime[%3s], availTime[%5s], remaining time[%4s], burst length [%3s].  Status: " "$currentTime" "$(($newTime-1))" "$curPID" "$curTID" "$curPrio" "$runTime" "$curAvail" "$remTime" "$remBurst"
    if [[ $runTime -eq $remTime ]]; then # process done
        printf "Terminated\n"
    elif [[ $runTime -eq $remBurst ]]; then # process blocked
        printf "Blocking I/O\n"
    elif [[ $runTime -eq $QUANTUM ]]; then # quantum expired
        printf "Quantum Expired\n"
    fi

    currentTime=$newTime
    getRemainingTime totalRemTime
    printf "Total time remaining (all threads): %s.\n" "$totalRemTime"
done

displayStats $hwCSCount $lwCSCount $timeSpentIdle $currentTime