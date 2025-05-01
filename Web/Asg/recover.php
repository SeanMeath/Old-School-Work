<?php
require_once "db_cred.php";
require_once("smarty/libs/Smarty.class.php");

$username = $_POST["user"];
$dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);

if(isset($_POST["answer"])){
    $answer = $_POST["answer"];

    $sql="Select Count(Username) From Accounts Where Username = ? and Security_Answer = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("ss", $username, $answer);
    $stmnt->bind_result($numAccounts);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    $smarty = new Smarty();
    $smarty->setTemplateDir("smarty/templates");
    $smarty->setCompileDir("smarty/templates_c");

    if($numAccounts == 1){

        $sql="Update Log Set Blocked = 0 Where Username = ?";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("s", $username);
        $stmnt->execute();
        $stmnt->close();

        $suc = "Your account has been successfully unlocked!<br><br>";
        $suc .= "<br><a href='login.html'>Login</a>";

        $smarty->assign("message", $suc);
        $smarty->display("success.html");
    }
    else{
        $smarty->assign("errors", "You did not enter the right answer to the security question!");
        $smarty->display("error.html");
    }
}
else{
    $sql="Select Security_Question From Accounts Where Username = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $username);
    $stmnt->bind_result($question);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    $smarty = new Smarty();
    $smarty->setTemplateDir("smarty/templates");
    $smarty->setCompileDir("smarty/templates_c");

    $smarty->assign("question", $question);
    $smarty->assign("user", $username);
    $smarty->display("recover.html");
}
?>