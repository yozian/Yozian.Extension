#!/bin/bash
version=$1

if [ "$version" == "" ];then
   echo "version should be provided!"
   exit;
fi

dotnet nuget push "nuget/Yozian.Extension.$version.nupkg" --source https://api.nuget.org/v3/index.json
