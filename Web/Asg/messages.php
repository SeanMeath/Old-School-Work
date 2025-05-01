<?php

require_once "db_cred.php";
require_once("smarty/libs/Smarty.class.php");

function get_new_messages($user){
    $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);

    $sql="SELECT MessageId FROM SeenMessages WHERE UserId = ? ORDER BY Id Desc";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $user);
    $stmnt->bind_result($lastMessage);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    $sql="SELECT MessageFrom, Message, TimeSent FROM Messages WHERE Id > ? ORDER BY TimeSent";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $lastMessage);
    $stmnt->bind_result($messageFrom, $message, $timeSent);
    $stmnt->execute();
    
    $messages = array(
        "status" => true,
        "messages" => array()
    );

    while($stmnt->fetch()){
        $messages["messages"][] = array(
            "from" => $messageFrom,
            "message" => $message,
            "timesent" => $timeSent
        );
    }

    $stmnt->close();

    var_dump($messages);
}

#get_new_messages($_POST["user"]);
get_new_messages("1");
?>