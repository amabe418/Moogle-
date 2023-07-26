#!/bin/bash
echo "Type an option"
read A 

  cd ..
   if [ "$A" == "run" ];
   then
    dotnet watch run --project MoogleServer
    clear
   fi

   if [ "$A" == "show_report" ];
   then
    cd informe
    read B
    if [ ! -f "moogle.pdf" ];
    then
      pdflatex moogle.tex
    fi
    if [ -n "$B" ];
    then 
       $B moogle.pdf
       else
       #Linux
       open moogle.pdf
       #Windows
       start moogle.pdf
    fi
    cd ..
    clear
   fi
   
  if [ "$A" == "show_slides" ];
  then 
    read B
    cd presentación
    if [ ! -f "Presentacion.pdf" ];
    then
      pdflatex Presentacion.tex
    fi
    if [ -n "$B" ];
    then 
     $B Presentacion.pdf
     else 
      #Linux
      open Presentacion.pdf
      #Windows
      start Presentacion.pdf
    fi
    cd ..
    clear
  fi

  if [ "$A" == "report" ];
  then 
   cd informe
   pdflatex moogle.tex
   cd ..
  fi

  if [ "$A" == "slides" ];
  then 
   cd presentación
   pdflatex Presentacion.tex
   cd ..
   clear
  fi

 if [ "$A" == "clean" ] ; 
 then 
  cd informe
  rm moogle.aux 
  rm moogle.fdb_latexmk 
  rm moogle.fls 
  rm moogle.log 
  rm end.aux
  rm moogle.pdf
  cd ..

  cd presentación
  rm Presentacion.aux 
  rm Presentacion.fdb_latexmk 
  rm Presentacion.fls 
  rm Presentacion.log 
  rm Large.aux
  rm Presentacion.nav
  rm Presentacion.out
  rm Presentacion.toc
  rm Presentacion.vrb
  rm texput.log
  rm Presentacion.pdf
  cd ..
  clear
 fi



