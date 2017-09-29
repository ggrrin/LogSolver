#!/bin/sh

echo -n "" > inputs.txt

for p in 2 4 6 8
do
	for cities in 1 2 3 4 5 6 7 8   
	do
	
		places=`expr $cities \* $p`

		trucks=`expr $places \* 2`
		planes=`expr $cities \* 2`
		packages=`expr $cities \* 10`
		for i in 1 2 
			do
			file=output-$cities-${places}_$i.in

			../Generator.exe $cities $places $trucks $planes $packages $file
			echo  $file >> inputs.txt
		done
	done
done
