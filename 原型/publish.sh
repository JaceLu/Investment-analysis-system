rm -rf bin/Release/netcoreapp2.2/publish
dotnet publish -c Release
cd bin/Release/netcoreapp2.2/publish
rm ./config/conn.config
zip -r ./dexin.zip ./*
scp -P 1496 ./dexin.zip root@47.92.217.144:/var/sites/dexin
ssh -p 1496 root@47.92.217.144 "cd /var/sites/dexin;unzip -o dexin;"
ssh -p 1496 root@47.92.217.144 "service dexin restart"

