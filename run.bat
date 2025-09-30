@echo off

echo "Setup rabbit"
call docker run -d --name rabbit-mq-tourapp -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password -p 52001:5672 -p 52002:15672 rabbitmq:4-management || GOTO FAILED

echo "Allow rabbit to start"
timeout 5

echo "Run services"
start cmd /k "dotnet run --project src\TourApp.Frontend\TourApp.Blazor"
start cmd /k "dotnet run --project src\TourApp.Admin.Console"
start cmd /k "dotnet run --project src\TourApp.Email.Console"

exit /b 0

:FAILED

echo "Could not start rabbit"

exut /b 1
