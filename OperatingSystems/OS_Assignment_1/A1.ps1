set-strictmode -version latest
$DebugPreference        = 'SilentlyContinue'# disable write-debug output
#$DebugPreference        = 'Continue'        # enable write-debug output
$ErrorActionPreference  = 'Stop'            # https://stackoverflow.com/questions/15545429/erroractionpreference-and-erroraction-silentlycontinue-for-get-pssessionconfigur
                                            # The ErrorAction can be used to convert non-terminating errors to terminating errors using the parameter value Stop. 
                                            # It can't help you ignore terminating errors. If you want to ignore, use a try { Stop-Transcript } catch {}


Clear-Host
Get-Random -maximum 100 -SetSeed 2019 | out-null
$TOTAL_OPS = 10000
$NUM_REGISTERS = 32
$CACHE_SIZE = 1024

# Response time of storage methods
$REGISTER_ACCESS_TIME = 0.25
$CACHE_ACCESS_TIME = 2
$RAM_ACCESS_TIME = 100
$DISK_ACCESS_TIME = 40000

# Time to transfer a single 64 bit word
$CACHE_BANDWIDTH_TIME   = ([Math]::Pow(10,9) / (700*[Math]::Pow(1024,3) / 8))
$RAM_BANDWIDTH_TIME     = ([Math]::Pow(10,9) / (15*[Math]::Pow(1024,3) / 8))
$DISK_BANDWIDTH_TIME    = ([Math]::Pow(10,9) / (200*[Math]::Pow(1024,2) / 8))

$RAM_HIT_PERCENTAGE = 90
$CACHE_REPLACEMENT_POLICIES = @('FIFO','LFU')
$INPUTFILE = "$PWD\dataset.txt"

# $storage is the ONLY permitted global variable (as opposed to global constants)
$storage = [PSCustomObject]@{
    Registers   = New-Object System.Collections.Generic.List[Int]
    Cache       = New-Object System.Collections.ArrayList
} 



function setRegisters($item_)
{
    $found = $storage.Registers.Contains($item_)
    if($found){
        $storage.Registers.Remove($item_)
    }
    elseif($storage.Registers.Count -eq $NUM_REGISTERS){
        $storage.Registers.RemoveAt(0)
    }
    $storage.Registers.Add($item_) | Out-Null
    write-debug $found
    return $found
}

function fetchEmptyStatsObject(){
    $stats = [PSCustomObject]@{ 
        'totalTime' = 0;    'regHits' = 0;      'cacheHits' = 0;    'ramHits' = 0;
        'diskHits'  = 0;     'regTime' = 0;     'cacheTime' = 0;    'ramTime' = 0;
        'diskTime'  = 0;
    }

    $stats | Add-Member -MemberType ScriptMethod -Name "show" -Force -Value {
        # OUTPUTS THE $STATS PROPERTIES FORMATED AS HE DID
        # Outputs the cache stats for a particular cache type
        "{0,-25} {1,39}" -f "Overall Performance (ns):", [math]::Round($stats.totalTime, 2)
        "{0,-25} {1,39}" -f "Cache Hit Ratio:", ($stats.cacheHits / ($TOTAL_OPS - $stats.regHits)).ToString("P2")
        ""
        "{0,-25} {1,39}" -f "Cache Hits:", ([string]$stats.cacheHits + " (" + ($stats.cacheHits / $TOTAL_OPS).ToString("P2") + ")")
        "{0,-25} {1,39}" -f "Cache Misses:", ($TOTAL_OPS - ($stats.regHits + $stats.cacheHits))
        ""
        "{0,-25} {1,39}" -f "Register Hits:", ([string]$stats.regHits + " (" + ($stats.regHits / $TOTAL_OPS).ToString("P2") + ")")
        "{0,-25} {1,39}" -f "RAM Hits:", ([string]$stats.ramHits + " (" + ($stats.ramHits / $TOTAL_OPS).ToString("P2") + ")")
        "{0,-25} {1,39}" -f "Disk Hits:", ([string]$stats.diskHits + " (" + ($stats.diskHits / $TOTAL_OPS).ToString("P2") + ")")
        ""
        "{0,-25} {1,39}" -f "Register Time:", ([string][math]::Round($stats.regTime, 2) + " NS" + " (" + ($stats.regTime/$stats.totalTime).ToString("P2") + ")")
        "{0,-25} {1,39}" -f "Cache Time:", ([string][math]::Round($stats.cacheTime, 2) + " NS" + " (" + ($stats.cacheTime/$stats.totalTime).ToString("P2") + ")")
        "{0,-25} {1,39}" -f "RAM Time:", ([string][math]::Round($stats.ramTime, 2) + " NS" + " (" + ($stats.ramTime/$stats.totalTime).ToString("P2") + ")")
        "{0,-25} {1,39}" -f "Disk Time:", ([string][math]::Round($stats.diskTime, 2) + " NS" + " (" + ($stats.diskTime/$stats.totalTime).ToString("P2") + ")")
    }
    return $stats
}


function setCache($cacheType_, $item_)
{
    if($cacheType_ -eq 'FIFO'){
        setFIFOCache $item_
    }
    elseif($cacheType_ -eq 'LFU'){
        setLFUCache $item_
    }
}

function setFIFOCache($item_){
    $found = $storage.Cache.Contains($item_)
    if($found){
        $storage.Cache.Remove($item_)
    }
    elseif($storage.Cache.Count -eq $CACHE_SIZE){
        $storage.Cache.RemoveAt(0)
    }
    $storage.Cache.Add($item_) | Out-Null
    write-debug $found
    return $found
}

function setLFUCache($item_){
    # Given an item request, set the cache
    # TODO: Code me :)
    if(!$Storage.Cache.count){                  #EMPTY CACHE
        $d = New-Object System.Collections.ArrayList
        $Storage.Cache.Add($d)  |Out-Null
    }
    $count = 0
    foreach($subArr in $Storage.Cache){
        $count += $subArr.count
    }
    $found = $false
    foreach($i in 0 .. ($Storage.Cache.count-1)){
        $subArr = $Storage.Cache[$i]
        if($found = $subArr.Contains($item_)){   #FOUND
            $subArr.Remove($item_)      |Out-Null
            if($Storage.Cache.count - 1 -eq $i){        #NEW OCCURENCE AT MAX OCCURENCE
                $Storage.Cache.add((New-Object System.Collections.ArrayList))   |Out-Null
            }
            $Storage.Cache[$i+1].add($item_)    |Out-Null
            break;
        }
    }
    if(!$found){                                #NOT FOUND
        if($count -eq $CACHE_SIZE){             #CACHE FULL
            foreach($subArr in $Storage.Cache){
                if($subArr.count){
                    $subArr.RemoveAt(0)     |Out-Null
                    break
                }
            }
        }
        $Storage.Cache[0].add($item_)   |Out-Null
    }
    return $found
}


function testSetLFUCache(){
    $item = 12
    $reps = 4

    for($i=0; $i -lt $reps; $i++){
        setCache 'LFU' $item | out-null
    }

    if($storage.cache[$reps-1][0] -ne $item){
        Write-Error "Argh! Weird cache values, broken setLFUCache()"
        exit
    }

    if($storage.cache[0].Count -ne 0 -or $storage.cache[1].Count -ne 0 -or $storage.cache[2].Count -ne 0){
        Write-Error "Argh! Weird cache values, broken setLFUCache()"
        exit
    }
}



function clearStorage($cacheT_){
    #TODO:
    # Clears the storage object so that it can be reused between cache types
    $storage.Registers.Clear()
    $storage.Cache.Clear()
    if($cacheT_ -eq 'LFU'){
        $storage.Cache = New-Object System.Collections.ArrayList
    }
    # $storage.Cache.Add((New-Object System.Collections.ArrayList))
}


#-----------------------------------------------------
# MAIN BLOCK------------------------------------------
#-----------------------------------------------------
testSetLFUCache
$items = Get-Content $INPUTFILE  # All of the data items to process
foreach($cacheType in $CACHE_REPLACEMENT_POLICIES){
    clearStorage $cacheType
    $stats = fetchEmptyStatsObject
    Write-Output "________________________________________________________________________"
    Write-Output "${cacheType}:"

    for($opNum = 0; $opNum -lt $TOTAL_OPS; $opNum++){
        $currItem = $items[$opNum]
        $stats.regTime += $REGISTER_ACCESS_TIME
        $stats.totalTime += $REGISTER_ACCESS_TIME
        if(setRegisters $currItem $opNum){ # item was in registers, we're done!
            $stats.regHits++
            continue
        }
        $stats.cacheTime += $CACHE_ACCESS_TIME + $CACHE_BANDWIDTH_TIME
        $stats.totalTime += $CACHE_ACCESS_TIME + $CACHE_BANDWIDTH_TIME
        if(setCache $cacheType $currItem){
            $stats.cacheHits++
            continue
        }
        $stats.ramTime += $RAM_ACCESS_TIME + $RAM_BANDWIDTH_TIME
        $stats.totalTime += $RAM_ACCESS_TIME + $RAM_BANDWIDTH_TIME
        if((Get-Random -maximum 100) -lt $RAM_HIT_PERCENTAGE){ # if in RAM...
            $stats.ramHits++
            continue
        }
        $stats.diskTime += $DISK_ACCESS_TIME + $DISK_BANDWIDTH_TIME
        $stats.totalTime += $DISK_ACCESS_TIME + $DISK_BANDWIDTH_TIME
        $stats.diskHits++
    }
    $stats.show()
}

#   $Cache = List[List[int]]
# cache hit ratio (registry + cache hits / total items)



