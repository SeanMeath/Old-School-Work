Param ( [int]$bufferSize_ = 2000, [int]$threadCount_ = 8 )
$INPUT_FILE = "$PWD\asg2-200000.txt"
$ITEMS = Get-Content $INPUT_FILE
$stopwatch =  [system.diagnostics.stopwatch]::StartNew()
$pool = [RunspaceFactory]::CreateRunspacePool(1, $threadCount_)
$pool.Open()
$runspaces = @()

$scriptBlock = {
    Param ( [int[]]$threadData_, $host_ )

    function addPrimes(){
        Param ( [int[]]$dataSet_ )
        $runningTotal
        foreach($data_ in $dataSet_){
            if(isPrime $data_){
                $runningTotal += $data_
            }
        }
        return $runningTotal
    }

    function isPrime(){
        Param ( [int]$elem_ )
        if($elem_ -lt 2) { return $false }
        if($elem_ -eq 2) { return $true }
        if($elem_ % 2 -eq 0) { return $false }
        for($divisibleCheck = 3; $divisibleCheck * $divisibleCheck -le $elem_; $divisibleCheck += 2){
            if($elem_ % $divisibleCheck -eq 0) { return $false }
        }
        return $true
    }
    return addPrimes $threadData_
}

foreach($thread in 0..($ITEMS.Count / $bufferSize_)){
    $start = $thread * $bufferSize_
    $givenData = $ITEMS[$start..($start+$bufferSize_-1)]
    $runspace = [PowerShell]::Create()
    $null = $runspace.AddScript($scriptBlock)
    $null = $runspace.AddArgument($givenData)
    $runspace.RunspacePool = $pool
    $runspaces += [PSCustomObject]@{ 
        Pipe = $runspace
        Status = $runspace.BeginInvoke()
    }
}

while ($runspaces.Status.IsCompleted -contains $false) {}
$sum = 0
foreach($runspace in $runspaces){
    $sum += $runspace.Pipe.EndInvoke($runspace.Status)[1]
    $runspace.Pipe.Dispose()
}
$totalSecs =  [math]::Round($stopwatch.Elapsed.TotalSeconds,0)
$threadCount_.ToString() + " Threads took a total time of " + $totalSecs + " seconds to run"
"Total Sum of Primes = " + $sum
$sum
$pool.Close() 
$pool.Dispose()