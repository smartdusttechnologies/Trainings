@echo off 
title Docker Compose Runner 

echo Docker Compose in root file is  running ....

docker compose -f docker-compose.common.yml up -d

echo Docker Compose in root file run successfully ....

pause


