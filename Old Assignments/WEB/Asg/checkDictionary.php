<?php
    $lines = file('linuxwords.txt');
    foreach($lines as $line)
    {
        if(strtolower($_POST['word']) == strtolower(substr($line, 0, -1))
        || strtolower($_POST['word']) == strrev(strtolower(substr($line, 0, -1)))){
            echo("true");
            break;
        }
    }
?>