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

$mtx = New-Object System.Threading.Mutex($false, "CounterMutex")

$pool = [RunspaceFactory]::CreateRunspacePool(1, $numThreads_)
$pool.Open()
$runspaces = @()

$scriptBlock = {
    Param ( [PSCustomObject]$myCounter_, $mutex_ )
    $ThreadID = [appdomain]::GetCurrentThreadId()
    $ProcessID = $PID
    start-sleep -s 1
    $myCounter_.host.ui.WriteVerboseLine("PID: " + $ProcessID + ", Tread ID: " + $ThreadID + " Starting, Initial Buffer Value: " + $myCounter_.num)
    while($true){
        if($mutex_.WaitOne(1)){
            if($myCounter_.num -lt $myCounter_.max){
                $MyCounter_.num++
                $mutex_.ReleaseMutex()
            }
            else{
                $mutex_.ReleaseMutex()
                break
            }
        }
    }
    $myCounter_.host.ui.WriteVerboseLine("PID: " + $ProcessID + ", Tread ID: " + $ThreadID + " Ending, Current Buffer Value: " + $myCounter_.num)
}

foreach($thread in 1..$numThreads_){
    'Creating thread #' + $thread
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

while ($runspaces.Status.IsCompleted -contains $false) {}
$myCounter.num
$pool.Close()
$pool.Dispose()