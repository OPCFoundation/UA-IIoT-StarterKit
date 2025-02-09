@echo off
setlocal
set MODELCOMPILER=D:\Work\OPC\UA-ModelCompiler\build\bin\Release\net8.0\
echo Building ModelDesign
"%MODELCOMPILER%Opc.Ua.ModelCompiler.exe" compile -version v105 -d2 ".\ModelDesign.xml" -cg ".\ModelDesign.csv" -o2 .\
echo Success!



