<?php
require_once "db_cred.php";
require_once("smarty/libs/Smarty.class.php");

$err="";
$suc=$_POST["user"]."'s account has been created!<br><br>";
$suc.="<br><a href='login.html'>Login</a>";
$errorFlag = false;

if( strlen($_POST["fname"]) == 0){
    $errorFlag = true;
    $err.="First name Empty<br>";
}
if( strlen($_POST["lname"]) == 0){
    $errorFlag = true;
    $err.="Last Name Empty<br>";
}
if( strlen($_POST["user"]) == 0){
    $errorFlag = true;
    $err.="User Empty<br>";
}
if(strlen($_POST["email"]) == 0){
    $errorFlag = true;
    $err.="Email Empty<br>";
}
if(strlen($_POST["pass"]) == 0){
    $errorFlag = true;
    $err.="Password Empty<br>";
}
if(strlen($_POST["secQuestion"]) == 0){
    $errorFlag = true;
    $err.="Security Question Empty<br>";
}
if(strlen($_POST["secAnswer"]) == 0 ){
    $errorFlag = true;
    $err.="Security Answer Empty<br>";
}
if( $_POST["pass"] != $_POST["confirmPass"] ){
    $errorFlag = true;
    $err.="Passwords dont match<br>";
}
if(!$errorFlag){
    $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);
    $sql="Select Count(Email) From Accounts Where Email = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $_POST["email"]);
    $stmnt->bind_result($emailCount);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    $sql="Select Count(Username) From Accounts Where Username = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $_POST["user"]);
    $stmnt->bind_result($userCount);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();
    if($emailCount > 0){
        $errorFlag = true;
        $err.="Error, Email already in use.<br>";
    }
    if($userCount > 0){
        $errorFlag = true;
        $err.="Error, Username already in use.<br>";
    }
    if($userCount == 0 && $emailCount == 0){
        $sql="Insert Into Accounts (FirstName, LastName, Username, Email, Password, Security_Question, Security_Answer) Values (?,?,?,?,?,?,?)";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("sssssss", $_POST["fname"], $_POST["lname"], $_POST["user"], $_POST["email"], $_POST["pass"], $_POST["secQuestion"], $_POST["secAnswer"]);
        $stmnt->execute();
        $stmnt->close();

        $time = time();

        $sql="Insert Into Log (UserName) Values (?)";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("s", $_POST["user"]);
        $stmnt->execute();
        $stmnt->close();
    }
    $dbConn->close();
}

$smarty = new Smarty();
$smarty->setTemplateDir("smarty/templates");
$smarty->setCompileDir("smarty/templates_c");

if ($errorFlag == true) {
    $smarty->assign("errors", $err);
    $smarty->display("error.html");
} else {
    $smarty->assign("message", $suc);
    $smarty->display("success.html");
}
