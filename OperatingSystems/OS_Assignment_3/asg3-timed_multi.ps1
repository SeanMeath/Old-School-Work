Param ( [int]$max_ = 100000, [int]$numThreads_ = 8 )

Set-StrictMode -version latest
$ErrorActionPreference = 'Stop'
$DebugPreference = 'continue'
$VerbosePreference = 'continue'

$myCounter = [PSCustomObject]@{
    num = 0;
    max = $max_;
    host = $host
}

$mtx = New-Object System.Threading.Mutex($false, "CounterMutex")

$pool = [RunspaceFactory]::CreateRunspacePool(1, $numThreads_)
$pool.Open()
$runspaces = @()

$scriptBlock = {
    Param ( [PSCustomObject]$myCounter_, $mutex_ )

    $myTimer = [PSCustomObject]@{
        start = 0;
        end = 0;
    }

    #Start of the process timer
    $myTimer.start = Get-Date

    while($true){
        if($mutex_.WaitOne(1)){
            if($myCounter_.num -lt $myCounter_.max){
                $MyCounter_.num++
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
    $runspace = [PowerShell]::Create()
    $null = $runspace.AddScript($scriptBlock)
    $null = $runspace.AddArgument($myCounter)
    $null = $runspace.AddArgument($mtx)
    $runspace.RunspacePool = $pool
    $runspaces += [PSCustomObject]@{ 
        Pipe = $runspace
        Status = $runspace.BeginInvoke()
    }
}

#End of the setup timer
$endTime = Get-Date
$setupTime = ($endTime - $startTime).TotalMilliseconds

while ($runspaces.Status.IsCompleted -contains $false) {}

#Start of the time to receive data
$startTime = Get-Date

foreach($runspace in $runspaces){
    $time = $runspace.Pipe.EndInvoke($runspace.Status)[0]
    $runTime += ($time.end - $time.start).TotalMilliseconds

    #End of exit time
    $currentTime = Get-Date
    $exitTime += ($currentTime - $time.end).TotalMilliseconds
    $runspace.Pipe.Dispose()
}

#End of the time to receive data
$endTime = Get-Date
$resultTime = ($endTime - $startTime).TotalMilliseconds

#Start of the time to cleanup
$startTime = Get-Date

$pool.Close()
$pool.Dispose()

#End of time to cleanup
$endTime = Get-Date
$cleanupTime = ($endTime - $startTime).TotalMilliseconds

$myCounter.num


# (C) 2012 Dr. Tobias Weltner
# you may freely use this code for commercial or non-commercial purposes at your own risk
# as long as you credit its original author and keep this comment block.
# For PowerShell training or PowerShell support, feel free to contact tobias.weltner@email.de

'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to set up background job', [math]::Round(($setupTime / $numThreads_), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to run code', [math]::Round(($runTime / $numThreads_), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to exit background job', [math]::Round(($exitTime / $numThreads_), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to receive results', [math]::Round(($resultTime), 2)
'{0,-30} : {1,10:#,##0.00} ms' -f 'Time to cleanup runspace', [math]::Round($cleanupTime, 2)