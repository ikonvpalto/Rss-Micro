wt -M `
    new-tab --title "Gateway" -p "PowerShell" -d . dotnet run --no-build --project "./Gateway/Gateway.csproj" `; `
    new-tab --title "Downloader" -p "PowerShell" -d . dotnet run --no-build --project "./Downloader/Downloader.API/Downloader.API.csproj"
