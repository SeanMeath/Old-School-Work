fact=1
num=$1
for i in $(seq 1 $num); do
	fact=$(($fact * $i))
done
echo $fact
