#
# awsecrdockerpush.ps1
#

Access Key ID	Secret Access Key
AKIAIOXP66NKYMBIBNQA	HZXPQyj70Eb9KYRrj7iV105AoJIL5XM6sz07sfWN


cd C:\Users\prave\source\repos\ujetsapi\ujetsapi
aws ecr get-login --no-include-email --region us-east-2
Invoke-Expression -Command (aws ecr get-login --no-include-email --region us-east-2)
docker build -t ujetsapi .
docker tag ujetsapi:latest 471861426143.dkr.ecr.us-east-2.amazonaws.com/ujetsapi:latest
docker push 471861426143.dkr.ecr.us-east-2.amazonaws.com/ujetsapi:latest