
rm -rf ./bin
dotnet publish -c Release 
cd  bin/Release/netcoreapp3.1/publish/
#rm -rf config/conn.config
zip -r ./Investment.zip ./*
#cp ColorStar.zip ../../../../../publish/ 
#cd  ../../../../../publish 
start .
scp -P 1496 ./Investment.zip root@47.92.217.144:/var/sites/Investment
ssh -p 1496 root@47.92.217.144 "cd /var/sites/Investment;unzip -o Investment.zip;pm2 restart Investment;"

#start mstsc /v 47.110.55.220