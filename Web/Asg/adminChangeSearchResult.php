<?php
require_once "db_cred.php";
require_once("smarty/libs/Smarty.class.php");

//Delete Button Was Pressed
if(isset($_POST["delete"])){
    $toDelete = "";
    $first = true;
    for($counter = 0; $counter < $_POST["totalRows"]; $counter++){
        if(isset($_POST["delete_$counter"])){
            if(!$first){
                $toDelete.=", ";
            }
            $first = false;
            $toDelete.=$_POST["email_$counter"];
        }
    }
    
    $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);
    $sql="Select Username from Accounts Where Email in (?)";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $toDelete);
    $stmnt->bind_result($users);
    $stmnt->execute();

    $first = true;
    $userNames = "";

    while($stmnt->fetch()){
        if(!$first){
            $userNames.=", ";
        }
        $first = false;
        $userNames.=$users;
    }
    $stmnt->close();

    $sql="Delete from Log Where Username in (?)";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $userNames);
    $stmnt->execute();

    $sql="Delete from Accounts Where Email in (?)";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $toDelete);
    $stmnt->execute();

    header('Location: adminSearch.html');
}
else{
    $toModify = "";
    for($counter = 0; $counter < $_POST["totalRows"]; $counter++){
        if(isset($_POST["modify_$counter"])){
            $toModify = $_POST["email_$counter"];
            break;
        }
    }
    $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);

    $sql="Select FirstName, LastName, Username, Password, Security_Question, Security_Answer From Accounts Where Email = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $toModify);
    $stmnt->bind_result($firstName, $lastName, $username, $password, $question, $answer);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();

    $sql="Select Blocked From Log Where Username = ?";
    $stmnt=$dbConn->prepare($sql);
    $stmnt->bind_param("s", $username);
    $stmnt->bind_result($blocked);
    $stmnt->execute();
    $stmnt->fetch();
    $stmnt->close();
    
    $smarty = new Smarty();
    $smarty->setTemplateDir("smarty/templates");
    $smarty->setCompileDir("smarty/templates_c");
    
    $smarty->assign("user", $username);
    $smarty->assign("fName", $firstName);
    $smarty->assign("lName", $lastName);
    $smarty->assign("email", $toModify);
    $smarty->assign("password", $password);
    $smarty->assign("question", $question);
    $smarty->assign("answer", $answer);
    $checked = "";
    if($blocked == 1){
        $checked = "checked";
    }
    $smarty->assign("blocked", $checked);
    $smarty->display("adminEdit.html");
}
?>