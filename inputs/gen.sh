#!/bin/sh

echo -n "" > inputs.txt

for cities in 10 20 30 40 50 
do
places=`expr $cities \* 4`
trucks=`expr $cities \* 2`
planes=`expr $cities \* 2`
packages=`expr $cities \* 10`
../Generator.exe $cities $places $trucks $planes $packages output-$cities.in
echo  output-$cities.in >> inputs.txt
done
