Param ( [int]$max_ = 10000, [int]$numThreads_ = 8 )

Set-StrictMode -version latest
$ErrorActionPreference = 'Stop'
$DebugPreference = 'continue'
$VerbosePreference = 'continue'

$myCounter = [PSCustomObject]@{
    num = 0;
    max = $max_;
    host = $host
}

$pool = [RunspaceFactory]::CreateRunspacePool(1, $numThreads_)
$pool.Open()
$runspaces = @()

$scriptBlock = {
    Param ( [PSCustomObject]$myCounter_ )
    $ThreadID = [appdomain]::GetCurrentThreadId()
    $ProcessID = $PID
    start-sleep -s 1
    $myCounter_.host.ui.WriteVerboseLine("PID: " + $ProcessID + ", Tread ID: " + $ThreadID + " Starting, Initial Buffer Value: " + $myCounter_.num)
    while($myCounter_.num -lt $myCounter_.max){
        $MyCounter_.num++
    }
    $myCounter_.host.ui.WriteVerboseLine("PID: " + $ProcessID + ", Tread ID: " + $ThreadID + " Ending, Current Buffer Value: " + $myCounter_.num)
}

foreach($thread in 1..$numThreads_){
    'Creating thread #' + $thread
    $runspace = [PowerShell]::Create()
    $null = $runspace.AddScript($scriptBlock)
    $null = $runspace.AddArgument($myCounter)
    $runspace.RunspacePool = $pool
    $runspaces += [PSCustomObject]@{ 
        Pipe = $runspace
        Status = $runspace.BeginInvoke()
    }
}

while ($runspaces.Status.IsCompleted -contains $false) {}
$myCounter.num
$pool.Close() 
$pool.Dispose()