<?php
require_once "db_cred.php";
require_once "smarty/libs/Smarty.class.php";

#Gets the username and password
#via post to check validity
$username = $_POST["user"];
$password = $_POST["pass"];

$errorFlag = false;

#Number of fails till account lock
$MAXFAILS = 5;

#Default empty messages
$err="";
$msg="";

#Get If Account Exists
$dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);
$sql="Select Count(Username) From Accounts Where Username = ?";
$stmnt=$dbConn->prepare($sql);
$stmnt->bind_param("s", $username);
$stmnt->bind_result($userCount);
$stmnt->execute();
$stmnt->fetch();
$stmnt->close();

$smarty = new Smarty();
$smarty->setTemplateDir("smarty/templates");
$smarty->setCompileDir("smarty/templates_c");

#If Account Exists
if($userCount == 1){
    #Get If Password Matches
    $sql="Select Count(Username) From Accounts Where Username = ? And Password = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("ss", $username, $password);
    $stmnt->bind_result($userCount);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    #If Password Matches
    if($userCount == 1){

        #Get If Account Is Blocked
        $sql="Select Blocked From Log Where Username = ?";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("s", $username);
        $stmnt->bind_result($isLocked);
        $stmnt->execute();
        $stmnt->fetch();
        $stmnt->close();

        #If Account Is Not Blocked
        if($isLocked == 0){

            #Set online cookie
            setcookie("user", $username, time()+3600);

            #Get last login
            $sql="Select LastLogin From Log Where Username = ?";
            $stmnt=$dbConn->prepare($sql);
            $stmnt->bind_param("s", $username);
            $stmnt->bind_result($lastLog);
            $stmnt->execute();
            $stmnt->fetch();
            $stmnt->close();

            #Get credentials
            $sql="Select FirstName, LastName From Accounts Where Username = ?";
            $stmnt=$dbConn->prepare($sql);
            $stmnt->bind_param("s", $username);
            $stmnt->bind_result($first, $last);
            $stmnt->execute();
            $stmnt->fetch();
            $stmnt->close();

            #Set up message
            $suc = "Welcome $first $last<br>";

            #Set up last login date
            if($lastLog == null){
                $suc.="It's your first time loggin in!<br><br>";
            }
            else{
                $suc.="Last Login: $lastLog<br><br>";
            }
            $suc.="<br><a href='edit.php'>I wish to change my account</a>";

            #Update login information
            $sql="Update Log Set NumUnsuccessfulLogins = 0, NumLogins = NumLogins + 1, LastLogin = CURRENT_TIMESTAMP Where Username = ?";
            $stmnt=$dbConn->prepare($sql);
            $stmnt->bind_param("s", $username);
            $stmnt->execute();
            $stmnt->close();

        }
        #Account Blocked
        else{
            $errorFlag = true;
            $smarty->assign("recUser", $username);
            $err = "Sorry Account Has Been Locked! :(";
        }
    }
    #Wrong Password
    else{

        setcookie("user", $username, time()-3600);

        $errorFlag = true;

        $sql="Update Log Set NumUnsuccessfulLogins = NumUnsuccessfulLogins + 1 Where Username = ?";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("s", $username);
        $stmnt->execute();
        $stmnt->close();

        $sql="Select NumUnsuccessfulLogins From Log Where Username = ?";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("s", $username);
        $stmnt->bind_result($numFail);
        $stmnt->execute();
        $stmnt->fetch();
        $stmnt->close();

        $err = "Invalid Email and/or Password!<br>Please try again<br>";

        if($numFail >= $MAXFAILS){
            $sql="Update Log Set Blocked = 1 Where Username = ?";
            $stmnt=$dbConn->prepare($sql);
            $stmnt->bind_param("s", $username);
            $stmnt->execute();
            $stmnt->close();
        }
    }
}
#Account Does Not Exist
else{
    $errorFlag = true;
    $err = "Invalid Email and/or Password!<br>Please try again<br>";
}

if ($errorFlag == true) {
    $smarty->assign("errors", $err);
    $smarty->display("error.html");
}
else {
    $smarty->assign("message", $suc);
    $smarty->display("success.html");
}
?>