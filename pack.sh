#!/bin/bash
version=$1

if [ "$version" == "" ];then
   echo "version should be provided!"
   exit;
fi

commit=`git rev-parse --short HEAD`

echo "pack with commit: $commit"

# change commit hash
sed -i -e "s/commit=\"*\"/commit=\"$commit\"/g" nuget/Yozian.Extension.nuspec


dotnet pack src/Yozian.Extension/Yozian.Extension.csproj -p:PackageVersion=$version -o nuget


# recover
git checkout nuget/Yozian.Extension.nuspec
