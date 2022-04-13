<?php
require_once "db_cred.php";
require_once("smarty/libs/Smarty.class.php");

$err="";
$errorFlag = false;

#region error checking
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
#endregion
if(!$errorFlag){
    $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);
    $sql="Select Count(Username) From Accounts Where Email != ? And Username = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("ss", $_POST["email"], $_POST["user"]);
    $stmnt->bind_result($userCount);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    if($userCount == 0){

        $sql="Select Username From Accounts Where Email = ?";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("s", $_POST["email"]);
        $stmnt->bind_result($oldUser);
        $stmnt->execute();
        $stmnt->fetch();
        $stmnt->close();

        $sql="Update Accounts Set Username=?, FirstName=?, LastName=?, Password=?, Security_Question=?, Security_Answer=? Where Email=?";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("sssssss", $_POST["user"], $_POST["fname"], $_POST["lname"], $_POST["pass"], $_POST["secQuestion"], $_POST["secAnswer"], $_POST["email"]);
        $stmnt->execute();
        $stmnt->close();

        if(isset($_POST["blocked"]))
            $sql="Update Log Set Username = ?, Blocked = 1 Where Username = ?";
        else
            $sql="Update Log Set Username = ?, Blocked = 0 Where Username = ?";

        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("ss", $_POST["user"], $oldUser);
        $stmnt->execute();
        $stmnt->close();

        header('Location: adminSearch.html');
    }
    else{
        $errorFlag = true;
        $err.="Username already in use<br>";
    }
}
$smarty = new Smarty();
$smarty->setTemplateDir("smarty/templates");
$smarty->setCompileDir("smarty/templates_c");if($errorFlag){
$smarty->assign("errors", $err);
$smarty->display("error.html");
}
else{
    $smarty->assign("message", $msg);
    $smarty->display("success.html");
}
?>