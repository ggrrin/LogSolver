#!/bin/sh

function gen() {
cities=$1 
places=$2 
trucks=$3
planes=$4
packages=$5
../Generator.exe $cities $places $trucks $planes $packages output-$cities-$places-$trucks-$planes-$packages.in
echo output-$cities-$places-$trucks-$planes-$packages.in >> inputs.txt
}

echo -n "" > inputs.txt

#1 cities
#1 truck
gen 1 2 1 1 1
gen 1 2 1 1 3 
gen 1 2 1 1 6 

#2 trucks
gen 1 3 2 1 1
gen 1 3 2 1 3 
gen 1 3 2 1 6 

#2 cities
#1 plane
gen 2 5 2 1 1
gen 2 5 2 1 17 
gen 2 5 2 1 45


#2 planes
gen 2 5 2 2 1
gen 2 5 2 2 17 
gen 2 5 2 2 45
