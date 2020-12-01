
rm -rf ./bin
dotnet publish -c Release 
cd  bin/Release/netcoreapp3.1/publish/
rm -rf config/conn.config
zip -r ./Investment.zip ./*
start .
scp  ./Investment.zip root@223.247.140.87:/var/sites/Investment
ssh  root@223.247.140.87 "cd /var/sites/Investment;unzip -o Investment.zip;pm2 restart Investment;"

