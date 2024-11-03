# Save this script as run_concurrent.ps1

param (
    [int]$numProcesses = 4,
    [int]$iterations = 100
)

$processes = @()
$results = @()
$filePath = "C:\Users\noamt\source\repos\CPU-Process\CPU-Process\bin\Debug\net8.0\CPU-Process.exe"

# Start the concurrent processes
for ($i = 0; $i -lt $numProcesses; $i++) {
    $process = Start-Process -FilePath $filePath -ArgumentList $iterations -NoNewWindow -RedirectStandardOutput "output_$i.txt" -PassThru
    $processes += $process
}

# Wait for all processes to complete
$processes | ForEach-Object { $_.WaitForExit() }

# Read and parse the output files
for ($i = 0; $i -lt $numProcesses; $i++) {
    $output = Get-Content "output_$i.txt"
    Write-Output "Reading output from output_$i.txt:"
    Write-Output $output
    $match = [regex]::Match($output, "Time for $iterations iterations: ([\d\.]+) ms")
    if ($match.Success) {
        Write-Output "Match found!"
        $results += [double]$match.Groups[1].Value
    } else {
        Write-Output "No match found in output_$i.txt"
    }
}

# Check if results array is empty
if ($results.Count -eq 0) {
    Write-Output "No valid results found."
} else {
    # Calculate the average time
    $averageTime = ($results | Measure-Object -Average).Average
    Write-Output "Average time for $iterations iterations with $numProcesses concurrent processes: $averageTime ms"
}

# Clean up the output files
for ($i = 0; $i -lt $numProcesses; $i++) {
    Remove-Item "output_$i.txt"
}