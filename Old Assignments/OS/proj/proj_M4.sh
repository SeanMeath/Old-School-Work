#!/bin/bash
set -euo pipefail

dns=$(cat /etc/resolv.conf | sed -n '3p' | awk '{ print $2 }')
host=$(hostname)
wget -q --spider http://google.com
connection=""
if [ $? -eq 0 ]; then
    connection="Connected"
else
    connection="Disconnected"
fi
publicIP=$(curl -s http://ipecho.net/plain)
privateIP=$(ifconfig | sed -n '2p' | awk '{ print $2 }')
loggedUsers=$(ps -ua | tail -n +2 | awk '{ print $1 }' | sort -u | tr '\n' ',')
loggedUsers=${loggedUsers%?};
readyProcs=$(top -b -n 1 | awk '{ print $8 }' | grep R | wc -l || true)
waitingProcs=$(top -b -n 1 | awk '{ print $8 }' | grep D | wc -l || true)
loadAvg=$(uptime | awk -F ',' '{ print $4 }' | sed -e 's/^[ \t]*//' | awk -F ':' '{ print $2 }')
#loadAvg=$(uptime | awk -F '  ' '{ print $3 }') #NOT SURE IF THIS ONE IS THE RIGHT ONE
uptime=$(uptime | awk '{ print $3, $4, $5 }' | tr ',' ' ' | sed -e 's/\s\+/ /g')
printf "\e[0;32m%-60s\e[m %s \n" "Internet:" "$connection"
printf "\e[0;32m%-60s\e[m %s \n" "Hostname:" "$host"
printf "\e[0;32m%-60s\e[m %s \n" "Private IP Address (internal):" "$privateIP"
printf "\e[0;32m%-60s\e[m %s \n" "Public IP Address (external):" "$publicIP"
printf "\e[0;32m%-60s\e[m %s \n" "DNS Server(s):" "$dns"
printf "\e[0;32m%-60s\e[m %s \n" "Logged In users:" "$loggedUsers"
printf "\e[0;32m%-60s\e[m %s \n" "# processes in ready queue:" "$readyProcs"
printf "\e[0;32m%-60s\e[m %s \n" "# processes in wait queue:" "$waitingProcs"
printf "\e[0;32m%-60s\e[m %s \n" "Disk Usages:" "$(df -h /boot | sed -n '1p')"
printf "\e[0;32m%-60s\e[m %s \n" "" "$(df -h /boot | sed -n '2p')"
printf "\e[0;32m%-59s\e[m %s \n" "Load Average:" "$loadAvg"
printf "\e[0;32m%-60s\e[m %s \n" "System Uptime Days/(HH:MM):" "$uptime"