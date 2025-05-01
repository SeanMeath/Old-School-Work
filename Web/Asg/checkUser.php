<?php
    require_once "db_cred.php";
    $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);

    $sql="Select Count(Username) From Accounts Where Username = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $_POST["user"]);
    $stmnt->bind_result($userCount);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    $dbConn->close();

    if($userCount > 0){
        echo("true");
    }
    else{
        echo("false");
    }
?>