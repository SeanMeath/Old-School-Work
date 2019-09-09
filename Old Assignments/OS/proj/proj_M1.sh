#!/bin/bash
set -euo pipefail

echo ''
echo ''
printf "\e[0;32m%s\e[m \n" "Memory Usage Statistics"
echo '-----------------------------------------------------------'
echo 'Ram Usage:'
free -h
echo ''
echo '-----------------------------------------------------------'

#MEME INFO
totalPhysicalMem=$(cat /proc/meminfo | sed -n '1p' | awk '{ print $2}')
useMem=$(free | sed -n '2p' | awk '{ print $3 }')
freeMem=$(cat /proc/meminfo | sed -n '2p' | awk '{ print $2}')
totalSwap=$(cat /proc/meminfo | sed -n '15p' | awk '{ print $2}')

#MATH STUFF
memBuf=$(free | sed -n '2p' | awk '{ print $6 }')
swapFree=$(free | sed -n '3p' | awk '{ print $4 }')

#OTHER
mostHardFaults=($(ps -eo cmd --sort=majflt | tail -1))
mostUsedShared=$(smem -um --sort=pids | grep '^/usr/lib64' | tail -1 | awk '{ print $1 }')
timesUsed=$(smem -um --sort=pids | grep '^/usr/lib64' | tail -1 | awk '{ print $2 }')
user=$(whoami)

totalMem=$(( totalSwap + totalPhysicalMem ))

#MIGHT BE WRONG
freePhyMemPer=$(echo "scale=2 ; ((100 * ($freeMem + $memBuf) / $totalPhysicalMem))" | bc)
freeMemPer=$(echo "scale=2 ; ((100 * ($freeMem + $memBuf + $swapFree) / ($totalPhysicalMem + $totalSwap)))" | bc)

printf "%-64s%'.f KB \n" "Total physical memory:" $totalPhysicalMem
printf "%-64s%'.f KB \n" "Physical memory in use:" $useMem
printf "%-64s%s \n" "Free physical memory ('free' + available buffers):" "$freePhyMemPer%"
printf "%-64s%'.f KB \n" "Total swap memory:" $totalSwap
printf "%-64s%'.f KB \n" "Total memory:" $totalMem
printf "%-64s%s \n" "Free total memory ('free' + available buffers):" "$freeMemPer%"
printf "%-64s%s %s \n" "Most frequently (hard) page-faulting process:" ${mostHardFaults[0]} ${mostHardFaults[1]}
printf "%-64s%s \n\n" "Current user (whoami):" $user

printf "\e[0;32m%s\e[m \n" "Shared Memory Statics"
echo '-----------------------------------------------------------'
uss=$(smem -u | sed -n '2p' | awk '{print $4}')
pss=$(smem -u | sed -n '2p' | awk '{print $5}')
rss=$(smem -u | sed -n '2p' | awk '{print $6}')
echo 'see http://stackoverflow.com/questions/22372960/is-this-explanation-about-vss-rss-pss-uss-accurately'
printf "%-64s%'.f KB \n" "Memory use by current user (RRS):" $rss
printf "%-64s%'.f KB \n" "Memory use by current user (PSS):" $pss
printf "%-64s%'.f KB \n" "Memory saved by implementing shared memory (current user):" $(( rss - pss ))
printf "%-64s%'.f KB \n" "Memory returned to the system when user logs out:" $uss
printf "%-64s%s (%s times) \n" "Most used shared library:" "$mostUsedShared" "$timesUsed"