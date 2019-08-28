Param ( [int]$max_ = 100000, [int]$numThreads_ = 8 )

Set-StrictMode -version latest
$ErrorActionPreference = 'Stop'
$DebugPreference = 'continue'
$VerbosePreference = 'continue'

$mtx = New-Object System.Threading.Mutex($false, "CounterMutex")
$fileName = "$PWD/Counter.txt"

0 > $fileName

$jobs = @()

$scriptBlock = {
    $counterFileName_ = $args[0]
    $maximum_ = $args[1]

    $myTimer = [PSCustomObject]@{
        start = 0;
        end = 0;
    }

    #Start of the process timer
    $myTimer.start = Get-Date

    $mutex_ = [System.Threading.Mutex]::OpenExisting("CounterMutex")

    while($true){
        if($mutex_.WaitOne(1)){
            [int]$counter = Get-Content $counterFileName_
            if($counter -lt $maximum_){
                $counter += 100
                $counter > $counterFileName_
                $mutex_.ReleaseMutex()
            }
            else{
                $mutex_.ReleaseMutex()

                #End of the process timer
                $myTimer.end = Get-Date

                return $myTimer
            }
        }
    }
}

$setupTime = 0
$runTime = 0
$exitTime = 0
$resultTime = 0
$cleanupTime = 0

#Start of the setup timer
$startTime = Get-Date

foreach($thread in 1..$numThreads_){
    $jobs += Start-Job $scriptBlock -ArgumentList $fileName, $max_
}

#End of the setup timer
$endTime = Get-Date
$setupTime = ($endTime - $startTime).TotalMilliseconds

Wait-Job $jobs | Out-Null

#Start of the time to receive data
$startTime = Get-Date

foreach($job in $jobs){
    $time = Receive-Job $job
    $runTime += ($time.end - $time.start).TotalMilliseconds

    #End of exit time
    $currentTime = Get-Date
    $exitTime += ($currentTime - $time.end).TotalMilliseconds
}

#End of the time to receive data
$endTime = Get-Date
$resultTime = ($endTime - $startTime).TotalMilliseconds

#Start of the time to cleanup
$startTime = Get-Date

#End of time to cleanup
$endTime = Get-Date
$cleanupTime = ($endTime - $startTime).TotalMilliseconds

Get-Content $fileName

# (C) 2012 Dr. Tobias Weltner
# you may freely use this code for commercial or non-commercial purposes at your own risk
# as long as you credit its original author and keep this comment block.
# For PowerShell training or PowerShell support, feel free to contact tobias.weltner@email.de

'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to set up background job', [math]::Round(($setupTime / $numThreads_), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to run code', [math]::Round(($runTime / $numThreads_), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to exit background job', [math]::Round(($exitTime / $numThreads_), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to receive results', [math]::Round(($resultTime), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to cleanup jobs', [math]::Round($cleanupTime, 2)