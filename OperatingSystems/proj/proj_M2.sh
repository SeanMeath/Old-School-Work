#!/bin/bash
set -euo pipefail

echo '------------------------------------------------------------'
printf "\e[0;32m%s\e[m \n" "Disk / Filesystem Statistics"
echo '------------------------------------------------------------'
printf "\e[0;32m%s\e[m \n" "Mounted filesystems (excluding tmpfs):"
driveCount=$(df -hT | grep -v tmpfs | wc -l)
drives=$(df -hT | sed -e 's/\s\+/,/g' | grep -v tmpfs | tail -n $((driveCount-1)))

printf "%-60s %11s%11s%13s%9s%6s \n" " " "Mount Point" "FS Type" "Disk" "Size" "Util%"
printf "%-60s %11s%11s%13s%9s%6s \n" " " "-----------" "-------" "----" "----" "-----"

for drive in $drives; do
   mount=$(echo $drive | awk -F',' '{ print $7 }')
   fs=$(echo $drive | awk -F',' '{ print $2 }')
   disk=$(echo $drive | awk -F',' '{ print $1 }')
   disk=$(echo ${disk##*/})
   size=$(echo $drive | awk -F',' '{ print $3 }')
   util=$(echo $drive | awk -F',' '{ print $6 }')
   printf "%-60s %11s%11s%13s%9s%6s \n" "" "$mount" "$fs" "$disk" "$size" "$util"
done

printf "\e[0;32m%s\e[m \n" "Kernel files in /boot (with permissions):"
bootFiles=$(ls -o /boot | grep vmlinuz | awk '{ print $1,$8 }' | tr ' ' ',')
for bootFile in $bootFiles; do
   name=$(echo $bootFile | awk -F',' '{ print $2 }')
   perm=$(stat -c '%a %n' /boot/* | grep $name | awk '{ print $1 }')
   printf "%-60s /boot/%s (%s) \n" " "  "$name" "$perm"
done


kernel=$(dmesg | grep BOOT_IMAGE | tail -1| awk '{ print $6 }' | awk -F'/' '{ print $2 }')
kernelInode=$(stat /boot/$kernel | sed -n '3p' | awk '{ print $4 }')
printf "\e[0;32m%-60s\e[m %s (%s) \n" "Running kernel file (with inode)" "$kernel" "$kernelInode"

printf "\e[0;32m%-60s\e[m %s \n" "Home directory:" "$HOME"

homeSpace=$(du -h ~ | tail -1 | awk '{ print $1 }')
printf "\e[0;32m%-60s\e[m %s \n" "Disk usage (home directory)" "$homeSpace"

path=$(realpath --relative-to=$HOME /)
printf "\e[0;32m%-60s\e[m %s \n" 'Relative path of / to $HOME' "$path"

pid=$(ps | grep 'proj_M2.sh' | head -1 | awk '{ print $1 }')
printf "\e[0;32m%-60s\e[m \n" "Open files for current process ID ($pid):"

procCount=$(lsof -p $pid | wc -l)
procs=$(lsof -p $pid | awk '{ print $9 }' | tail -n $((procCount-1)))
for proc in $procs; do
   printf "\e[0;32m%-60s\e[m %s \n" "" "$proc"
done