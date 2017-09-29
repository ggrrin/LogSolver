#!/bin/sh

function draw(){
file="$1"
title="$2"
xlabel="$3"
ylabel="$4"
zlabel="$5"
graphTitle="$6"
of=`basename -s .csv $file`

/c/Users/petrg/Desktop/gnuplot/bin/gnuplot.exe << END
set grid xtics ytics ztics 
set xlabel '$xlabel'
set ylabel '$ylabel'
set zlabel '$zlabel'
set terminal png size 720,720
#set zrange [0:200]
#set logscale x 2 
#set logscale y 2 
#set logscale z 2 
#set format x '%g'
#set dgrid3d box 
set output 'png/$of.png'
set term png
set ticslevel 0
set style line 1 linecolor rgb '#0060ad' linetype 2 linewidth 8 
set hidden3d
set title "$graphTitle"
splot '$file'  title "$title" with impulses linestyle 1 
END

}


function draw_versions() {

for f in $1*
do
	draw "$f" "$2" "$3" "$4" "$5" "$6" 
done
}

#print places " " seconds >>  "csv/times-c" cities "_v" variant ".csv"
#print cities " " seconds >>  "csv/times-p" places/cities "_v" variant ".csv"


mkdir csv 2> /dev/null
rm -f ./csv/*

grep -E -e $'^[^[0-9\t]' stats.txt | gawk '
/Running/ {
	split($2, c, /[-_\.]/)
	cities = c[2]
	places = c[3]
	variant = c[4]
}


/Time/ {
	split($2, t, /:/)
	seconds = t[1]*60*60 + t[2]*60 + t[3]


	print cities " " places " " seconds >>  "csv/times_v" variant ".csv"
}

/Expand/ {
	print cities " " places " " $2 >>  "csv/expand_v" variant ".csv"

}

/Depth/ {
	print cities " " places " " $2 >>  "csv/depth_v" variant ".csv"

}

/Price/ {
	print cities " " places " " $2 >>  "csv/price_v" variant ".csv"

}
'

mkdir png  2> /dev/null
rm -f ./png/*


draw_versions ./csv/times_v "" "Number of cities" "Number of sites" "Time [s]" "Search times"
draw_versions ./csv/expand_v "" "Number of cities" "Number of sites" "Expand nodes" "Search expand nodes"
draw_versions ./csv/depth_v "" "Number of cities" "Number of sites" "Depth" "Search depths"
draw_versions ./csv/price_v "" "Number of cities" "Number of sites" "Price [eur]" "Search prices"
