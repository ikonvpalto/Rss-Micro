wt -M `
    new-tab --title "Gateway"    -p "PowerShell" -d . dotnet run --no-build --project "./Gateway/Gateway.csproj" `; `
    new-tab --title "Downloader" -p "PowerShell" -d . dotnet run --no-build --project "./Downloader/Downloader.API/Downloader.API.csproj" `; `
    new-tab --title "Filter"     -p "PowerShell" -d . dotnet run --no-build --project "./Filter/Filter.API/Filter.API.csproj"
