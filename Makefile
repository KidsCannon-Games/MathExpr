CS_FILES = $(shell find MathExpr/ -type f -name *.cs)
CS_TEST_FILES = $(shell find MathExprTest/ -type f -name *.cs)

all: MathExpr/bin/Debug/MathExpr.dll MathExprTest/bin/Debug/MathExprTest.dll

MathExpr/bin/Debug/MathExpr.dll: $(CS_FILES)
	xbuild MathExpr/MathExpr.csproj > /dev/null
MathExprTest/bin/Debug/MathExprTest.dll: $(CS_TEST_FILES)
	xbuild MathExprTest/MathExprTest.csproj > /dev/null

.PHONY: test
test: all
	mono packages/NUnit.Runners.2.6.4/tools/nunit-console-x86.exe MathExprTest/bin/Debug/MathExprTest.dll
