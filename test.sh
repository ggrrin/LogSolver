

for x in ./$1/*.in 
do 
echo $x 
./Evaluator.exe $x ${x}.out
done
