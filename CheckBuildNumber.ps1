if ($env:APPVEYOR_BUILD_NUMBER -ge 9) {
    $headers = @{
        "Authorization" = "Bearer $env:APPVEYOR_TOKEN"
        "Content-type" = "application/json"
        "Accept" = "application/json"
    }
    $build = @{
        nextBuildNumber = 0
    }
    $json = $build | ConvertTo-Json
    Invoke-RestMethod -Method Put "https://ci.appveyor.com/api/projects/$env:APPVEYOR_ACCOUNT_NAME/$env:APPVEYOR_PROJECT_SLUG/settings/build-number" -Body $json -Headers $headers
}